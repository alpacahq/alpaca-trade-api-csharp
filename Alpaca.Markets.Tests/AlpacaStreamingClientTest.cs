namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String TradeUpdates = "trade_updates";

    private const String Authorization = "authorization";

    private const String Listening = "listening";

    private const Int64 IntegerQuantity = 12L;

    private const Decimal Quantity = 12.34M;

    private const Decimal Price = 123.45M;

    private const String Stock = "AAPL";

    public AlpacaStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Theory]
    [ClassData(typeof(EnvironmentTestData))]
    public async Task ConnectAndSubscribeWorks(IEnvironment environment)
    {
        using var client = _mockClientsFactory.GetAlpacaStreamingClientMock(environment);

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", AuthStatus.Authorized.ToString()))));

        client.AddResponse(getMessage(Listening, new JObject()));

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        var tradeUpdates = new AutoResetEvent(false);
        client.Client.OnTradeUpdate += HandleTradeUpdate;

        await client.AddMessageAsync(getMessage(TradeUpdates, new JObject(
            new JProperty("order", Stock.CreateMarketOrder()),
            new JProperty("event", TradeEvent.PendingNew),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("position_qty", Quantity),
            new JProperty("qty", Quantity),
            new JProperty("price", Price))));

        Assert.True(tradeUpdates.WaitOne(TimeSpan.FromSeconds(1)));

        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();

        void HandleTradeUpdate(
            ITradeUpdate tradeUpdate)
        {
            Assert.NotNull(tradeUpdate);
            Assert.NotNull(tradeUpdate.Order);

            Assert.NotNull(tradeUpdate.TimestampUtc);
            Assert.True(tradeUpdate.TimestampUtc < DateTime.UtcNow);

            Assert.Equal(Quantity, tradeUpdate.PositionQuantity);
            Assert.Equal(Quantity, tradeUpdate.TradeQuantity);
            Assert.Equal(Price, tradeUpdate.Price);

            Assert.Equal(IntegerQuantity, tradeUpdate.PositionIntegerQuantity);
            Assert.Equal(IntegerQuantity, tradeUpdate.TradeIntegerQuantity);

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
