using Alpaca.Markets;
using System.Diagnostics.CodeAnalysis;

namespace UsageExamples;

// This version of the mean reversion example algorithm utilizes Alpaca data that
// is available to users who have a funded Alpaca brokerage account. By default, it
// is configured to use the paper trading API, but you can change it to use the live
// trading API by setting the API_URL.
[SuppressMessage("ReSharper", "UnusedVariable")]
[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
internal sealed class MeanReversionWithCrypto : IDisposable
{
    private const String API_KEY = "REPLACEME";

    private const String API_SECRET = "REPLACEME";

    private const String symbol = "BTCUSD";

    private const Decimal scale = 200;

    private IAlpacaCryptoDataClient alpacaCryptoDataClient;

    private IAlpacaTradingClient alpacaTradingClient;

    private IAlpacaStreamingClient alpacaStreamingClient;

    private IAlpacaCryptoStreamingClient alpacaCryptoStreamingClient;

    private Guid lastTradeId = Guid.NewGuid();

    private Boolean lastTradeOpen;

    private readonly List<Decimal> closingPrices = [];

    private Boolean isAssetShortable;

    public async Task Run()
    {
        alpacaTradingClient = Environments.Paper.GetAlpacaTradingClient(new SecretKey(API_KEY, API_SECRET));

        alpacaCryptoDataClient = Environments.Paper.GetAlpacaCryptoDataClient(new SecretKey(API_KEY, API_SECRET));

        // Connect to Alpaca's websocket and listen for updates on our orders.
        alpacaStreamingClient = Environments.Paper.GetAlpacaStreamingClient(new SecretKey(API_KEY, API_SECRET));

        await alpacaStreamingClient.ConnectAndAuthenticateAsync();

        alpacaStreamingClient.OnTradeUpdate += HandleTradeUpdate;

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

        // Get the first group of bars from today if the market has already been open.
        var today = DateTime.Today;
        var calendar = await alpacaTradingClient.ListIntervalCalendarAsync(
            CalendarRequest.GetForSingleDay(DateOnly.FromDateTime(today)));
        var tradingDay = calendar[0];

        var bars = await alpacaCryptoDataClient.ListHistoricalBarsAsync(
            new HistoricalCryptoBarsRequest(symbol, BarTimeFrame.Minute, tradingDay.Trading));
        var lastBars = bars.Items.Skip(Math.Max(0, bars.Items.Count - 20));

        foreach (var bar in lastBars)
        {
            if (bar.TimeUtc.Date == today)
            {
                closingPrices.Add(bar.Close);
            }
        }

        Console.WriteLine("Waiting for market open...");
        await AwaitMarketOpen();
        Console.WriteLine("Market opened.");

        // Connect to Alpaca's websocket and listen for price updates.
        alpacaCryptoStreamingClient = Environments.Live.GetAlpacaCryptoStreamingClient(new SecretKey(API_KEY, API_SECRET));

        await alpacaCryptoStreamingClient.ConnectAndAuthenticateAsync();
        Console.WriteLine("Alpaca streaming client opened.");

        var subscription = alpacaCryptoStreamingClient.GetMinuteBarSubscription(symbol);
        // ReSharper disable once AsyncVoidLambda
        subscription.Received += async bar =>
        {
                // If the market's close to closing, exit position and stop trading.
                var minutesUntilClose = closingTime - DateTime.UtcNow;
            if (minutesUntilClose.TotalMinutes < 15)
            {
                Console.WriteLine("Reached the end of trading window.");
                await ClosePositionAtMarket();
                await alpacaCryptoStreamingClient.DisconnectAsync();
            }
            else
            {
                    // Decide whether to buy or sell and submit orders.
                    await HandleMinuteBar(bar);
            }
        };
        await alpacaCryptoStreamingClient.SubscribeAsync(subscription);
    }

    public void Dispose()
    {
        alpacaTradingClient?.Dispose();
        alpacaCryptoDataClient?.Dispose();
        alpacaStreamingClient?.Dispose();
        alpacaCryptoStreamingClient?.Dispose();
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
    private async Task HandleMinuteBar(IBar agg)
    {
        closingPrices.Add(agg.Close);
        if (closingPrices.Count <= 20)
        {
            Console.WriteLine("Waiting on more data.");
            return;
        }

        closingPrices.RemoveAt(0);

        var avg = closingPrices.Average();
        var diff = avg - agg.Close;

        // If the last trade hasn't filled yet, we'd rather replace
        // it than have two orders open at once.
        if (lastTradeOpen)
        {
            // We need to wait for the cancel to process in order to avoid
            // having long and short orders open at the same time.
            var res = await alpacaTradingClient.CancelOrderAsync(lastTradeId);
        }

        // Make sure we know how much we should spend on our position.
        var account = await alpacaTradingClient.GetAccountAsync();
        // Use maximum 10% of total account buying power for single trade
        var buyingPower = account.BuyingPower * 0.10M ?? 0M;
        var equity = account.Equity;
        var multiplier = (Int64)account.Multiplier;

        // Check how much we currently have in this position.
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
                var portfolioShare = -1 * diff / agg.Close * scale;
                var targetPositionValue = -1 * equity * multiplier * portfolioShare;
                var amountToShort = targetPositionValue - positionValue;

                switch (amountToShort)
                {
                    case < 0:
                        {
                            // We want to expand our existing short position.
                            amountToShort *= -1;
                            if (amountToShort > buyingPower)
                            {
                                amountToShort = buyingPower;
                            }

                            var qty = (Int64)(amountToShort / agg.Close);
                            if (isAssetShortable)
                            {
                                Console.WriteLine($"Adding {qty * agg.Close:C2} to short position.");
                                await SubmitOrder(qty, agg.Close, OrderSide.Sell);
                            }
                            else
                            {
                                Console.WriteLine("Unable to place short order - asset is not shortable.");
                            }
                            break;
                        }

                    case > 0:
                        {
                            // We want to shrink our existing short position.
                            var qty = (Int64)(amountToShort / agg.Close);
                            if (qty > -1 * positionQuantity)
                            {
                                qty = -1 * positionQuantity;
                            }

                            Console.WriteLine($"Removing {qty * agg.Close:C2} from short position");
                            await SubmitOrder(qty, agg.Close, OrderSide.Buy);
                            break;
                        }
                }
            }
        }
        else
        {
            // Allocate a percent of our portfolio to long position.
            var portfolioShare = diff / agg.Close * scale;
            var targetPositionValue = equity * multiplier * portfolioShare;
            var amountToLong = targetPositionValue - positionValue;

            if (positionQuantity < 0)
            {
                // There is an existing short position we need to dispose of first
                Console.WriteLine($"Removing {positionValue:C2} short position.");
                await SubmitOrder(-positionQuantity, agg.Close, OrderSide.Buy);
            }
            else switch (amountToLong)
            {
                case > 0:
                {
                    // We want to expand our existing long position.
                    if (amountToLong > buyingPower)
                    {
                        amountToLong = buyingPower;
                    }

                    var qty = (Int32)(amountToLong / agg.Close);

                    await SubmitOrder(qty, agg.Close, OrderSide.Buy);
                    Console.WriteLine($"Adding {qty * agg.Close:C2} to long position.");
                    break;
                }

                case < 0:
                {
                    // We want to shrink our existing long position.
                    amountToLong *= -1;
                    var qty = (Int64)(amountToLong / agg.Close);
                    if (qty > positionQuantity)
                    {
                        qty = positionQuantity;
                    }

                    if (isAssetShortable)
                    {
                        await SubmitOrder(qty, agg.Close, OrderSide.Sell);
                        Console.WriteLine($"Removing {qty * agg.Close:C2} from long position");
                    }
                    else
                    {
                        Console.WriteLine("Unable to place short order - asset is not shortable.");
                    }
                    break;
                }
            }
        }
    }

    // Update our information about the last order we placed.
    // This is done so that we know whether or not we need to submit a cancel for
    // that order before we place another.
    private void HandleTradeUpdate(ITradeUpdate trade)
    {
        if (trade.Order.OrderId != lastTradeId)
        {
            return;
        }

        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
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

    // Submit an order if quantity is not zero.
    private async Task SubmitOrder(Int64 quantity, Decimal price, OrderSide side)
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
            Console.WriteLine("Warning: " + e.Message); //-V5621
        }
    }

    private async Task ClosePositionAtMarket()
    {
        try
        {
            var positionQuantity = (await alpacaTradingClient.GetPositionAsync(symbol)).IntegerQuantity;
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
        catch (Exception) //-V3163 //-V5606
        {
            // No position to exit.
        }
    }
}
