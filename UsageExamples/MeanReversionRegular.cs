using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UsageExamples
{
    internal sealed class MeanReversionRegular
    {
        private string API_KEY = "REPLACEME";
        private string API_SECRET = "REPLACEME";
        private string API_URL = "https://paper-api.alpaca.markets";

        private string symbol = "SPY";
        private Decimal scale = 200;

        private RestClient restClient;

        private Guid lastTradeId = Guid.NewGuid();
        private bool lastTradeOpen = false;
        private List<Decimal> closingPrices = new List<Decimal>();

        public async void Run()
        {
            restClient = new RestClient(API_KEY, API_SECRET, API_URL);

            // First, cancel any existing orders so they don't impact our buying power.
            var orders = restClient.ListOrdersAsync().Result;
            foreach (var order in orders)
            {
                restClient.DeleteOrderAsync(order.OrderId);
            }

            // Figure out when the market will close so we can prepare to sell beforehand.
            var calendars = restClient.ListCalendarAsync(DateTime.Today).Result;
            var calendarDate = calendars.First().TradingDate;
            var closingTime = calendars.First().TradingCloseTime;
            closingTime = new DateTime(calendarDate.Year, calendarDate.Month, calendarDate.Day, closingTime.Hour, closingTime.Minute, closingTime.Second);

            var today = DateTime.Today;
            // Get the first group of bars from today if the market has already been open.
            var bars = restClient.ListMinuteAggregatesAsync(symbol, 1, today.AddDays(-5), today).Result.Items;
            foreach (var bar in bars)
            {
                if (bar.Time.Date == today)
                {
                    closingPrices.Add(bar.Close);
                }
            }

            Console.WriteLine("Waiting for market open...");
            AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Connect to Polygon and listen for price updates.
            var natsClient = new NatsClient(API_KEY, false);
            natsClient.Open();
            Console.WriteLine("Polygon client opened.");
            natsClient.AggReceived += (agg) =>
            {
                // If the market's close to closing, exit position and stop trading.
                TimeSpan minutesUntilClose = closingTime - DateTime.UtcNow;
                if (minutesUntilClose.TotalMinutes < 15)
                {
                    Console.WriteLine("Reached the end of trading window.");
                    ClosePositionAtMarket();
                    natsClient.Close();
                }
                else
                {
                    // Decide whether to buy or sell and submit orders.
                    HandleMinuteAgg(agg);
                }
            };
            natsClient.SubscribeMinuteAgg(symbol);

            // Connect to Alpaca and listen for updates on our orders.
            var sockClient = new SockClient(API_KEY, API_SECRET, API_URL);
            await sockClient.ConnectAsync();
            Console.WriteLine("Socket client opened.");
            sockClient.OnTradeUpdate += (trade) =>
            {
                HandleTradeUpdate(trade);
            };
        }

        // Waits until the clock says the market is open.
        // Note: if you wanted the algorithm to start trading right at market open, you would instead
        // use the method restClient.GetCalendarAsync() to get the open time and schedule execution
        // of your code based on that. However, this algorithm does not start trading until at least
        // 20 minutes after the market opens.
        private void AwaitMarketOpen()
        {
            while (!restClient.GetClockAsync().Result.IsOpen)
            {
                Thread.Sleep(60000);
            }
        }

        // Determine whether our position should grow or shrink and submit orders.
        private void HandleMinuteAgg(IStreamAgg agg)
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
                    restClient.DeleteOrderAsync(lastTradeId);
                }

                // Make sure we know how much we should spend on our position.
                var account = restClient.GetAccountAsync().Result;
                Decimal buyingPower = account.BuyingPower;
                Decimal portfolioValue = account.Equity;

                // Check how much we currently have in this position.
                int positionQuantity = 0;
                Decimal positionValue = 0;
                try
                {
                    var currentPosition = restClient.GetPositionAsync(symbol).Result;
                    positionQuantity = currentPosition.Quantity;
                    positionValue = currentPosition.MarketValue;
                }
                catch (Exception)
                {
                    // No position exists. This exception can be safely ignored.
                }

                if (diff <= 0)
                {
                    // Above the 20 minute average - exit any existing long position.
                    if (positionQuantity > 0)
                    {
                        Console.WriteLine("Setting position to zero.");
                        SubmitOrder(positionQuantity, agg.Close, OrderSide.Sell);
                    }
                    else
                    {
                        Console.WriteLine("No position to exit.");
                    }
                }
                else
                {
                    // Allocate a percent of our portfolio to this position.
                    Decimal portfolioShare = diff / agg.Close * scale;
                    Decimal targetPositionValue = portfolioValue * portfolioShare;
                    Decimal amountToAdd = targetPositionValue - positionValue;

                    if (amountToAdd > 0)
                    {
                        // Buy as many shares as we can without going over amountToAdd.

                        // Make sure we're not trying to buy more than we can.
                        if (amountToAdd > buyingPower)
                        {
                            amountToAdd = buyingPower;
                        }
                        int qtyToBuy = (int)(amountToAdd / agg.Close);

                        SubmitOrder(qtyToBuy, agg.Close, OrderSide.Buy);
                        Console.WriteLine(String.Format("Adding {0:C2} to position.", qtyToBuy * agg.Close));
                    }
                    else
                    {
                        // Sell as many shares as we can without going under amountToAdd.

                        // Make sure we're not trying to sell more than we have.
                        amountToAdd *= -1;
                        int qtyToSell = (int)(amountToAdd / agg.Close);
                        if (qtyToSell > positionQuantity)
                        {
                            qtyToSell = positionQuantity;
                        }

                        SubmitOrder(qtyToSell, agg.Close, OrderSide.Sell);
                        Console.WriteLine(String.Format("Removing {0:C2} from position", qtyToSell * agg.Close));
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
                    case TradeEvent.Rejected:
                    case TradeEvent.Canceled:
                        lastTradeOpen = false;
                        break;
                }
            }
        }

        // Submit an order if quantity is not zero.
        private void SubmitOrder(int quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                return;
            }
            var order = restClient.PostOrderAsync(symbol, quantity, side, OrderType.Limit, TimeInForce.Day, price).Result;
            lastTradeId = order.OrderId;
            lastTradeOpen = true;
        }

        private void ClosePositionAtMarket()
        {
            try
            {
                var positionQuantity = restClient.GetPositionAsync(symbol).Result.Quantity;
                restClient.PostOrderAsync(symbol, positionQuantity, OrderSide.Sell, OrderType.Market, TimeInForce.Day);
            }
            catch (Exception)
            {
                // No position to exit.
            }
        }
    }
}
