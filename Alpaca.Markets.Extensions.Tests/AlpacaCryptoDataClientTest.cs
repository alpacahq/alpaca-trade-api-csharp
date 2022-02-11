using Newtonsoft.Json.Linq;
using Xunit;

namespace Alpaca.Markets.Extensions.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaCryptoDataClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private static readonly Interval<DateTime> _timeInterval;

    private const String Crypto = "BTCUSD";

    private const Int32 Pages = 5;

    static AlpacaCryptoDataClientTest()
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);
        _timeInterval = new Interval<DateTime>(yesterday, today);
    }

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

        Assert.Equal(1000M, adtv);
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
        new JProperty("o", 200M),
        new JProperty("l", 100M),
        new JProperty("h", 400M),
        new JProperty("c", 300M),
        new JProperty("v", 1000M));
}
