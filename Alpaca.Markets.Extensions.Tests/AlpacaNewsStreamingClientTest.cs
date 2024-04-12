using System.Linq.Expressions;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace Alpaca.Markets.Extensions.Tests;

public sealed class AlpacaNewsStreamingClientTest
{
    private static readonly List<String> _symbols = [ Stock, Other ];

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
            client => client.GetNewsSubscription(It.IsAny<String>()));

        var parameters = new ReconnectionParameters
        {
            MaxReconnectionDelay = TimeSpan.FromMilliseconds(100),
            MinReconnectionDelay = TimeSpan.FromMilliseconds(10),
            MaxReconnectionAttempts = 2
        };
        using var wrapped = client.Object.WithReconnect(parameters);

        wrapped.Connected += HandleConnected;
        wrapped.SocketOpened += HandleOpened;
        wrapped.OnWarning += HandleWarning;

        var result = await wrapped.ConnectAndAuthenticateAsync();

        client.Setup(streamingClient => streamingClient.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthStatus.Unauthorized);

        // ReSharper disable MethodHasAsyncOverload
        client.Raise(streamingClient => streamingClient.SocketClosed += null);
        client.Raise(streamingClient => streamingClient.SocketClosed += null);
        await Task.Delay(ReconnectionParameters.Default.MaxReconnectionDelay);
        client.Raise(streamingClient => streamingClient.SocketClosed += null);

        client.Raise(streamingClient => streamingClient.OnError += null, new SocketException());
        client.Raise(streamingClient => streamingClient.OnError += null, new WebSocketException());
        client.Raise(streamingClient => streamingClient.OnError += null, new TaskCanceledException());
        client.Raise(streamingClient => streamingClient.OnError += null, new RestClientErrorException());
        client.Raise(streamingClient => streamingClient.OnError += null, new InvalidOperationException());
        // ReSharper restore MethodHasAsyncOverload

        wrapped.OnWarning -= HandleWarning;
        wrapped.SocketOpened -= HandleOpened;
        wrapped.Connected -= HandleConnected;

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
        Assert.Equal(0, connected);
        Assert.Equal(0, warnings);
        Assert.Equal(0, opened);
        return;

        void HandleConnected(AuthStatus status) => ++connected;
        void HandleWarning(String message) => ++warnings;
        void HandleOpened() => ++opened;
    }
    
    [Fact]
    public async Task ErrorsHandlingWithReconnectWorks()
    {
        const Int32 expectedErrorsCount = 1;

        var errors = 0;

        var client = createMockClient(
            client => client.GetNewsSubscription(It.IsAny<String>()));

        using var wrapped = client.Object.WithReconnect();

        wrapped.OnError += HandleWarning;

        // ReSharper disable once MethodHasAsyncOverload
        client.Raise(streamingClient => streamingClient.OnError += null, new RestClientErrorException());

        wrapped.OnError -= HandleWarning;

        await wrapped.DisconnectAsync();

        Assert.Equal(expectedErrorsCount, errors);
        return;

        void HandleWarning(
            Exception _)
        {
            ++errors;
            client.Raise(streamingClient => streamingClient.OnError += null, new TaskCanceledException());
        }
    }

    [Fact]
    public Task OnNormalErrorWithReconnectWorks() => 
        onErrorWithReconnectWorks<InvalidOperationException>();

    [Fact]
    public Task OnTimeoutErrorWithReconnectWorks() => 
        onErrorWithReconnectWorks<TaskCanceledException>();

    [Fact]
    public void GetNewsSubscriptionWorks()
    {
        var client = createMockClient(
            client => client.GetNewsSubscription(It.IsAny<String>()));

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
            client => client.GetNewsSubscription(It.IsAny<String>()));

        await using var subscription = await client.Object.SubscribeNewsAsync(Stock);
        await using var subscriptionOne = await client.Object.SubscribeNewsAsync(_symbols);
        // ReSharper disable once UseAwaitUsing
        using var subscriptionTwo = await client.Object.SubscribeNewsAsync(Stock, Other);

        _symbols.VerifySubscriptionsStreams(subscriptionOne, subscriptionTwo);
        subscription.VerifySubscriptionEventsNumber(ExpectedNumberOfEventsForOneSymbol);

        client.VerifyAll();
    }

    private static async Task onErrorWithReconnectWorks<TException>()
        where TException : Exception, new()
    {
        var client = createMockClient(
            client => client.GetNewsSubscription(It.IsAny<String>()));

        var parameters = new ReconnectionParameters
        {
            MaxReconnectionDelay = TimeSpan.FromMilliseconds(100),
            MinReconnectionDelay = TimeSpan.FromMilliseconds(10),
            MaxReconnectionAttempts = 0
        };
        using var wrapped = client.Object.WithReconnect(parameters);

        wrapped.SocketClosed += HandleClosed;
        wrapped.OnError += HandleError;

        var result = await wrapped.ConnectAndAuthenticateAsync();

        client.Setup(c => c.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthStatus.Authorized);

        // ReSharper disable MethodHasAsyncOverload
        client.Raise(streamingClient => streamingClient.SocketClosed += null);
        client.Raise(streamingClient => streamingClient.SocketClosed += null);
        await Task.Delay(ReconnectionParameters.Default.MaxReconnectionDelay);
        client.Raise(streamingClient => streamingClient.OnError += null, new TException());
        // ReSharper restore MethodHasAsyncOverload

        wrapped.OnError -= HandleError;
        wrapped.SocketClosed -= HandleClosed;

        await wrapped.DisconnectAsync();

        Assert.Equal(AuthStatus.Authorized, result);
        return;

        void HandleError(Exception _) => throw new TException();
        void HandleClosed() => throw new TException();
    }

    private static Mock<IAlpacaNewsStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaNewsStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class => subscriptionFactory.CreateMockClient();
}
