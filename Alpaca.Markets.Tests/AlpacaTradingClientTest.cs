using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using Xunit;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaTradingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    public AlpacaTradingClientTest(MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task GetClockAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        mock.Handler.When("/v2/clock").Respond("application/json",
            JsonConvert.SerializeObject(new JsonClock
            {
                TimestampUtc = DateTime.UtcNow,
                IsOpen = true
            }));

        var clock = await mock.Client.GetClockAsync();

        Assert.True(clock.IsOpen);
        Assert.True(clock.TimestampUtc <= DateTime.UtcNow);
    }
}