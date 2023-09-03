using System.Net.WebSockets;

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

        client.SetupAdd(c => c.OnTradeUpdate += It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);
        client.SetupRemove(c => c.OnTradeUpdate -= It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);

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
            client.Raise(c => c.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);

            client.Raise(c => c.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(c => c.SocketOpened += null);

            client.Raise(c => c.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);
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
            .Setup(c => c.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                client.Raise(c => c.OnError += null, new WebSocketException());
                client
                    .Setup(c => c.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(AuthStatus.Authorized);
                return AuthStatus.Unauthorized;
            });

        client
            .Setup(c => c.GetNewsSubscription(It.IsAny<String>()))
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
            client.Raise(c => c.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(c => c.SocketOpened += null);
            // ReSharper restore MethodHasAsyncOverload

            await wrapped.UnsubscribeAsync(subscriptions[0]);
            await wrapped.UnsubscribeAsync(subscriptions.Skip(1));
        }

        client.Verify();
        return;

        IAlpacaDataSubscription<INewsArticle> GetSubscription(String symbol)
        {
            var mock = new Mock<IAlpacaDataSubscription<INewsArticle>>();
            mock.Setup(s => s.Streams).Returns(Enumerable.Repeat(symbol, 1));
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
            client.Raise(c => c.OnWarning += null, warning);

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
