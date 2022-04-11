namespace Alpaca.Markets.Extensions.Tests;

public sealed class WithReconnectTest
{
    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    [Fact]
    public async Task AlpacaStreamingClientWithReconnectWorks()
    {
        var count = 0;
        var client = new Mock<IAlpacaStreamingClient>();

        client.SetupAdd(_ => _.OnTradeUpdate += It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);
        client.SetupRemove(_ => _.OnTradeUpdate -= It.IsAny<Action<ITradeUpdate>>()).Callback(() => ++count);

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

            client.Raise(_ => _.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);

            client.Raise(_ => _.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(_ => _.SocketOpened += null);

            client.Raise(_ => _.OnTradeUpdate += null, new Mock<ITradeUpdate>().Object);

            wrapped.OnTradeUpdate -= HandleTradeUpdate;
        }

        Assert.Equal(4, count);
        client.Verify();

        void HandleTradeUpdate(ITradeUpdate _) => ++count;
    }

    [Fact]
    public async Task AlpacaNewsClientWithReconnectWorks()
    {
        //var count = 0;
        var client = new Mock<IAlpacaNewsStreamingClient>();

        client
            .Setup(_ => _.ConnectAndAuthenticateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(AuthStatus.Authorized);

        client
            .Setup(_ => _.GetNewsSubscription(It.IsAny<String>()))
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

            client.Raise(_ => _.SocketClosed += null);
            await Task.Delay(reconnectionParameters.MaxReconnectionDelay);
            client.Raise(_ => _.SocketOpened += null);

            await wrapped.UnsubscribeAsync(subscriptions[0]);
            await wrapped.UnsubscribeAsync(subscriptions.Skip(1));
        }

        //Assert.Equal(4, count);
        client.Verify();

        IAlpacaDataSubscription<INewsArticle> GetSubscription(String symbol)
        {
            var mock = new Mock<IAlpacaDataSubscription<INewsArticle>>();
            mock.Setup(_ => _.Streams).Returns(Enumerable.Repeat(symbol, 1));
            return mock.Object;
        }
    }
}
