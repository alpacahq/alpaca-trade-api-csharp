namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaDataStreamingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const Decimal DownPrice = 100M;

    private const Decimal UpPrice = 200M;

    private const String Stock = "AAPL";

    [Fact]
    public async Task ConnectAndSubscribeQuotesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, quote => quote.Validate(Stock),
                         streamingClient => streamingClient.GetQuoteSubscription(Stock)))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingQuote()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeAllQuotesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, quote => quote.Validate(Stock),
                         streamingClient => streamingClient.GetQuoteSubscription()))
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
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, trade => trade.Validate(Stock),
                         streamingClient => streamingClient.GetCancellationSubscription(Stock),
                         streamingClient => streamingClient.GetTradeSubscription(Stock)))
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
    public async Task ConnectAndSubscribeAllTradesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, trade => trade.Validate(Stock),
                         streamingClient => streamingClient.GetTradeSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingTrade("t")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeStatusesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IStatus>.Create(
                         client.Client, validate,
                         streamingClient => streamingClient.GetStatusSubscription(Stock)))
        {
            await client.AddMessageAsync(new JArray(createStatus()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeLimitUpLimitDownsWorks()
    {
        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ILimitUpLimitDown>.Create(
                         client.Client, validate,
                         streamingClient => streamingClient.GetLimitUpLimitDownSubscription(Stock)))
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

        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.AddSubscription(channel, new JArray(Stock));

        await using (await SubscriptionHelper<ITrade>.Create(
                   client.Client, streamingClient => streamingClient.Validate(Stock),
                   streamingClient => streamingClient.GetTradeSubscription(Stock)))
        {
            await using var corrections =
                await SubscriptionHelper<ICorrection>.Create(client.Client, validate,
                    streamingClient => streamingClient.GetCorrectionSubscription(Stock));

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
        var expectedWarnings = (0, 0);
        var expectedErrors = (3, 4);

        using var client = mockClientsFactory.GetAlpacaDataStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        // Errors (2 in row)
        await client.AddAuthenticationAsync(null); 

        // No errors or warnings
        await client.AddErrorMessageAsync(HttpStatusCode.NotAcceptable);
        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.Client.Connected += HandleConnected;
        await client.AddErrorMessageAsync(HttpStatusCode.Forbidden);

        // Error (only one)
        await client.AddMessageAsync(new JObject(new JProperty("T", channel)));

        await using (var subscriptionHelper = await SubscriptionHelper<ICorrection>.Create(
                         client.Client, validate, streamingClient => streamingClient.GetCorrectionSubscription(Stock)))
        {
            subscriptionHelper.Subscribe(HandleCorrection);

            await client.AddMessageAsync(new JArray(Stock.CreateCorrection()));
            Assert.True(subscriptionHelper.WaitAll());

            subscriptionHelper.Unsubscribe(HandleCorrection);
        }

        tracker.WaitAllEvents();

        client.Client.Connected -= HandleConnected;

        await client.Client.DisconnectAsync();
        return;

        void HandleConnected(AuthStatus status)
        {
            if (status == AuthStatus.Authorized)
            {
                throw new InvalidOperationException(); // Should be reported via OnError event
            }
        } 

        void HandleCorrection(ICorrection _) =>
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
