namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaDataStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const Decimal DownPrice = 100M;

    private const Decimal UpPrice = 200M;

    private const String Stock = "AAPL";

    public AlpacaDataStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ConnectAndSubscribeQuotesWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, _ => _.Validate(Stock),
                         _ => _.GetQuoteSubscription(Stock)))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingQuote()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeTradesWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, _ => _.Validate(Stock),
                         _ => _.GetCancellationSubscription(Stock),
                         _ => _.GetTradeSubscription(Stock)))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingTrade("t")));
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingTrade("x")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeStatusesWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IStatus>.Create(
                         client.Client, validate,
                         _ => _.GetStatusSubscription(Stock)))
        {
            await client.AddMessageAsync(new JArray(createStatus()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeLimitUpLimitDownsWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ILimitUpLimitDown>.Create(
                         client.Client, validate,
                         _ => _.GetLimitUpLimitDownSubscription(Stock)))
        {
            await client.AddMessageAsync(new JArray(createLimitUpLimitDown()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeCorrectionsWorks()
    {
        const String channel = "trades";

        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.AddSubscription(channel, new JArray(Stock));

        await using (await SubscriptionHelper<ITrade>.Create(
                   client.Client, _ => _.Validate(Stock),
                   _ => _.GetTradeSubscription(Stock)))
        {
            await using var corrections =
                await SubscriptionHelper<ICorrection>.Create(client.Client, validate,
                    _ => _.GetCorrectionSubscription(Stock));

            await client.AddMessageAsync(new JArray(Stock.CreateCorrection()));
            Assert.True(corrections.WaitAll());

            client.AddSubscription(channel, new JArray());
        }

        await client.Client.DisconnectAsync();
    }
    
    [Fact]
    public async Task ErrorsAndWarningsWorks()
    {
        const String channel = "corrections";
        const Int32 expectedWarnings = 0;
        const Int32 expectedErrors = 4;

        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        // Errors (2 in row)
        await client.AddAuthenticationAsync(null); 

        // No errors or warnings
        await client.AddErrorMessageAsync(406);
        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.Client.Connected += HandleConnected;
        await client.AddErrorMessageAsync(403);

        // Error (only one)
        await client.AddMessageAsync(new JObject(new JProperty("T", channel)));

        await using (var subscriptionHelper = await SubscriptionHelper<ICorrection>.Create(
                         client.Client, validate, _ => _.GetCorrectionSubscription(Stock)))
        {
            subscriptionHelper.Subscribe(HandleCorrection);

            await client.AddMessageAsync(new JArray(Stock.CreateCorrection()));
            Assert.True(subscriptionHelper.WaitAll());

            subscriptionHelper.Unsubscribe(HandleCorrection);
        }

        tracker.WaitAllEvents();

        client.Client.Connected -= HandleConnected;

        await client.Client.DisconnectAsync();

        void HandleConnected(AuthStatus status)
        {
            if (status == AuthStatus.Authorized)
            {
                throw new InvalidOperationException(); // Should be reported via OnError event
            }
        } 

        void HandleCorrection(ICorrection correction) =>
            throw new InvalidOperationException(); // Should be reported via OnError event
    }

    private static JObject createStatus() =>
        new(
            new JProperty(MessageDataHelpers.StreamingMessageTypeTag, "s"),
            new JProperty("sc", Guid.NewGuid().ToString("D")),
            new JProperty("sm", Guid.NewGuid().ToString("D")),
            new JProperty("rc", Guid.NewGuid().ToString("D")),
            new JProperty("rm", Guid.NewGuid().ToString("D")),
            new JProperty("z", Guid.NewGuid().ToString("D")),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("S", Stock));

    private static JObject createLimitUpLimitDown() =>
        new(
            new JProperty(MessageDataHelpers.StreamingMessageTypeTag, "l"),
            new JProperty("i", Guid.NewGuid().ToString("D")),
            new JProperty("z", Guid.NewGuid().ToString("D")),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("d", DownPrice),
            new JProperty("u", UpPrice),
            new JProperty("S", Stock));

    private static void validate(
        IStatus status)
    {
        Assert.NotNull(status);

        Assert.False(String.IsNullOrEmpty(status.Tape));
        Assert.False(String.IsNullOrEmpty(status.StatusCode));
        Assert.False(String.IsNullOrEmpty(status.ReasonCode));
        Assert.False(String.IsNullOrEmpty(status.StatusMessage));
        Assert.False(String.IsNullOrEmpty(status.ReasonMessage));
    }

    private static void validate(
        ILimitUpLimitDown limitUpLimitDown)
    {
        Assert.NotNull(limitUpLimitDown);

        Assert.False(String.IsNullOrEmpty(limitUpLimitDown.Tape));
        Assert.False(String.IsNullOrEmpty(limitUpLimitDown.Indicator));

        Assert.Equal(UpPrice, limitUpLimitDown.LimitUpPrice);
        Assert.Equal(DownPrice, limitUpLimitDown.LimitDownPrice);
    }

    private static void validate(
        ICorrection correction)
    {
        Assert.NotNull(correction);

        correction.CorrectedTrade.Validate(Stock);
        correction.OriginalTrade.Validate(Stock);
    }
}
