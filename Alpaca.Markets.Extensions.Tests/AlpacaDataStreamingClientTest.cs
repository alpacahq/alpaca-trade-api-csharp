using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Alpaca.Markets.Extensions.Tests;

public sealed partial class AlpacaDataStreamingClientTest
{
    private sealed class Subscription<TItem> : IAlpacaDataSubscription<TItem>
        where TItem : class
    {
        private readonly String _stream;

        public Subscription(
            String stream) =>
            _stream = stream;

        public IEnumerable<String> Streams
        {
            get { yield return _stream; }
        }

        public Boolean Subscribed
        {
            get
            {
                Received?.Invoke(new Mock<TItem>().Object);
                OnSubscribedChanged?.Invoke();
                return true;
            }
        }

        public event Action? OnSubscribedChanged;

        public event Action<TItem>? Received;
    }

    private static readonly List<String> _symbols = new () { Stock, Other };

    private const String Stock = "AAPL";

    private const String Other = "MSFT";

    [Fact]
    public async Task WithReconnectWorks()
    {
        var client = createMockClient(
            _ => _.GetTradeSubscription(It.IsAny<String>()));

        var wrapped = client.Object.WithReconnect();
        var result = await wrapped.ConnectAndAuthenticateAsync();

        Assert.Equal(AuthStatus.Authorized, result);
    }

    private static Mock<IAlpacaDataStreamingClient> createMockClient<TItem>(
        Expression<Func<IAlpacaDataStreamingClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TItem : class
    {
        var client = new Mock<IAlpacaDataStreamingClient>();
        var subscriptions = new ConcurrentDictionary<String, IAlpacaDataSubscription<TItem>>();

        client.Setup(subscriptionFactory).Returns<String>(stream =>
            subscriptions.GetOrAdd(stream, new Subscription<TItem>(stream)));

        return client;
    }

    private static void verifySubscriptions<TItem>(
        IAlpacaDataSubscription<TItem> lhs,
        IAlpacaDataSubscription<TItem> rhs)
    {
        Assert.NotEqual(lhs, rhs);
        Assert.Equal(_symbols, lhs.Streams);
        Assert.Equal(_symbols, rhs.Streams);
    }

    private static void verifySubscriptionEvents<TItem>(
        IAlpacaDataSubscription<TItem> subscription,
        Int32 expectedNumberOfEvents)
    {
        var count = 0;
        subscription.Received += HandleReceived;
        subscription.OnSubscribedChanged += HandleChanged;

        Assert.True(subscription.Subscribed); // Raises both events for subscription

        subscription.OnSubscribedChanged -= HandleChanged;
        subscription.Received -= HandleReceived;

        Assert.Equal(expectedNumberOfEvents, count);

        void HandleReceived(TItem _) => ++count;
        void HandleChanged() => ++count;
    }
}
