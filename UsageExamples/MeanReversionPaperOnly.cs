using Alpaca.Markets;
using System.Diagnostics.CodeAnalysis;

namespace UsageExamples;

// This version of the mean reversion example algorithm uses only API features which
// are available to users with a free account that can only be used for paper trading.
[SuppressMessage("ReSharper", "InconsistentNaming")]
// ReSharper disable once UnusedType.Global
internal sealed class MeanReversionPaperOnly : IDisposable
{
    private const String API_KEY = "REPLACEME";

    private const String API_SECRET = "REPLACEME";

    private const String symbol = "SPY";

    private const Decimal scale = 200;

    private IAlpacaTradingClient alpacaTradingClient;

    private IAlpacaDataClient alpacaDataClient;

    private Guid lastTradeId = Guid.NewGuid();

    private Boolean isAssetShortable;

    // ReSharper disable once UnusedMember.Global
    public async Task Run()
    {
        alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET));

        alpacaDataClient = Environments.Paper.GetAlpacaDataClient(new SecretKey(API_KEY, API_SECRET));

        var asset = await alpacaTradingClient.GetAssetAsync(symbol);
        isAssetShortable = asset.Shortable;

        // First, cancel any existing orders so they don't impact our buying power.
        await alpacaTradingClient.CancelAllOrdersAsync();

        // Figure out when the market will close so we can prepare to sell beforehand.
        var calendars = (await alpacaTradingClient
            .ListIntervalCalendarAsync(new CalendarRequest().WithInterval(DateTime.Today.GetIntervalFromThat())))
            .ToList();
        var calendarDate = calendars.First().GetTradingDate();
        var closingTime = calendars.First().GetTradingCloseTimeUtc();

        closingTime = new DateTime(calendarDate.Year, calendarDate.Month, calendarDate.Day, closingTime.Hour, closingTime.Minute, closingTime.Second);

        Console.WriteLine("Waiting for market open...");
        await AwaitMarketOpen();
        Console.WriteLine("Market opened.");

        // Check every minute for price updates.
        var timeUntilClose = closingTime - DateTime.UtcNow;
        while (timeUntilClose.TotalMinutes > 15)
        {
            // Cancel old order if it's not already been filled.
            await alpacaTradingClient.CancelOrderAsync(lastTradeId);

            // Get information about current account value.
            var account = await alpacaTradingClient.GetAccountAsync();
            // Use maximum 10% of total account buying power for single trade
            var buyingPower = account.BuyingPower * 0.10M ?? 0M;
            var portfolioValue = account.Equity;

            // Get information about our existing position.
            var positionQuantity = 0L;
            var positionValue = 0M;
            try
            {
                var currentPosition = await alpacaTradingClient.GetPositionAsync(symbol);
                positionQuantity = currentPosition.IntegerQuantity;
                positionValue = currentPosition.MarketValue ?? 0M;
            }
            catch (Exception) //-V3163 //-V5606
            {
                // No position exists. This exception can be safely ignored.
            }

            var into = DateTime.Now;
            var from = into.Subtract(TimeSpan.FromMinutes(25));
            var barSet = await alpacaDataClient.ListHistoricalBarsAsync(
                new HistoricalBarsRequest(symbol, from, into, BarTimeFrame.Minute).WithPageSize(20));
            var bars = barSet.Items;

            var avg = bars.Average(item => item.Close);
            var currentPrice = bars.Last().Close;
            var diff = avg - currentPrice;

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
                var portfolioShare = diff / currentPrice * scale;
                var targetPositionValue = portfolioValue * portfolioShare;
                var amountToAdd = targetPositionValue - positionValue;

                switch (amountToAdd)
                {
                    case > 0:
                        {
                            // Buy as many shares as we can without going over amountToAdd.

                            // Make sure we're not trying to buy more than we can.
                            if (amountToAdd > buyingPower)
                            {
                                amountToAdd = buyingPower;
                            }
                            var qtyToBuy = (Int32)(amountToAdd / currentPrice);

                            await SubmitOrder(qtyToBuy, currentPrice, OrderSide.Buy);
                            break;
                        }

                    case < 0:
                        {
                            // Sell as many shares as we can without going under amountToAdd.

                            // Make sure we're not trying to sell more than we have.
                            amountToAdd *= -1;
                            var qtyToSell = (Int64)(amountToAdd / currentPrice);
                            if (qtyToSell > positionQuantity)
                            {
                                qtyToSell = positionQuantity;
                            }

                            if (isAssetShortable)
                            {
                                await SubmitOrder(qtyToSell, currentPrice, OrderSide.Sell);
                            }
                            else
                            {
                                Console.WriteLine("Unable to place short order - asset is not shortable.");
                            }

                            break;
                        }
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
    private async Task SubmitOrder(Int64 quantity, Decimal price, OrderSide side)
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
            var positionQuantity = (await alpacaTradingClient.GetPositionAsync(symbol)).IntegerQuantity;
            await alpacaTradingClient.PostOrderAsync(
                OrderSide.Sell.Market(symbol, positionQuantity));
        }
        catch (Exception) //-V3163 //-V5606
        {
            // No position to exit.
        }
    }
}
