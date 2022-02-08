using Xunit;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaCryptoDataClientTest
{
    private static readonly String _condition = Guid.NewGuid().ToString("D");

    private static readonly String _exchange = CryptoExchange.Ersx.ToString();

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static readonly String[] _symbols = { Crypto, Other };

    private static readonly List<CryptoExchange> _exchangesList =
        new () { CryptoExchange.Ersx, CryptoExchange.Gnss };

    private static readonly Interval<DateTime> _timeInterval;

    private static readonly DateTime _yesterday;

    private static readonly DateTime _today;

    private const String Crypto = "BTCUSD";

    private const String Other = "ETHUSD";

    static AlpacaCryptoDataClientTest()
    {
        _today = DateTime.Today;
        _yesterday = _today.AddDays(-1);
        _timeInterval = new Interval<DateTime>(_yesterday, _today);
    }

    public AlpacaCryptoDataClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public void AlpacaCryptoDataClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaCryptoDataClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaCryptoDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaCryptoDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(nullThrottleParameters));
    }
}
