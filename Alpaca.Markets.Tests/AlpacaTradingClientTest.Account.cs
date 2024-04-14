using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private const Int64 IntegerPrice = 1235L;

    private const Decimal Price = 1234.56M;

    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task GetAccountAsyncWorks()
    {
        const Decimal cash = 10_000M;
        const Decimal transfer = 0M;
        const Int32 multiplier = 4;
        const UInt64 count = 2UL;

        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account", new JObject(
            new JProperty("account_number", Guid.NewGuid().ToString("D")),
            new JProperty("options_approved_level", OptionsTradingLevel.Disabled),
            new JProperty("options_trading_level", OptionsTradingLevel.Disabled),
            new JProperty("crypto_status", AccountStatus.Active),
            new JProperty("non_maginable_buying_power", Price),
            new JProperty("daytrading_buying_power", Price),
            new JProperty("last_maintenance_margin", Price),
            new JProperty("pending_transfer_out", transfer),
            new JProperty("pending_transfer_in", transfer),
            new JProperty("trade_suspended_by_user", true),
            new JProperty("status", AccountStatus.Active),
            new JProperty("created_at", DateTime.UtcNow),
            new JProperty("options_buying_power", cash),
            new JProperty("short_market_value", Price),
            new JProperty("maintenance_margin", Price),
            new JProperty("pattern_day_trader", false),
            new JProperty("regt_buying_power", Price),
            new JProperty("long_market_value", Price),
            new JProperty("transfers_blocked", false),
            new JProperty("accrued_fees", transfer),
            new JProperty("trading_blocked", false),
            new JProperty("account_blocked", false),
            new JProperty("shorting_enabled", true),
            new JProperty("multiplier", multiplier),
            new JProperty("initial_margin", Price),
            new JProperty("daytrade_count", count),
            new JProperty("buying_power", Price),
            new JProperty("last_equity", Price),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("equity", Price),
            new JProperty("sma", Price),
            new JProperty("cash", cash)));

        var account = await mock.Client.GetAccountAsync();

        Assert.Equal(Multiplier.Quadruple, account.Multiplier);
        Assert.NotEqual(Guid.NewGuid(), account.AccountId);
        Assert.False(String.IsNullOrEmpty(account.Currency));
        Assert.Equal(count, account.DayTradeCount);

        Assert.Equal(AccountStatus.Active, account.CryptoStatus);
        Assert.Equal(AccountStatus.Active, account.Status);
        Assert.Equal(transfer, account.PendingTransferOut);
        Assert.Equal(transfer, account.PendingTransferIn);
        Assert.Equal(transfer, account.AccruedFees);
        Assert.Equal(cash, account.OptionsBuyingPower);

        Assert.True(account.LastMaintenanceMargin != 0M);
        Assert.True(account.MaintenanceMargin != 0M);
        Assert.True(account.TradableCash != 0M);
        Assert.True(account.LastEquity != 0M);
        Assert.True(account.Sma != 0M);

        Assert.NotNull(account.AccountNumber);

        Assert.NotNull(account.NonMarginableBuyingPower);
        Assert.NotNull(account.DayTradingBuyingPower);
        Assert.NotNull(account.RegulationBuyingPower);
        Assert.NotNull(account.OptionsApprovedLevel);
        Assert.NotNull(account.OptionsTradingLevel);
        Assert.NotNull(account.ShortMarketValue);
        Assert.NotNull(account.LongMarketValue);
        Assert.NotNull(account.InitialMargin);
        Assert.NotNull(account.BuyingPower);
        Assert.NotNull(account.Equity);

        Assert.True(account.TradeSuspendedByUser);
        Assert.True(account.ShortingEnabled);

        Assert.False(account.IsDayPatternTrader);
        Assert.False(account.IsTransfersBlocked);
        Assert.False(account.IsTradingBlocked);
        Assert.False(account.IsAccountBlocked);
    }

    [Fact]
    public async Task ListAccountActivitiesAsyncWorks()
    {
        const Int64 integerCumulativeQuantity = 1_234_568L;
        const Decimal cumulativeQuantity = 1_234_567.89M;

        const Int64 integerQuantity = 12L;
        const Decimal quantity = 12.34M;

        const Decimal amount = 42M;

        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var orderId = Guid.NewGuid();
        var activityGuid = Guid.NewGuid();
        var timestamp = new DateTime(2022, 02, 21, 10, 30, 0, DateTimeKind.Utc);

        mock.AddGet("/v2/account/activities", new JArray(
            new JObject(
                new JProperty("id", $"{timestamp:yyyyMMddHHmmssfff}:{activityGuid:D}"),
                new JProperty("activity_type", AccountActivityType.Fill),
                new JProperty("transaction_time", DateTime.UtcNow),
                new JProperty("cum_qty", cumulativeQuantity),
                new JProperty("per_share_amount", amount),
                new JProperty("type", TradeEvent.Fill),
                new JProperty("side", OrderSide.Sell),
                new JProperty("net_amount", amount),
                new JProperty("leaves_qty", Price),
                new JProperty("order_id", orderId),
                new JProperty("date", timestamp),
                new JProperty("Symbol", Stock),
                new JProperty("qty", quantity),
                new JProperty("price", Price))));

        var activities = await mock.Client.ListAccountActivitiesAsync(
            new AccountActivitiesRequest(AccountActivityType.Fill)
                {
                    PageToken = Guid.NewGuid().ToString("D"),
                    Direction = SortDirection.Ascending,
                    PageSize = 1000
                }
                .SetSingleDate(DateOnly.FromDateTime(timestamp)));

        var activity = activities.Single();

        Assert.Equal(orderId, activity.OrderId);
        Assert.Equal(activityGuid, activity.ActivityGuid);
        Assert.Equal(timestamp.Date, activity.ActivityDateTimeUtc.Date);
        Assert.Equal(DateOnly.FromDateTime(timestamp.Date), activity.ActivityDate!.Value);

        Assert.Equal(integerCumulativeQuantity, activity.IntegerCumulativeQuantity);
        Assert.Equal(IntegerPrice, activity.IntegerLeavesQuantity);
        Assert.Equal(integerQuantity, activity.IntegerQuantity);
        Assert.Equal(Stock, activity.Symbol);

        Assert.NotNull(activity.PerShareAmount);
        Assert.NotNull(activity.NetAmount);
        Assert.NotNull(activity.Price);
    }
    
    [Fact]
    public async Task GetPortfolioHistoryAsyncWorks()
    {
        const Decimal profitLossPercent = 0.01M;
        const Decimal profitLoss = 10M;
        const Decimal equity = 20M;

        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var today = DateTime.UtcNow.Date;

        mock.AddGet("/v2/account/portfolio/history", new JObject(
            new JProperty("timestamp", new JArray(
                new DateTimeOffset(today).ToUnixTimeSeconds())),
            new JProperty("profit_loss_pct", new JArray(profitLossPercent)),
            new JProperty("profit_loss", new JArray(profitLoss)),
            new JProperty("equity", new JArray(equity)),
            // ReSharper disable once StringLiteralTypo
            new JProperty("timeframe", TimeFrame.Day),
            new JProperty("base_value", Price)));

        var history = await mock.Client.GetPortfolioHistoryAsync(
            new PortfolioHistoryRequest
            {
                Period = new HistoryPeriod(1, HistoryPeriodUnit.Week),
                IntradayReporting = IntradayReporting.Continuous,
                IntradayProfitLoss = IntradayProfitLoss.PerDay,
                TimeFrame = TimeFrame.FifteenMinutes
            });

        Assert.Equal(Price, history.BaseValue);

        Assert.NotNull(history.Items);
        Assert.NotEmpty(history.Items);
        var item = history.Items.Single();

        Assert.Equal(profitLossPercent, item.ProfitLossPercentage);
        Assert.Equal(profitLoss, item.ProfitLoss);
        Assert.Equal(today, item.TimestampUtc);
        Assert.Equal(equity, item.Equity);

        Assert.NotNull(JsonConvert.SerializeObject(history));
    }

    [Fact]
    public async Task GetAccountConfigurationAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account/configurations", createConfiguration());

        var configuration = await mock.Client.GetAccountConfigurationAsync();

        validateConfiguration(configuration);
    }

    [Fact]
    public async Task PatchAccountConfigurationAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account/configurations", createConfiguration());
        mock.AddPatch("/v2/account/configurations",createConfiguration());

        var configuration = await mock.Client.PatchAccountConfigurationAsync(
            await mock.Client.GetAccountConfigurationAsync());

        validateConfiguration(configuration);
    }

    private static JObject createConfiguration() =>
        new(
            // ReSharper disable once StringLiteralTypo
            new JProperty("max_options_trading_level", OptionsTradingLevel.LongCallPut),
            new JProperty("dtbp_check", DayTradeMarginCallProtection.Both),
            new JProperty("trade_confirm_email", TradeConfirmEmail.All),
            new JProperty("ptp_no_exception_entry", false),
            new JProperty("suspend_trade", false),
            new JProperty("no_shorting", true));

    private static void validateConfiguration(
        IAccountConfiguration configuration)
    {
        Assert.Equal(DayTradeMarginCallProtection.Both, configuration.DayTradeMarginCallProtection);
        Assert.Equal(TradeConfirmEmail.All, configuration.TradeConfirmEmail);
        Assert.False(configuration.IsSuspendTrade);
        Assert.True(configuration.IsNoShorting);
    }
}
