using System.Linq.Expressions;
using System.Net.Sockets;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaNewsStreamingClientTest
{
    private static readonly List<String> _symbols = new () { Stock, Other };

    private const Int32 ExpectedNumberOfEventsForAllSymbols = 4;

    private const Int32 ExpectedNumberOfEventsForOneSymbol = 2;

    private const String Stock = "AAPL";

    private const String Other = "MSFT";
    
    [Fact]
    public async Task WithReconnectWorks()
    {
        var connected = 0;
        var warnings = 0;
        var opened = 0;

        var client = createMockClient(
            _ => _.GetNewsSubscription(It.IsAny<String>()));

        using var wrapped = client.Object.WithReconnect();

        wrapped.Connected += HandleConnected;
        wrapped.SocketOpened += HandleOpened;
        wrapped.OnWarning += HandleWarning;

        var result = await wrapped.ConnectAndAuthenticateAsync();
        client.Raise(_ => _.SocketClosed += null);
        client.Raise(_ => _.OnError += null, new SocketException());

        wrapped.OnWarning -= HandleWarning;
        wrapped.SocketOpened -= HandleOpened;
        wrapped.Connected -= HandleConnected;

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
        Assert.Equal(0, connected);
        Assert.Equal(0, warnings);
        Assert.Equal(0, opened);

        void HandleConnected(AuthStatus status) => ++connected;
        void HandleWarning(String message) => ++warnings;
        void HandleOpened() => ++opened;
    }

    [Fact]
    public void GetNewsSubscriptionWorks()
    {
        var client = createMockClient(
            _ => _.GetNewsSubscription(It.IsAny<String>()));

        var subscriptionOne = client.Object.GetNewsSubscription(_symbols);
        var subscriptionTwo = client.Object.GetNewsSubscription(Stock, Other);

        _symbols.VerifySubscriptionsStreams(subscriptionOne, subscriptionTwo);
        subscriptionOne.VerifySubscriptionEventsNumber(ExpectedNumberOfEventsForAllSymbols);

        client.VerifyAll();
    }

    [Fact]
    public async Task SubscribeNewsAsyncWorks()
    {
        var client = createMockClient(
            _ => _.GetNewsSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeNewsAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeNewsAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeNewsAsync(Stock, Other);

        _symbols.VerifySubscriptionsStreams(subscriptionOne, subscriptionTwo);
        subscription.VerifySubscriptionEventsNumber(ExpectedNumberOfEventsForOneSymbol);

        await subscriptionOne.DisposeAsync();
        client.VerifyAll();
    }

    private static Mock<IAlpacaNewsStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaNewsStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class => subscriptionFactory.CreateMockClient();
}
