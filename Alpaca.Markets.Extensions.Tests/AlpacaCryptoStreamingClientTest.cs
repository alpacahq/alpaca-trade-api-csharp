using System.Linq.Expressions;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaCryptoStreamingClientTest
{
    private static readonly List<String> _symbols = [ Crypto, Other ];

    private const Int32 ExpectedNumberOfEventsForAllSymbols = 4;

    private const Int32 ExpectedNumberOfEventsForOneSymbol = 2;

    private const String Crypto = "BTCUSD";

    private const String Other = "ETHUSD";

    [Fact]
    public async Task WithReconnectWorks()
    {
        var client = createMockClient(
            client => client.GetTradeSubscription(It.IsAny<String>()));

        using var wrapped = client.Object.WithReconnect();
        var result = await wrapped.ConnectAndAuthenticateAsync();

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
    }

    [Fact]
    public void GetOrderBookSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetOrderBookSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetOrderBookSubscription(_symbols);
        var subscriptionTwo = client.Object.GetOrderBookSubscription(Crypto, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscriptionOne, ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeOrderBookAsyncWorks()
    {
        var client = createMockClient(
            client => client.GetOrderBookSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeOrderBookAsync(Crypto);
        await using var subscriptionOne = await client.Object.SubscribeOrderBookAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeOrderBookAsync(Crypto, Other);

        verifySubscriptions(subscriptionOne, subscriptionTwo);
        verifySubscriptionEvents(subscription, ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }
    
    private static void verifySubscriptions<TItem>(
        IAlpacaDataSubscription<TItem> lhs,
        IAlpacaDataSubscription<TItem> rhs) =>
        _symbols.VerifySubscriptionsStreams(lhs, rhs);

    private static void verifySubscriptionEvents<TItem>(
        IAlpacaDataSubscription<TItem> subscription,
        Int32 expectedNumberOfEvents) =>
        subscription.VerifySubscriptionEventsNumber(expectedNumberOfEvents);

    private static Mock<IAlpacaCryptoStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaCryptoStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class =>
        subscriptionFactory.CreateMockClient();
}
