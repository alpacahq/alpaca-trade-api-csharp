namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaNewsStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String NewsChannelName = "news";

    private const String Stock = "AAPL";

    public AlpacaNewsStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Theory]
    [ClassData(typeof(EnvironmentTestData))]
    public async Task ConnectAndSubscribeWorks(IEnvironment environment)
    {
        using var client = _mockClientsFactory.GetAlpacaNewsStreamingClientMock(environment);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        client.AddSubscription(NewsChannelName, new JArray(Stock, "*"));

        await using (var helper = await SubscriptionHelper<INewsArticle>.Create(
                         client.Client, _ => _.Validate(Stock),
                         _ => _.GetNewsSubscription(Stock),
                         _ => _.GetNewsSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingNewsArticle()));
            Assert.True(helper.WaitAll());

            client.AddSubscription(NewsChannelName, new JArray());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ErrorsAndWarningsWorks()
    {
        const Int32 expectedWarnings = 3;
        const Int32 expectedErrors = 4;

        using var client = _mockClientsFactory.GetAlpacaNewsStreamingClientMock();
        using var tracker = new ErrorsAndWarningsTracker(
            client.Client, expectedWarnings, expectedErrors);

        // Warning
        await client.AddAuthenticationAsync("failed");
        await client.AddMessageAsync(new JArray(new JObject()));
        await client.AddMessageAsync(new JArray(new JObject(
            new JProperty(MessageDataHelpers.StreamingMessageTypeTag, "mocks"))));

        // No errors or warnings
        await client.AddErrorMessageAsync(402);
        Assert.Equal(AuthStatus.Unauthorized,
            await client.Client.ConnectAndAuthenticateAsync());
        await client.AddErrorMessageAsync(401);
        await client.AddErrorMessageAsync(403);

        // Errors
        client.AddSubscription(NewsChannelName, Guid.NewGuid());
        await client.AddMessageAsync(new JObject(
            new JProperty(MessageDataHelpers.StreamingMessageTypeTag, NewsChannelName)));
        await client.AddErrorMessageAsync(500);

        await using (var helper = await SubscriptionHelper<INewsArticle>.Create(
                         client.Client, _ => _.Validate(Stock),
                         _ => _.GetNewsSubscription(Stock)))
        {
            helper.Subscribe(HandleArticle);

            await client.AddMessageAsync(
                new JArray(Stock.CreateStreamingNewsArticle()));
            Assert.True(helper.WaitAll());

            client.AddSubscription(NewsChannelName, new JArray());

            helper.Unsubscribe(HandleArticle);
        }

        tracker.WaitAllEvents();

        await client.Client.DisconnectAsync();

        void HandleArticle(INewsArticle article) =>
            throw new InvalidOperationException(); // Should be reported via OnError event
    }
}
