using System.Globalization;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed partial class AlpacaOptionsDataClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private static readonly String[] _symbols = [ Symbol, "AAPL240315C00225000" ];

    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private static DateTime Yesterday => _timeInterval.From!.Value;

    private static DateTime Today => _timeInterval.Into!.Value;

    private const String PathPrefix = "/v1beta1/options";

    private const String Symbol = "AAPL241220C00300000";

    [Fact]
    public void AlpacaDataClientConfigurationValidationWorks()
    {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullSecurityId = new AlpacaOptionsDataClientConfiguration { SecurityId = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaOptionsDataClientMock(Environments.Paper, nullSecurityId));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullApiEndpoint = new AlpacaOptionsDataClientConfiguration { ApiEndpoint = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaOptionsDataClientMock(Environments.Paper, nullApiEndpoint));

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        var nullThrottleParameters = new AlpacaOptionsDataClientConfiguration { ThrottleParameters = null };
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        Assert.Throws<InvalidOperationException>(() =>
            mockClientsFactory.GetAlpacaOptionsDataClientMock(Environments.Paper, nullThrottleParameters));
    }

    [Fact]
    public async Task ListExchangesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddGet($"{PathPrefix}/meta/exchanges", createDictionary());

        verifyDictionary(await mock.Client.ListExchangesAsync());
    }

    [Fact]
    public async Task ListLatestQuotesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddLatestQuotesExpectation(PathPrefix, _symbols);

        var quotes = await mock.Client.ListLatestQuotesAsync(
            new LatestOptionsDataRequest(_symbols));

        Assert.NotNull(quotes);
        Assert.NotEmpty(quotes);

        foreach (var symbol in _symbols)
        {
            Assert.True(quotes[symbol].Validate(symbol));
        }

        Assert.NotNull(mock.Client.GetRateLimitValues());
    }

    [Fact]
    public async Task ListLatestTradesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddLatestTradesExpectation(PathPrefix, _symbols);

        var trades = await mock.Client.ListLatestTradesAsync(
            new LatestOptionsDataRequest(_symbols));

        Assert.NotNull(trades);
        Assert.NotEmpty(trades);

        foreach (var symbol in _symbols)
        {
            Assert.True(trades[symbol].Validate(symbol));
        }
    }

    private static JObject createDictionary() =>
        new(Enumerable.Range(1, 10)
            .Select(index => new JProperty(
                index.ToString("D", CultureInfo.InvariantCulture),
                Guid.NewGuid().ToString("D"))));

    private static void verifyDictionary(
        IReadOnlyDictionary<String, String> dictionary)
    {
        Assert.NotEmpty(dictionary);
        foreach (var (code, name) in dictionary)
        {
            Assert.False(String.IsNullOrEmpty(code));
            Assert.False(String.IsNullOrWhiteSpace(name));
        }
    }

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
