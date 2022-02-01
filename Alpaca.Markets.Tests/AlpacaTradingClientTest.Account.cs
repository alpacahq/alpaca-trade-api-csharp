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
