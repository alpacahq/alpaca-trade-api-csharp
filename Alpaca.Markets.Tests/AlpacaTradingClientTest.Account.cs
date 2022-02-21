namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task GetAccountAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account", new JObject(
            new JProperty("status", AccountStatus.Active),
            new JProperty("created_at", DateTime.UtcNow),
            new JProperty("pattern_day_trader", false),
            new JProperty("transfers_blocked", false),
            new JProperty("trading_blocked", false),
            new JProperty("account_blocked", false),
            new JProperty("id", Guid.NewGuid()),
            new JProperty("cash", 10000M)));

        var account = await mock.Client.GetAccountAsync();

        Assert.False(String.IsNullOrEmpty(account.Currency));
    }

    [Fact]
    public async Task ListAccountActivitiesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var activityGuid = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        mock.AddGet("/v2/account/activities", new JArray(
            new JObject(
                new JProperty("id", $"{timestamp:yyyyMMddHHmmssfff}:{activityGuid:D}"),
                new JProperty("activity_type", AccountActivityType.Fill),
                new JProperty("leaves_qty", 1234.56M),
                new JProperty("cum_qty", 1234567.89M),
                new JProperty("date", timestamp),
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
    }
    
    [Fact]
    public async Task GetPortfolioHistoryAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var today = DateTime.UtcNow.Date;

        mock.AddGet("/v2/account/portfolio/history", new JObject(
            new JProperty("timestamp", new JArray(
                new DateTimeOffset(today).ToUnixTimeSeconds())),
            new JProperty("profit_loss_pct", new JArray(0.01M)),
            new JProperty("profit_loss", new JArray(10M)),
            new JProperty("timeframe", TimeFrame.Day),
            new JProperty("equity", new JArray(20M)),
            new JProperty("base_value", 1234.56M)));

        var history = await mock.Client.GetPortfolioHistoryAsync(
            new PortfolioHistoryRequest
            {
                Period = new HistoryPeriod(5, HistoryPeriodUnit.Day),
                ExtendedHours = true
            }.WithInterval(new Interval<DateTime>(today, today)));

        Assert.NotNull(history.Items);
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
