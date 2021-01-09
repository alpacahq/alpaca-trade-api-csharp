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
        private const String API_KEY = "REPLACEME";

        private const String API_SECRET = "REPLACEME";

        private const String symbol = "SPY";

        private const Decimal scale = 200;

        private IAlpacaTradingClient alpacaTradingClient;

        private IAlpacaDataClient alpacaDataClient;

        private Guid lastTradeId = Guid.NewGuid();

        public async Task Run()
        {
            alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET));

            alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET));

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

            Console.WriteLine("Waiting for market open...");
            await AwaitMarketOpen();
            Console.WriteLine("Market opened.");

            // Check every minute for price updates.
            TimeSpan timeUntilClose = closingTime - DateTime.UtcNow;
            while (timeUntilClose.TotalMinutes > 15)
            {
                // Cancel old order if it's not already been filled.
                await alpacaTradingClient.DeleteOrderAsync(lastTradeId);

                // Get information about current account value.
                var account = await alpacaTradingClient.GetAccountAsync();
                Decimal buyingPower = account.BuyingPower;
                Decimal portfolioValue = account.Equity;

                // Get information about our existing position.
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

                var barSet = await alpacaDataClient.GetBarSetAsync(
                    new BarSetRequest(symbol, TimeFrame.Minute) { Limit = 20 });
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
                        Int32 qtyToBuy = (Int32)(amountToAdd / currentPrice);

                        await SubmitOrder(qtyToBuy, currentPrice, OrderSide.Buy);
                    }
                    else
                    {
                        // Sell as many shares as we can without going under amountToAdd.

                        // Make sure we're not trying to sell more than we have.
                        amountToAdd *= -1;
                        Int32 qtyToSell = (Int32)(amountToAdd / currentPrice);
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
            alpacaTradingClient?.Dispose();
            alpacaDataClient?.Dispose();
        }

        private async Task AwaitMarketOpen()
        {
            while (!(await alpacaTradingClient.GetClockAsync()).IsOpen)
            {
                await Task.Delay(60000);
            }
        }

        // Submit an order if quantity is not zero.
        private async Task SubmitOrder(Int32 quantity, Decimal price, OrderSide side)
        {
            if (quantity == 0)
            {
                Console.WriteLine("No order necessary.");
                return;
            }
            Console.WriteLine($"Submitting {side} order for {quantity} shares at ${price}.");
            var order = await alpacaTradingClient.PostOrderAsync(
                side.Limit(symbol, quantity, price));
            lastTradeId = order.OrderId;
        }

        private async Task ClosePositionAtMarket()
        {
            try
            {
                var positionQuantity = (await alpacaTradingClient.GetPositionAsync(symbol)).Quantity;
                await alpacaTradingClient.PostOrderAsync(
                    OrderSide.Sell.Market(symbol, positionQuantity));
            }
            catch (Exception)
            {
                // No position to exit.
            }
        }
    }
}
