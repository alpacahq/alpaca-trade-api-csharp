﻿namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public sealed class AlpacaCryptoStreamingClientTest
{
    private readonly MockClientsFactoryFixture _mockClientsFactory;

    private const String Crypto = "BTCUSD";

    public AlpacaCryptoStreamingClientTest(
        MockClientsFactoryFixture mockClientsFactory) =>
        _mockClientsFactory = mockClientsFactory;

    [Fact]
    public async Task ConnectAndSubscribeMinuteBarsWorks()
    {
        using var client = _mockClientsFactory.GetAlpacaCryptoStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IBar>.Create(
                         client.Client, _ => _.Validate(Crypto),
                         _ => _.GetMinuteBarSubscription(Crypto),
                         _ => _.GetMinuteBarSubscription()))
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
        using var client = _mockClientsFactory.GetAlpacaCryptoStreamingClientMock(
            configuration: new AlpacaCryptoStreamingClientConfiguration()
                .WithExchanges(CryptoExchange.Cbse));

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IBar>.Create(
                         client.Client, _ => _.Validate(Crypto),
                         _ => _.GetDailyBarSubscription(Crypto),
                         _ => _.GetUpdatedBarSubscription(Crypto)))
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

        using var client = _mockClientsFactory.GetAlpacaCryptoStreamingClientMock(configuration: configuration);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, _ => _.Validate(Crypto),
                         _ => _.GetQuoteSubscription(Crypto)))
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
        using var client = _mockClientsFactory.GetAlpacaCryptoStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, _ => _.Validate(Crypto),
                         _ => _.GetTradeSubscription(Crypto)))
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
        using var client = _mockClientsFactory.GetAlpacaCryptoStreamingClientMock();

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IOrderBook>.Create(
                         client.Client, _ => _.Validate(Crypto),
                         _ => _.GetOrderBookSubscription(Crypto)))
        {
            await client.AddMessageAsync(
                new JArray(Crypto.CreateOrderBook()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }
}
