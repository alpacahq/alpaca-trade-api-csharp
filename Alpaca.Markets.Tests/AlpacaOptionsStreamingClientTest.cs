namespace Alpaca.Markets.Tests;

[Collection("MockEnvironment")]
public abstract class AlpacaOptionsStreamingClientTest(
    MockClientsFactoryFixture mockClientsFactory)
{
    private const String OptionSymbol = "AAPL250117C00150000";

    [Fact]
    public async Task ConnectAndSubscribeTradesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaOptionsStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<ITrade>.Create(
                         client.Client, trade => trade.Validate(OptionSymbol),
                         streamingClient => streamingClient.GetTradeSubscription(OptionSymbol),
                         streamingClient => streamingClient.GetTradeSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(OptionSymbol.CreateStreamingTrade("t")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeQuotesWorks()
    {
        using var client = mockClientsFactory.GetAlpacaOptionsStreamingClientMock(
            configuration: new AlpacaOptionsStreamingClientConfiguration(OptionsFeed.Opra));

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IQuote>.Create(
                         client.Client, quote => quote.Validate(OptionSymbol),
                         streamingClient => streamingClient.GetQuoteSubscription(OptionSymbol),
                         streamingClient => streamingClient.GetQuoteSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(OptionSymbol.CreateStreamingQuote()));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }

    [Fact]
    public async Task ConnectAndSubscribeMinuteBarsWorks()
    {
        using var client = mockClientsFactory.GetAlpacaOptionsStreamingClientMock(Environments.Paper);

        await client.AddAuthenticationAsync();

        Assert.Equal(AuthStatus.Authorized,
            await client.Client.ConnectAndAuthenticateAsync());

        await using (var helper = await SubscriptionHelper<IBar>.Create(
                         client.Client, bar => bar.Validate(OptionSymbol),
                         streamingClient => streamingClient.GetMinuteBarSubscription(OptionSymbol),
                         streamingClient => streamingClient.GetMinuteBarSubscription()))
        {
            await client.AddMessageAsync(
                new JArray(OptionSymbol.CreateStreamingBar("b")));
            Assert.True(helper.WaitAll());
        }

        await client.Client.DisconnectAsync();
    }
}