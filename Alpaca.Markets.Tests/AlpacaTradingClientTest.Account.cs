using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public async Task GetAccountAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account", new JObject(
            new JProperty("account_number", Guid.NewGuid().ToString("D")),
            new JProperty("daytrading_buying_power", 1234.56M),
            new JProperty("last_maintenance_margin", 1234.56M),
            new JProperty("trade_suspended_by_user", true),
            new JProperty("short_market_value", 1234.56M),
            new JProperty("status", AccountStatus.Active),
            new JProperty("maintenance_margin", 1234.56M),
            new JProperty("created_at", DateTime.UtcNow),
            new JProperty("regt_buying_power", 1234.56M),
            new JProperty("long_market_value", 1234.56M),
            new JProperty("pattern_day_trader", false),
            new JProperty("transfers_blocked", false),
            new JProperty("initial_margin", 1234.56M),
            new JProperty("trading_blocked", false),
            new JProperty("account_blocked", false),
            new JProperty("buying_power", 1234.56M),
            new JProperty("last_equity", 1234.56M),
            new JProperty("shorting_enabled", true),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("daytrade_count", 2),
            new JProperty("equity", 1234.56M),
            new JProperty("sma", 1234.56M),
            new JProperty("multiplier", 4),
            new JProperty("cash", 10000M)));

        var account = await mock.Client.GetAccountAsync();

        Assert.Equal(Multiplier.Quadruple, account.Multiplier);
        Assert.NotEqual(Guid.NewGuid(), account.AccountId);
        Assert.False(String.IsNullOrEmpty(account.Currency));
        Assert.Equal(2UL, account.DayTradeCount);

        Assert.True(account.LastMaintenanceMargin != 0M);
        Assert.True(account.MaintenanceMargin != 0M);
        Assert.True(account.TradableCash != 0M);
        Assert.True(account.LastEquity != 0M);
        Assert.True(account.Sma != 0M);

        Assert.NotNull(account.AccountNumber);

        Assert.NotNull(account.DayTradingBuyingPower);
        Assert.NotNull(account.RegulationBuyingPower);
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
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var activityGuid = Guid.NewGuid();
        var timestamp = new DateTime(2022, 02, 21, 10, 30, 0, DateTimeKind.Utc);

        mock.AddGet("/v2/account/activities", new JArray(
            new JObject(
                new JProperty("id", $"{timestamp:yyyyMMddHHmmssfff}:{activityGuid:D}"),
                new JProperty("activity_type", AccountActivityType.Fill),
                new JProperty("transaction_time", DateTime.UtcNow),
                new JProperty("per_share_amount", 42M),
                new JProperty("type", TradeEvent.Fill),
                new JProperty("side", OrderSide.Sell),
                new JProperty("leaves_qty", 1234.56M),
                new JProperty("cum_qty", 1234567.89M),
                new JProperty("price", 12345.67M),
                new JProperty("net_amount", 42M),
                new JProperty("date", timestamp),
                new JProperty("Symbol", Stock),
                new JProperty("qty", 12.34M))));

        var activities = await mock.Client.ListAccountActivitiesAsync(
            new AccountActivitiesRequest(AccountActivityType.Fill)
                .SetSingleDate(DateOnly.FromDateTime(timestamp)));

        var activity = activities.Single();

        Assert.Equal(activityGuid, activity.ActivityGuid);
        Assert.Equal(timestamp.Date, activity.ActivityDateTimeUtc.Date);
        Assert.Equal(DateOnly.FromDateTime(timestamp.Date), activity.ActivityDate!.Value);

        Assert.Equal(1234568L, activity.IntegerCumulativeQuantity);
        Assert.Equal(1235L, activity.IntegerLeavesQuantity);
        Assert.Equal(12L, activity.IntegerQuantity);
        Assert.Equal(Stock, activity.Symbol);

        Assert.NotNull(activity.PerShareAmount);
        Assert.NotNull(activity.NetAmount);
        Assert.NotNull(activity.Price);
    }
    
    [Fact]
    public async Task GetPortfolioHistoryAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var today = DateTime.UtcNow.Date;
        var todayDateOnly = DateOnly.FromDateTime(today);

        mock.AddGet("/v2/account/portfolio/history", new JObject(
            new JProperty("timestamp", new JArray(
                new DateTimeOffset(today).ToUnixTimeSeconds())),
            new JProperty("profit_loss_pct", new JArray(0.01M)),
            new JProperty("profit_loss", new JArray(10M)),
            // ReSharper disable once StringLiteralTypo
            new JProperty("timeframe", TimeFrame.Day),
            new JProperty("equity", new JArray(20M)),
            new JProperty("base_value", 1234.56M)));

        var history = await mock.Client.GetPortfolioHistoryAsync(
            new PortfolioHistoryRequest
            {
                Period = new HistoryPeriod(5, HistoryPeriodUnit.Day),
                TimeFrame = TimeFrame.FifteenMinutes,
                ExtendedHours = true
            }.WithInterval(new Interval<DateOnly>(todayDateOnly, todayDateOnly)));

        Assert.Equal(1234.56M, history.BaseValue);

        Assert.NotNull(history.Items);
        Assert.NotEmpty(history.Items);
        var item = history.Items.Single();

        Assert.Equal(0.01M, item.ProfitLossPercentage);
        Assert.Equal(today, item.TimestampUtc);
        Assert.Equal(10M, item.ProfitLoss);
        Assert.Equal(20M, item.Equity);
    }

    [Fact]
    public async Task GetAccountConfigurationAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account/configurations", createConfiguration());

        var configuration = await mock.Client.GetAccountConfigurationAsync();

        validateConfiguration(configuration);
    }

    [Fact]
    public async Task PatchAccountConfigurationAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account/configurations", createConfiguration());
        mock.AddPatch("/v2/account/configurations",createConfiguration());

        var configuration = await mock.Client.PatchAccountConfigurationAsync(
            await mock.Client.GetAccountConfigurationAsync());

        validateConfiguration(configuration);
    }

    private static JObject createConfiguration() =>
        new (
            // ReSharper disable once StringLiteralTypo
            new JProperty("dtbp_check", DayTradeMarginCallProtection.Both),
            new JProperty("trade_confirm_email", TradeConfirmEmail.All),
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
