using Alpaca.Markets;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UsageExamples
{
    // This version of the mean reversion example algorithm uses only API features which
    // are available to users with a free account that can only be used for paper trading.
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal sealed class MeanReversionPaperOnly : IDisposable
    {
        private string API_KEY = "REPLACEME";

        private string API_SECRET = "REPLACEME";

        private string API_URL = "https://paper-api.alpaca.markets";

        private string symbol = "SPY";

        private Decimal scale = 200;

        private RestClient restClient;

        private Guid lastTradeId = Guid.NewGuid();

        public async Task Run()
        {
            restClient = new RestClient(API_KEY, API_SECRET, API_URL);

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

            Console.WriteLine("Waiting for market open...");
            await AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Check every minute for price updates.
            TimeSpan timeUntilClose = closingTime - DateTime.UtcNow;
            while (timeUntilClose.TotalMinutes > 15)
            {
                // Cancel old order if it's not already been filled.
                await restClient.DeleteOrderAsync(lastTradeId);

                // Get information about current account value.
                var account = await restClient.GetAccountAsync();
                Decimal buyingPower = account.BuyingPower;
                Decimal portfolioValue = account.Equity;

                // Get information about our existing position.
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

                var barSet = await restClient.GetBarSetAsync(new[] { symbol }, TimeFrame.Minute, 20);
                var bars = barSet[symbol].ToList();

                Decimal avg = bars.Average(item => item.Close);
                Decimal currentPrice = bars.Last().Close;
                Decimal diff = avg - currentPrice;

                if (diff <= 0)
                {
                    // Above the 20 minute average - exit any existing long position.
                    if (positionQuantity > 0)
                    {
                        Console.WriteLine("Setting position to zero.");
                        await SubmitOrder(positionQuantity, currentPrice, OrderSide.Sell);
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

                        await SubmitOrder(qtyToBuy, currentPrice, OrderSide.Buy);
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

                        await SubmitOrder(qtyToSell, currentPrice, OrderSide.Sell);
                    }
                }

                // Wait another minute.
                Thread.Sleep(60000);
                timeUntilClose = closingTime - DateTime.UtcNow;
            }

            Console.WriteLine("Market nearing close; closing position.");
            await ClosePositionAtMarket();
        }

        public void Dispose()
        {
            restClient?.Dispose();
        }

        private async Task AwaitMarketOpen()
        {
            while (!(await restClient.GetClockAsync()).IsOpen)
            {
                await Task.Delay(60000);
            }
        }

        // Submit an order if quantity is not zero.
        private async Task SubmitOrder(int quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                Console.WriteLine("No order necessary.");
                return;
            }
            Console.WriteLine($"Submitting {side} order for {quantity} shares at ${price}.");
            var order = await restClient.PostOrderAsync(symbol, quantity, side, OrderType.Limit, TimeInForce.Day, price);
            lastTradeId = order.OrderId;
        }

        private async Task ClosePositionAtMarket()
        {
            try
            {
                var positionQuantity = (await restClient.GetPositionAsync(symbol)).Quantity;
                await restClient.PostOrderAsync(symbol, positionQuantity, OrderSide.Sell, OrderType.Market, TimeInForce.Day);
            }
            catch (Exception)
            {
                // No position to exit.
            }
        }
    }
}
