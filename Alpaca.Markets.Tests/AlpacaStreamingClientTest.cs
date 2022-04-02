namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String TradeUpdates = "trade_updates";

    private const String Authorization = "authorization";

    private const String Listening = "listening";

    private const String Stock = "AAPL";

    public AlpacaStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ConnectAndSubscribeWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaStreamingClientMock();

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", AuthStatus.Authorized.ToString()))));

        client.AddResponse(getMessage(Listening, new JObject()));

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        var tradeUpdates = new AutoResetEvent(false);
        client.Client.OnTradeUpdate += HandleTradeUpdate;

        await client.AddMessageAsync(getMessage(TradeUpdates, new JObject(
            new JProperty("order", Stock.CreateMarketOrder()),
            new JProperty("event", TradeEvent.PendingNew))));

        Assert.True(tradeUpdates.WaitOne(TimeSpan.FromSeconds(1)));

        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();

        void HandleTradeUpdate(
            ITradeUpdate tradeUpdate)
        {
            Assert.NotNull(tradeUpdate);
            Assert.NotNull(tradeUpdate.Order);

            Assert.Equal(TradeEvent.PendingNew, tradeUpdate.Event);
            tradeUpdate.Order.Validate(Stock);

            tradeUpdates.Set();
        }
    }

    private static JObject getMessage(
        String stream,
        JObject data) =>
        new (
            new JProperty("stream", stream),
            new JProperty("data", data));
}
