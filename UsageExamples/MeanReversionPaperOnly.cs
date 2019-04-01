using Alpaca.Markets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace UsageExamples
{
    class MeanReversionPaperOnly
    {
        private string API_KEY = "REPLACEME";
        private string API_SECRET = "REPLACEME";
        private string API_URL = "https://paper-api.alpaca.markets";

        private string symbol = "SPY";
        private Decimal scale = 200;

        private RestClient restClient;

        private Guid lastTradeId = Guid.NewGuid();
        private List<Decimal> closingPrices = new List<Decimal>();

        public void Run()
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

            Console.WriteLine("Waiting for market open...");
            AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Check every minute for price updates.
            TimeSpan timeUntilClose = closingTime - DateTime.UtcNow;
            while (timeUntilClose.TotalMinutes > 15)
            {
                // Cancel old order if it's not already been filled.
                restClient.DeleteOrderAsync(lastTradeId);

                // Get information about current account value.
                var account = restClient.GetAccountAsync().Result;
                Decimal buyingPower = account.BuyingPower;
                Decimal portfolioValue = account.PortfolioValue;

                // Get information about our existing position.
                int positionQuantity = 0;
                Decimal positionValue = 0;
                try
                {
                    var currentPosition = restClient.GetPositionAsync(symbol).Result;
                    positionQuantity = currentPosition.Quantity;
                    positionValue = currentPosition.MarketValue;
                }
                catch (Exception e)
                {
                    // No position exists. This exception can be safely ignored.
                }

                var barSet = restClient.GetBarSetAsync(new String[] { symbol }, TimeFrame.Minute, 20).Result;
                var bars = barSet[symbol];
                Decimal avg = bars.Average(item => item.Close);
                Decimal currentPrice = bars.Last().Close;
                Decimal diff = avg - currentPrice;

                if (diff <= 0)
                {
                    // Above the 20 minute average - exit any existing long position.
                    if (positionQuantity > 0)
                    {
                        Console.WriteLine("Setting position to zero.");
                        SubmitOrder(positionQuantity, currentPrice, OrderSide.Sell);
                    }
                    else
                    {
                        Console.WriteLine("No position to exit.");
                    }
                }
                else
                {
                    // Allocate a percent of our portfolio to this position.
                    Decimal portfolioShare = diff / currentPrice * scale;
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
                        int qtyToBuy = (int)(amountToAdd / currentPrice);

                        SubmitOrder(qtyToBuy, currentPrice, OrderSide.Buy);
                    }
                    else
                    {
                        // Sell as many shares as we can without going under amountToAdd.

                        // Make sure we're not trying to sell more than we have.
                        amountToAdd *= -1;
                        int qtyToSell = (int)(amountToAdd / currentPrice);
                        if (qtyToSell > positionQuantity)
                        {
                            qtyToSell = positionQuantity;
                        }

                        SubmitOrder(qtyToSell, currentPrice, OrderSide.Sell);
                    }
                }

                // Wait another minute.
                Thread.Sleep(60000);
                timeUntilClose = closingTime - DateTime.UtcNow;
            }

            Console.WriteLine("Market nearing close; closing position.");
            ClosePositionAtMarket();
        }

        private void AwaitMarketOpen()
        {
            while (!restClient.GetClockAsync().Result.IsOpen)
            {
                Thread.Sleep(60000);
            }
        }

        // Submit an order if quantity is not zero.
        private void SubmitOrder(int quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                Console.WriteLine("No order necessary.");
                return;
            }
            Console.WriteLine($"Submitting {side} order for {quantity} shares at ${price}.");
            var order = restClient.PostOrderAsync(symbol, quantity, side, OrderType.Limit, TimeInForce.Day, price).Result;
            lastTradeId = order.OrderId;
        }

        private void ClosePositionAtMarket()
        {
            try
            {
                var positionQuantity = restClient.GetPositionAsync(symbol).Result.Quantity;
                restClient.PostOrderAsync(symbol, positionQuantity, OrderSide.Sell, OrderType.Market, TimeInForce.Day);
            }
            catch (Exception e)
            {
                // No position to exit.
            }
        }
    }
}
