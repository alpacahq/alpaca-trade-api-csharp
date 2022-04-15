﻿namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaCryptoDataClientTest
{
    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static readonly String[] _symbols = { Crypto, Other };

    private static readonly List<CryptoExchange> _exchangesList =
        new () { CryptoExchange.Ersx, CryptoExchange.Ftx };

    private const String PathPrefix = "/v1beta1/crypto";

    private static DateTime Yesterday => _timeInterval.From!.Value;

    private static DateTime Today => _timeInterval.Into!.Value;

    private const String Crypto = "BTCUSD";

    private const String Other = "ETHUSD";

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
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaCryptoDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaCryptoDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(
            () => _mockClientsFactory.GetAlpacaCryptoDataClientMock(Environments.Paper, nullThrottleParameters));
    }

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
