namespace Alpaca.Markets.Extensions.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaCryptoDataClientTest
{
    private static readonly Interval<DateTime> _timeInterval = getTimeInterval();

    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String Crypto = "BTCUSD";

    private const Decimal Volume = 1_000M;

    private const Decimal Price = 100M;

    private const Int32 Pages = 5;

    public AlpacaCryptoDataClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task GetAverageDailyTradeVolumeAsyncWithIntervalWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaCryptoDataClientMock();

        addPaginatedResponses(mock, addSingleBarsPageExpectation);

        var (adtv, count) = await mock.Client.GetAverageDailyTradeVolumeAsync(
            Crypto, _timeInterval.AsDateInterval());

        Assert.Equal(Volume, adtv);
        Assert.True(count != 0);
    }

    private static void addPaginatedResponses<TConfiguration, TClient>(
        MockClient<TConfiguration, TClient> mock,
        Action<MockClient<TConfiguration, TClient>, String?> singleResponseFactory)
        where TConfiguration : AlpacaClientConfigurationBase
        where TClient : class, IDisposable
    {
        for (var index = 1; index <= Pages; ++index)
        {
            singleResponseFactory(mock, index != Pages
                ? Guid.NewGuid().ToString("D") : null);
        }
    }

    private static void addSingleBarsPageExpectation(
        MockClient<AlpacaCryptoDataClientConfiguration, IAlpacaCryptoDataClient> mock,
        String? token = null) =>
        mock.AddGet("/v1beta1/crypto/**/bars", new JObject(
            new JProperty("bars", createBarsList()),
            new JProperty("next_page_token", token),
            new JProperty("symbol", Crypto)));

    private static JArray createBarsList() =>
        new (createBar(), createBar(), createBar());

    private static JObject createBar() => new (
        new JProperty("t", DateTime.UtcNow),
        new JProperty("v", Volume),
        new JProperty("o", Price),
        new JProperty("l", Price),
        new JProperty("h", Price),
        new JProperty("c", Price));

    private static Interval<DateTime> getTimeInterval()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        return new Interval<DateTime>(yesterday, today);
    }
}
