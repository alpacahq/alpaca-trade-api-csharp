using System.Net.Sockets;

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
        using var client = _mockClientsFactory.GetAlpacaStreamingClientMock(environment,
            environment.GetAlpacaStreamingClientConfiguration(new SecretKey(
                Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"))));

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", AuthStatus.Authorized.ToString()))));

        client.AddResponse(getMessage(Listening, new JObject()));

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        var tradeUpdates = new AutoResetEvent(false);
        client.Client.OnTradeUpdate += HandleTradeUpdate;

        await client.AddMessageAsync(getTradeUpdate());

        Assert.True(tradeUpdates.WaitOne(TimeSpan.FromSeconds(1)));

        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();
        return;

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

    [Fact]
    public async Task ErrorsAndWarningWorks()
    {
        const Int32 expectedWarnings = 3;
        const Int32 expectedErrors = 2;

        using var client = _mockClientsFactory.GetAlpacaStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", AuthStatus.Unauthorized.ToString()),
            new JProperty("message", Guid.NewGuid().ToString("N")))));

        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());

        // Warnings
        await client.AddMessageAsync(getMessage(Authorization, null));
        await client.AddMessageAsync(getMessage(String.Empty, null));
        await client.AddMessageAsync(new JObject());

        // Errors
        await client.AddMessageAsync(getMessage(Authorization,
            new JObject(new JProperty("success", "authenticated"))));
        await client.AddMessageAsync("<html><body>451</body></html>");

        tracker.WaitAllEvents();

        await client.Client.DisconnectAsync();
        client.Client.Dispose(); // Double dispose should be safe
    }

    [Fact]
    public async Task RecursiveErrorsWorks()
    {
        const Int32 expectedWarnings = 1;
        const Int32 expectedErrors = 1;

        using var client = _mockClientsFactory.GetAlpacaStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", AuthStatus.Unauthorized.ToString()),
            new JProperty("message", Guid.NewGuid().ToString("N")))));

        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.Client.OnTradeUpdate += HandleTradeUpdate;
        client.Client.OnWarning += HandleWarning;

        await client.AddMessageAsync(
            getMessage(TradeUpdates, new JObject()));
        await client.AddMessageAsync(getTradeUpdate());
        await client.AddMessageAsync(
            getMessage("mock", new JObject()));

        tracker.WaitAllEvents();

        client.Client.OnWarning -= HandleWarning;
        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();
        return;

        void HandleTradeUpdate(
            ITradeUpdate _) =>
            throw new SocketException((Int32)SocketError.IsConnected);

        void HandleWarning(
            String _) =>
            throw new SocketException((Int32)SocketError.IsConnected);
    }

    private static JObject getTradeUpdate() =>
        getMessage(TradeUpdates, new JObject(
            new JProperty("order", Stock.CreateMarketOrder()),
            new JProperty("event", TradeEvent.PendingNew),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("position_qty", Quantity),
            new JProperty("qty", Quantity),
            new JProperty("price", Price)));

    private static JObject getMessage(
        String stream,
        JObject? data) =>
        new (
            new JProperty("stream", stream),
            new JProperty("data", data));
}
