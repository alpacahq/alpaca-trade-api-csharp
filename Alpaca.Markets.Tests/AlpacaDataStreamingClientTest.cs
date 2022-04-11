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

        await client.AddAuthentication();

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

        await client.AddAuthentication();

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

        await client.AddAuthentication();

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

        await client.AddAuthentication();

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
        using var client = _mockClientsFactory.GetAlpacaDataStreamingClientMock();

        await client.AddAuthentication();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ICorrection>.Create(
                         client.Client, validate,
                         _ => _.GetCorrectionSubscription(Stock)))
        {
            await client.AddMessageAsync(new JArray(Stock.CreateCorrection()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    private static JObject createStatus() =>
        new(
            new JProperty("sc", Guid.NewGuid().ToString("D")),
            new JProperty("sm", Guid.NewGuid().ToString("D")),
            new JProperty("rc", Guid.NewGuid().ToString("D")),
            new JProperty("rm", Guid.NewGuid().ToString("D")),
            new JProperty("z", Guid.NewGuid().ToString("D")),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("S", Stock),
            new JProperty("T", "s"));

    private static JObject createLimitUpLimitDown() =>
        new(
            new JProperty("i", Guid.NewGuid().ToString("D")),
            new JProperty("z", Guid.NewGuid().ToString("D")),
            new JProperty("t", DateTime.UtcNow),
            new JProperty("d", DownPrice),
            new JProperty("u", UpPrice),
            new JProperty("S", Stock),
            new JProperty("T", "l"));

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
