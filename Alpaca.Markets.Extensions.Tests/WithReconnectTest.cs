namespace Alpaca.Markets.Extensions.Tests;

public sealed class WithReconnectTest
{
    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    [Fact]
    public async Task AlpacaStreamingClientWithReconnectWorks()
    {
        const Int32 expectedEventsCount = 4;

        var count = 0;
        var client = new Mock<IAlpacaStreamingClient>();

        client.SetupAdd(streamingClient =>
            streamingClient.OnTradeUpdate += It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);
        client.SetupRemove(streamingClient =>
            streamingClient.OnTradeUpdate -= It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);

        var reconnectionParameters = new ReconnectionParameters
        {
            MaxReconnectionDelay = TimeSpan.FromMilliseconds(500),
            MinReconnectionDelay = TimeSpan.FromMilliseconds(10),
            MaxReconnectionAttempts = 2
        };

        using (var wrapped = client.Object.WithReconnect(reconnectionParameters))
        {
            await wrapped.ConnectAsync();

            wrapped.OnTradeUpdate += HandleTradeUpdate;

            // ReSharper disable MethodHasAsyncOverload
            client.Raise(streamingClient => streamingClient.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);

            client.Raise(streamingClient => streamingClient.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(streamingClient => streamingClient.SocketOpened += null);

            client.Raise(streamingClient => streamingClient.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);
            // ReSharper restore MethodHasAsyncOverload

            wrapped.OnTradeUpdate -= HandleTradeUpdate;
        }

        Assert.Equal(expectedEventsCount, count);
        client.Verify();
        return;

        void HandleTradeUpdate(ITradeUpdate _) => ++count;
    }

    [Fact]
    public async Task AlpacaNewsClientWithReconnectWorks()
    {
        var client = new Mock<IAlpacaNewsStreamingClient>();

        client
            .Setup(streamingClient => streamingClient.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthStatus.Authorized);

        client
            .Setup(streamingClient => streamingClient.GetNewsSubscription(It.IsAny<String>()))
            .Returns<String>(GetSubscription);

        var reconnectionParameters = new ReconnectionParameters
        {
            MaxReconnectionDelay = TimeSpan.FromMilliseconds(500),
            MinReconnectionDelay = TimeSpan.FromMilliseconds(10),
            MaxReconnectionAttempts = 2
        };

        using (var wrapped = client.Object.WithReconnect(reconnectionParameters))
        {
            Assert.Equal(AuthStatus.Authorized, await wrapped.ConnectAndAuthenticateAsync());

            var subscriptions = new []
            {
                wrapped.GetNewsSubscription(Stock),
                wrapped.GetNewsSubscription(Other)
            };

            await wrapped.SubscribeAsync(subscriptions.Skip(1));
            await wrapped.SubscribeAsync(subscriptions[0]);

            // ReSharper disable MethodHasAsyncOverload
            client.Raise(streamingClient => streamingClient.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(streamingClient => streamingClient.SocketOpened += null);
            // ReSharper restore MethodHasAsyncOverload

            await wrapped.UnsubscribeAsync(subscriptions[0]);
            await wrapped.UnsubscribeAsync(subscriptions.Skip(1));
        }

        client.Verify();
        return;

        IAlpacaDataSubscription<INewsArticle> GetSubscription(String symbol)
        {
            var mock = new Mock<IAlpacaDataSubscription<INewsArticle>>();
            mock.Setup(subscription => subscription.Streams).Returns(Enumerable.Repeat(symbol, 1));
            return mock.Object;
        }
    }

    [Fact]
    public async Task ClientWarningsWithReconnectWorks()
    {
        var warning = Guid.NewGuid().ToString("N");
        var count = 0;

        var client = new Mock<IAlpacaStreamingClient>();
        client.SetupAllProperties();

        using (var wrapped = client.Object.WithReconnect())
        {
            await wrapped.ConnectAsync();

            wrapped.OnWarning += HandleWarning;
            
            // ReSharper disable once MethodHasAsyncOverload
            client.Raise(streamingClient => streamingClient.OnWarning += null, warning);

            wrapped.OnWarning -= HandleWarning;
        }

        Assert.Equal(1, count);
        client.Verify();
        return;

        void HandleWarning(String message)
        {
            Assert.Equal(warning, message);
            ++count;
        }
    }
}
