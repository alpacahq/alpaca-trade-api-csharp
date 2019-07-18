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
        private string API_KEY = "REPLACEME";

        private string API_SECRET = "REPLACEME";

        private string API_URL = "https://paper-api.alpaca.markets";

        private string symbol = "AAPL";

        private Decimal scale = 200;

        private RestClient restClient;
        private SockClient sockClient;
        private PolygonSockClient polygonSockClient;

        private Guid lastTradeId = Guid.NewGuid();

        private bool lastTradeOpen = false;

        private readonly List<Decimal> closingPrices = new List<Decimal>();

        public async Task Run()
        {
            restClient = new RestClient(API_KEY, API_SECRET, API_URL, apiVersion: 2);

            // Connect to Alpaca's websocket and listen for updates on our orders.
            sockClient = new SockClient(API_KEY, API_SECRET, API_URL);
            sockClient.ConnectAsync().Wait();

            sockClient.OnTradeUpdate += HandleTradeUpdate;

            // First, cancel any existing orders so they don't impact our buying power.
            var orders = await restClient.ListOrdersAsync();
            foreach (var order in orders)
            {
                await restClient.DeleteOrderAsync(order.OrderId);
            }

            // Figure out when the market will close so we can prepare to sell beforehand.
            var calendars = (await restClient.ListCalendarAsync(DateTime.Today)).ToList();
            var calendarDate = calendars.First().TradingDate;
            var closingTime = calendars.First().TradingCloseTime;

            closingTime = new DateTime(calendarDate.Year, calendarDate.Month, calendarDate.Day, closingTime.Hour, closingTime.Minute, closingTime.Second);

            var today = DateTime.Today;
            // Get the first group of bars from today if the market has already been open.
            var bars = await restClient.ListMinuteAggregatesAsync(symbol, 1, today, today.AddDays(1));
            var lastBars = bars.Items.Skip(Math.Max(0, bars.Items.Count() - 20));
            foreach (var bar in lastBars)
            {
                if (bar.Time.Date == today)
                {
                    closingPrices.Add(bar.Close);
                }
            }

            Console.WriteLine("Waiting for market open...");
            await AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Connect to Polygon's websocket and listen for price updates.
            polygonSockClient = new PolygonSockClient(API_KEY);
            polygonSockClient.ConnectAsync().Wait();
            Console.WriteLine("Polygon client opened.");
            polygonSockClient.MinuteAggReceived += async (agg) =>
            {
                // If the market's close to closing, exit position and stop trading.
                TimeSpan minutesUntilClose = closingTime - DateTime.UtcNow;
                if (minutesUntilClose.TotalMinutes < 15)
                {
                    Console.WriteLine("Reached the end of trading window.");
                    await ClosePositionAtMarket();
                    await polygonSockClient.DisconnectAsync();
                }
                else
                {
                    // Decide whether to buy or sell and submit orders.
                    await HandleMinuteAgg(agg);
                }
            };
            polygonSockClient.SubscribeSecondAgg(symbol);
        }

        public void Dispose()
        {
            restClient?.Dispose();
            sockClient?.Dispose();
            polygonSockClient?.Dispose();
        }

        // Waits until the clock says the market is open.
        // Note: if you wanted the algorithm to start trading right at market open, you would instead
        // use the method restClient.GetCalendarAsync() to get the open time and schedule execution
        // of your code based on that. However, this algorithm does not start trading until at least
        // 20 minutes after the market opens.
        private async Task AwaitMarketOpen()
        {
            while (!(await restClient.GetClockAsync()).IsOpen)
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
                    bool res = await restClient.DeleteOrderAsync(lastTradeId);
                }

                // Make sure we know how much we should spend on our position.
                var account = await restClient.GetAccountAsync();
                Decimal buyingPower = account.BuyingPower;
                Decimal equity = account.Equity;
                long multiplier = account.Multiplier;

                // Check how much we currently have in this position.
                int positionQuantity = 0;
                Decimal positionValue = 0;
                try
                {
                    var currentPosition = await restClient.GetPositionAsync(symbol);
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
                            int qty = (int)(amountToShort / agg.Close);
                            Console.WriteLine($"Adding {qty * agg.Close:C2} to short position.");
                            await SubmitOrder(qty, agg.Close, OrderSide.Sell);
                        }
                        else
                        {
                            // We want to shrink our existing short position.
                            int qty = (int)(amountToShort / agg.Close);
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
                        await SubmitOrder(positionQuantity * -1, agg.Close, OrderSide.Buy);
                    }
                    else if (amountToLong > 0)
                    {
                        // We want to expand our existing long position.
                        if (amountToLong > buyingPower)
                        {
                            amountToLong = buyingPower;
                        }
                        int qty = (int)(amountToLong / agg.Close);

                        await SubmitOrder(qty, agg.Close, OrderSide.Buy);
                        Console.WriteLine($"Adding {qty * agg.Close:C2} to long position.");
                    }
                    else
                    {
                        // We want to shrink our existing long position.
                        amountToLong *= -1;
                        int qty = (int)(amountToLong / agg.Close);
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
        private async Task SubmitOrder(int quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                return;
            }
            try
            {
                var order = await restClient.PostOrderAsync(symbol, quantity, side, OrderType.Limit, TimeInForce.Day, price);
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
                var positionQuantity = (await restClient.GetPositionAsync(symbol)).Quantity;
                Console.WriteLine("Closing position at market price.");
                if (positionQuantity > 0)
                {
                    await restClient.PostOrderAsync(symbol, positionQuantity, OrderSide.Sell, OrderType.Market, TimeInForce.Day);
                } else
                {
                    await restClient.PostOrderAsync(symbol, positionQuantity * -1, OrderSide.Buy, OrderType.Market, TimeInForce.Day);
                }
            }
            catch (Exception)
            {
                // No position to exit.
            }
        }
    }
}
