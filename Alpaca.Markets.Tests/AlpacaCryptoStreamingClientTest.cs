namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaCryptoStreamingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const String Crypto = "BTCUSD";

    [Fact]
    public async Task ConnectAndSubscribeMinuteBarsWorks()
    {
        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IBar>.Create(
                         client.Client, bar => bar.Validate(Crypto),
                         streamingClient => streamingClient.GetMinuteBarSubscription(Crypto),
                         streamingClient => streamingClient.GetMinuteBarSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingBar("b")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeDailyBarsWorks()
    {
        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock(
            configuration: new AlpacaCryptoStreamingClientConfiguration()
                .WithExchanges(CryptoExchange.Cbse));

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IBar>.Create(
                         client.Client, bar => bar.Validate(Crypto),
                         streamingClient => streamingClient.GetDailyBarSubscription(Crypto),
                         streamingClient => streamingClient.GetUpdatedBarSubscription(Crypto)))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingBar("d")));
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingBar("u")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeQuotesWorks()
    {
        var configuration = new AlpacaCryptoStreamingClientConfiguration()
            .WithExchanges(Enum.GetValues<CryptoExchange>().AsEnumerable());

        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock(configuration: configuration);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, quote => quote.Validate(Crypto),
                         streamingClient => streamingClient.GetQuoteSubscription(Crypto)))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingQuote()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();

        Assert.Equal(Enum.GetValues<CryptoExchange>(), configuration.Exchanges);
    }

    [Fact]
    public async Task ConnectAndSubscribeAllQuotesWorks()
    {
        var configuration = new AlpacaCryptoStreamingClientConfiguration()
            .WithExchanges(Enum.GetValues<CryptoExchange>().AsEnumerable());

        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock(configuration: configuration);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, quote => quote.Validate(Crypto),
                         streamingClient => streamingClient.GetQuoteSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingQuote()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();

        Assert.Equal(Enum.GetValues<CryptoExchange>(), configuration.Exchanges);
    }

    [Fact]
    public async Task ConnectAndSubscribeTradesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, trade => trade.Validate(Crypto),
                         streamingClient => streamingClient.GetTradeSubscription(Crypto)))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateStreamingTrade("t")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeOrderBookWorks()
    {
        using var client = mockClientsFactory.GetAlpacaCryptoStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IOrderBook>.Create(
                         client.Client, book => MessageDataHelpers.Validate(book, Crypto),
                         streamingClient => streamingClient.GetOrderBookSubscription(Crypto)))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateOrderBook()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }
}
