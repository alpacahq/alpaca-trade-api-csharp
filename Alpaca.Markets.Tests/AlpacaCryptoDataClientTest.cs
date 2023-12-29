namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaCryptoDataClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private static readonly String[] _symbols = [ Crypto, Other ];

    private static readonly List<CryptoExchange> _exchangesList =
        [ CryptoExchange.Ersx, CryptoExchange.Ftx ];

    private const String PathPrefix = "/v1beta3/crypto/us";

    private static DateTime Yesterday => _timeInterval.From!.Value;

    private static DateTime Today => _timeInterval.Into!.Value;

    private static readonly String[] _symbol = [ Crypto ];

    private const String Crypto = "BTC/USD";

    private const String Other = "ETH/USD";

    [Fact]
    public void AlpacaCryptoDataClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaCryptoDataClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaCryptoDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaCryptoDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullThrottleParameters));
    }

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
