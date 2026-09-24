using System.ComponentModel;
using System.Net.Sockets;

namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaStreamingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private readonly record struct FakeEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi => Environments.Paper.AlpacaTradingApi;

        public Uri AlpacaDataApi => Environments.Paper.AlpacaTradingApi;

        public Uri AlpacaStreamingApi => new("https://www.alpaca.com");

        public Uri AlpacaDataStreamingApi => Environments.Paper.AlpacaTradingApi;

        public Uri AlpacaCryptoStreamingApi => Environments.Paper.AlpacaTradingApi;

        public Uri AlpacaNewsStreamingApi => Environments.Paper.AlpacaTradingApi;

        public Uri AlpacaOptionsStreamingApi => Environments.Paper.AlpacaTradingApi;
    }

    private const String TradeUpdates = "trade_updates";

    private const String Authorization = "authorization";

    private const String Listening = "listening";

    private const Int64 IntegerQuantity = 12L;

    private const Decimal Quantity = 12.34M;

    private const Decimal Price = 123.45M;

    private const String Stock = "AAPL";

    [Theory]
    [ClassData(typeof(EnvironmentTestData))]
    public async Task ConnectAndSubscribeWorks(IEnvironment environment)
    {
        Assert.NotNull(environment);

        using var client = mockClientsFactory.GetAlpacaStreamingClientMock(environment,
            environment.GetAlpacaStreamingClientConfiguration(new SecretKey(
                Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"))));

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", nameof(AuthStatus.Authorized)))));

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

            Assert.NotNull(tradeUpdate.ExecutionId);
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

    [Theory]
    [InlineData("simple")]
    [InlineData("")]
    public async Task OnTradeUpdateReadsSimpleOrderClass(
        String orderClass)
    {
        using var client = mockClientsFactory.GetAlpacaStreamingClientMock();

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", nameof(AuthStatus.Authorized)))));

        client.AddResponse(getMessage(Listening, new JObject()));

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        var tradeUpdates = new AutoResetEvent(false);
        client.Client.OnTradeUpdate += HandleTradeUpdate;

        await client.AddMessageAsync(getTradeUpdate(orderClass));

        Assert.True(tradeUpdates.WaitOne(TimeSpan.FromSeconds(1)));

        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();
        return;

        void HandleTradeUpdate(
            ITradeUpdate tradeUpdate)
        {
            Assert.Equal(OrderClass.Simple, tradeUpdate.Order.OrderClass);
            Assert.Empty(tradeUpdate.Order.Legs);

            tradeUpdates.Set();
        }
    }

    [Fact]
    public async Task OnTradeUpdateReadsMultiLegOrderClass()
    {
        using var client = mockClientsFactory.GetAlpacaStreamingClientMock();

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", nameof(AuthStatus.Authorized)))));

        client.AddResponse(getMessage(Listening, new JObject()));

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        var tradeUpdates = new AutoResetEvent(false);
        client.Client.OnTradeUpdate += HandleTradeUpdate;

        await client.AddMessageAsync(getMultiLegTradeUpdate());

        Assert.True(tradeUpdates.WaitOne(TimeSpan.FromSeconds(1)));

        client.Client.OnTradeUpdate -= HandleTradeUpdate;

        await client.Client.DisconnectAsync();
        return;

        void HandleTradeUpdate(
            ITradeUpdate tradeUpdate)
        {
            Assert.Equal(OrderClass.MultiLegOptions, tradeUpdate.Order.OrderClass);
            Assert.Equal(2, tradeUpdate.Order.Legs.Count);
            Assert.All(tradeUpdate.Order.Legs, leg => Assert.Equal(OrderClass.MultiLegOptions, leg.OrderClass));

            tradeUpdates.Set();
        }
    }

    [Fact]
    public async Task ErrorsAndWarningWorks()
    {
        const Int32 expectedWarnings = 3;
        const Int32 expectedErrors = 2;

        using var client = mockClientsFactory.GetAlpacaStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", nameof(AuthStatus.Unauthorized)),
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

    [Fact(Skip = "Not stable on GitHub Action environment.")]
    public async Task RecursiveErrorsWorks()
    {
        const Int32 expectedWarnings = 1;
        const Int32 expectedErrors = 1;

        using var client = mockClientsFactory.GetAlpacaStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        client.AddResponse(getMessage(Authorization, new JObject(
            new JProperty("status", nameof(AuthStatus.Unauthorized)),
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

        static void HandleTradeUpdate(
            ITradeUpdate _) =>
            throw new SocketException((Int32)SocketError.IsConnected);

        static void HandleWarning(
            String _) =>
            throw new SocketException((Int32)SocketError.IsConnected);
    }

    [Fact]
    public async Task TopLevelExceptionsWorks()
    {
        const Int32 expectedWarnings = 0;
        const Int32 expectedErrors = 1;

        using var client = mockClientsFactory.GetAlpacaStreamingClientMock(new FakeEnvironment());
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        // Errors
        client.AddException(
            socket => socket.ConnectAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()),
            new InvalidAsynchronousStateException());

        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await client.Client.DisconnectAsync(new CancellationToken(true));

        tracker.WaitAllEvents();

        await client.Client.DisconnectAsync();
        client.Client.Dispose(); // Double dispose should be safe
    }

    private static JObject getTradeUpdate(
        OrderClass orderClass = OrderClass.Simple) =>
        getMessage(TradeUpdates, new JObject(
            new JProperty("order", Stock.CreateMarketOrder(orderClass)),
            new JProperty("execution_id", Guid.NewGuid()),
            new JProperty("event", TradeEvent.PendingNew),
            new JProperty("timestamp", DateTime.UtcNow),
            new JProperty("position_qty", Quantity),
            new JProperty("qty", Quantity),
            new JProperty("price", Price)));

    private static JObject getTradeUpdate(
        String orderClass)
    {
        var tradeUpdate = getTradeUpdate();
        tradeUpdate["data"]!["order"]!["order_class"] = orderClass;
        return tradeUpdate;
    }

    private static JObject getMultiLegTradeUpdate()
    {
        var tradeUpdate = getTradeUpdate(OrderClass.MultiLegOptions);
        tradeUpdate["data"]!["order"]!["legs"] = new JArray(
            "AAPL261218C00250000".CreateMarketOrder(OrderClass.MultiLegOptions),
            "AAPL261218C00260000".CreateMarketOrder(OrderClass.MultiLegOptions));
        return tradeUpdate;
    }

    private static JObject getMessage(
        String stream,
        JObject? data) =>
        new(
            new JProperty("stream", stream),
            new JProperty("data", data));
}
