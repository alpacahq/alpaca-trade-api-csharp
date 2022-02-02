using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task GetAccountAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.AddGet("/v2/account", new JsonAccount());

        var account = await mock.Client.GetAccountAsync();

        Assert.False(String.IsNullOrEmpty(account.Currency));
    }

    [Fact]
    public async Task ListAccountActivitiesAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var activityGuid = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        mock.AddGet("/v2/account/activities", new JsonAccountActivity[]
        {
            new ()
            {
                ActivityDateTime = timestamp,
                ActivityId = $"{timestamp:yyyyMMddHHmmssfff}:{activityGuid:D}",
                CumulativeQuantity = 1234567.89M,
                LeavesQuantity = 1234.56M,
                Quantity = 12.34M
            }
        });

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

        mock.AddPatch("/v2/account/configurations",createConfiguration());

        var configuration = await mock.Client.PatchAccountConfigurationAsync(
            new JsonAccountConfiguration());

        validateConfiguration(configuration);
    }

    private static JsonAccountConfiguration createConfiguration() =>
        new ()
        {
            DayTradeMarginCallProtection = DayTradeMarginCallProtection.Both,
            TradeConfirmEmail = TradeConfirmEmail.All,
            IsNoShorting = true
        };

    private static void validateConfiguration(
        IAccountConfiguration configuration)
    {
        Assert.Equal(DayTradeMarginCallProtection.Both, configuration.DayTradeMarginCallProtection);
        Assert.Equal(TradeConfirmEmail.All, configuration.TradeConfirmEmail);
        Assert.True(configuration.IsNoShorting);
    }
}
