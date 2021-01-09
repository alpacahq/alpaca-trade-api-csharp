using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace UsageExamples
{
    // This version of the mean reversion example algorithm utilizes Polygon data that
    // is available to users who have a funded Alpaca brokerage account. By default, it
    // is configured to use the paper trading API, but you can change it to use the live
    // trading API by setting the API_URL.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "UnusedVariable")]
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    internal sealed class MeanReversionBrokerage : IDisposable
    {
        private const String API_KEY = "REPLACEME";

        private const String API_SECRET = "REPLACEME";

        private const String symbol = "AAPL";

        private const Decimal scale = 200;

        private IPolygonDataClient polygonDataClient;

        private IAlpacaTradingClient alpacaTradingClient;

        private IAlpacaStreamingClient alpacaStreamingClient;

        private IPolygonStreamingClient polygonStreamingClient;

        private Guid lastTradeId = Guid.NewGuid();

        private Boolean lastTradeOpen;

        private readonly List<Decimal> closingPrices = new List<Decimal>();

        public async Task Run()
        {
            alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET));

            polygonDataClient = Environments.Paper.GetPolygonDataClient(API_KEY);

            // Connect to Alpaca's websocket and listen for updates on our orders.
            alpacaStreamingClient = Environments.Paper.GetAlpacaStreamingClient(new SecretKey(API_KEY, API_SECRET));

            alpacaStreamingClient.ConnectAndAuthenticateAsync().Wait();

            alpacaStreamingClient.OnTradeUpdate += HandleTradeUpdate;

            // First, cancel any existing orders so they don't impact our buying power.
            var orders = await alpacaTradingClient.ListOrdersAsync(new ListOrdersRequest());
            foreach (var order in orders)
            {
                await alpacaTradingClient.DeleteOrderAsync(order.OrderId);
            }

            // Figure out when the market will close so we can prepare to sell beforehand.
            var calendars = (await alpacaTradingClient
                .ListCalendarAsync(new CalendarRequest().SetTimeInterval(DateTime.Today.GetInclusiveIntervalFromThat())))
                .ToList();
            var calendarDate = calendars.First().TradingDateUtc;
            var closingTime = calendars.First().TradingCloseTimeUtc;

            closingTime = new DateTime(calendarDate.Year, calendarDate.Month, calendarDate.Day, closingTime.Hour, closingTime.Minute, closingTime.Second);

            var today = DateTime.Today;
            // Get the first group of bars from today if the market has already been open.
            var bars = await polygonDataClient.ListAggregatesAsync(
                new AggregatesRequest(symbol, new AggregationPeriod(1, AggregationPeriodUnit.Minute))
                    .SetInclusiveTimeInterval(today, today.AddDays(1)));
            var lastBars = bars.Items.Skip(Math.Max(0, bars.Items.Count() - 20));
            foreach (var bar in lastBars)
            {
                if (bar.TimeUtc?.Date == today)
                {
                    closingPrices.Add(bar.Close);
                }
            }

            Console.WriteLine("Waiting for market open...");
            await AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Connect to Polygon's websocket and listen for price updates.
            polygonStreamingClient = Environments.Live.GetPolygonStreamingClient(API_KEY);

            polygonStreamingClient.ConnectAndAuthenticateAsync().Wait();
            Console.WriteLine("Polygon client opened.");
            polygonStreamingClient.MinuteAggReceived += async (agg) =>
            {
                // If the market's close to closing, exit position and stop trading.
                TimeSpan minutesUntilClose = closingTime - DateTime.UtcNow;
                if (minutesUntilClose.TotalMinutes < 15)
                {
                    Console.WriteLine("Reached the end of trading window.");
                    await ClosePositionAtMarket();
                    await polygonStreamingClient.DisconnectAsync();
                }
                else
                {
                    // Decide whether to buy or sell and submit orders.
                    await HandleMinuteAgg(agg);
                }
            };
            polygonStreamingClient.SubscribeMinuteAgg(symbol);
        }

        public void Dispose()
        {
            alpacaTradingClient?.Dispose();
            polygonDataClient?.Dispose();
            alpacaStreamingClient?.Dispose();
            polygonStreamingClient?.Dispose();
        }

        // Waits until the clock says the market is open.
        // Note: if you wanted the algorithm to start trading right at market open, you would instead
        // use the method restClient.GetCalendarAsync() to get the open time and schedule execution
        // of your code based on that. However, this algorithm does not start trading until at least
        // 20 minutes after the market opens.
        private async Task AwaitMarketOpen()
        {
            while (!(await alpacaTradingClient.GetClockAsync()).IsOpen)
            {
                await Task.Delay(60000);
            }
        }

        // Determine whether our position should grow or shrink and submit orders.
        private async Task HandleMinuteAgg(IStreamAgg agg)
        {
            closingPrices.Add(agg.Close);
            if (closingPrices.Count > 20)
            {
                closingPrices.RemoveAt(0);

                Decimal avg = closingPrices.Average();
                Decimal diff = avg - agg.Close;

                // If the last trade hasn't filled yet, we'd rather replace
                // it than have two orders open at once.
                if (lastTradeOpen)
                {
                    // We need to wait for the cancel to process in order to avoid
                    // having long and short orders open at the same time.
                    Boolean res = await alpacaTradingClient.DeleteOrderAsync(lastTradeId);
                }

                // Make sure we know how much we should spend on our position.
                var account = await alpacaTradingClient.GetAccountAsync();
                Decimal buyingPower = account.BuyingPower;
                Decimal equity = account.Equity;
                Int64 multiplier = account.Multiplier;

                // Check how much we currently have in this position.
                Int32 positionQuantity = 0;
                Decimal positionValue = 0;
                try
                {
                    var currentPosition = await alpacaTradingClient.GetPositionAsync(symbol);
                    positionQuantity = currentPosition.Quantity;
                    positionValue = currentPosition.MarketValue;
                }
                catch (Exception)
                {
                    // No position exists. This exception can be safely ignored.
                }

                if (diff <= 0)
                {
                    // Price is above average: we want to short.
                    if (positionQuantity > 0)
                    {
                        // There is an existing long position we need to dispose of first
                        Console.WriteLine($"Removing {positionValue:C2} long position.");
                        await SubmitOrder(positionQuantity, agg.Close, OrderSide.Sell);
                    }
                    else
                    {
                        // Allocate a percent of portfolio to short position
                        Decimal portfolioShare = -1 * diff / agg.Close * scale;
                        Decimal targetPositionValue = -1 * equity * multiplier * portfolioShare;
                        Decimal amountToShort = targetPositionValue - positionValue;

                        if (amountToShort < 0)
                        {
                            // We want to expand our existing short position.
                            amountToShort *= -1;
                            if (amountToShort > buyingPower)
                            {
                                amountToShort = buyingPower;
                            }
                            Int32 qty = (Int32)(amountToShort / agg.Close);
                            Console.WriteLine($"Adding {qty * agg.Close:C2} to short position.");
                            await SubmitOrder(qty, agg.Close, OrderSide.Sell);
                        }
                        else
                        {
                            // We want to shrink our existing short position.
                            Int32 qty = (Int32)(amountToShort / agg.Close);
                            if (qty > -1 * positionQuantity)
                            {
                                qty = -1 * positionQuantity;
                            }
                            Console.WriteLine($"Removing {qty * agg.Close:C2} from short position");
                            await SubmitOrder(qty, agg.Close, OrderSide.Buy);
                        }
                    }
                }
                else
                {
                    // Allocate a percent of our portfolio to long position.
                    Decimal portfolioShare = diff / agg.Close * scale;
                    Decimal targetPositionValue = equity * multiplier * portfolioShare;
                    Decimal amountToLong = targetPositionValue - positionValue;

                    if (positionQuantity < 0)
                    {
                        // There is an existing short position we need to dispose of first
                        Console.WriteLine($"Removing {positionValue:C2} short position.");
                        await SubmitOrder(-positionQuantity, agg.Close, OrderSide.Buy);
                    }
                    else if (amountToLong > 0)
                    {
                        // We want to expand our existing long position.
                        if (amountToLong > buyingPower)
                        {
                            amountToLong = buyingPower;
                        }
                        Int32 qty = (Int32)(amountToLong / agg.Close);

                        await SubmitOrder(qty, agg.Close, OrderSide.Buy);
                        Console.WriteLine($"Adding {qty * agg.Close:C2} to long position.");
                    }
                    else
                    {
                        // We want to shrink our existing long position.
                        amountToLong *= -1;
                        Int32 qty = (Int32)(amountToLong / agg.Close);
                        if (qty > positionQuantity)
                        {
                            qty = positionQuantity;
                        }

                        await SubmitOrder(qty, agg.Close, OrderSide.Sell);
                        Console.WriteLine($"Removing {qty * agg.Close:C2} from long position");
                    }
                }
            }
            else
            {
                Console.WriteLine("Waiting on more data.");
            }
        }

        // Update our information about the last order we placed.
        // This is done so that we know whether or not we need to submit a cancel for
        // that order before we place another.
        private void HandleTradeUpdate(ITradeUpdate trade)
        {
            if (trade.Order.OrderId == lastTradeId)
            {
                switch (trade.Event)
                {
                    case TradeEvent.Fill:
                        Console.WriteLine("Trade filled.");
                        lastTradeOpen = false;
                        break;
                    case TradeEvent.Rejected:
                        Console.WriteLine("Trade rejected.");
                        lastTradeOpen = false;
                        break;
                    case TradeEvent.Canceled:
                        Console.WriteLine("Trade canceled.");
                        lastTradeOpen = false;
                        break;
                }
            }
        }

        // Submit an order if quantity is not zero.
        private async Task SubmitOrder(Int32 quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                return;
            }
            try
            {
                var order = await alpacaTradingClient.PostOrderAsync(
                    side.Limit(symbol, quantity, price));

                lastTradeId = order.OrderId;
                lastTradeOpen = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Warning: " + e.Message);
            }
        }

        private async Task ClosePositionAtMarket()
        {
            try
            {
                var positionQuantity = (await alpacaTradingClient.GetPositionAsync(symbol)).Quantity;
                Console.WriteLine("Closing position at market price.");
                if (positionQuantity > 0)
                {
                    await alpacaTradingClient.PostOrderAsync(
                        OrderSide.Sell.Market(symbol, positionQuantity));
                }
                else
                {
                    await alpacaTradingClient.PostOrderAsync(
                        OrderSide.Buy.Market(symbol, Math.Abs(positionQuantity)));
                }
            }
            catch (Exception)
            {
                // No position to exit.
            }
        }
    }
}