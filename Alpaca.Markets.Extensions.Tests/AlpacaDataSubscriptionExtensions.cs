using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Alpaca.Markets.Extensions.Tests;

internal static class AlpacaDataSubscriptionExtensions
{
    private sealed class Subscription<TItem>(
        String stream) : IAlpacaDataSubscription<TItem>
        where TItem : class
    {
        public IEnumerable<String> Streams
        {
            get { yield return stream; }
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

    internal static Mock<TClient> CreateMockClient<TClient, TItem>(
        this Expression<Func<TClient, IAlpacaDataSubscription<TItem>>> subscriptionFactory)
        where TClient : class, IStreamingClient
        where TItem : class
    {
        var client = new Mock<TClient>();
        var subscriptions = new ConcurrentDictionary<String, IAlpacaDataSubscription<TItem>>();

        client.Setup(subscriptionFactory).Returns<String>(stream =>
            subscriptions.GetOrAdd(stream, new Subscription<TItem>(stream)));

        return client;
    }

    internal static void VerifySubscriptionsStreams<TItem>(
        this IReadOnlyList<String> streams,
        IAlpacaDataSubscription<TItem> lhs,
        IAlpacaDataSubscription<TItem> rhs)
    {
        Assert.NotEqual(lhs, rhs);
        Assert.Equal(streams, lhs.Streams);
        Assert.Equal(streams, rhs.Streams);
    }

    internal static void VerifySubscriptionEventsNumber<TItem>(
        this IAlpacaDataSubscription<TItem> subscription,
        Int32 expectedNumberOfEvents)
    {
        var count = 0;
        subscription.Received += HandleReceived;
        subscription.OnSubscribedChanged += HandleChanged;

        Assert.True(subscription.Subscribed); // Raises both events for subscription

        subscription.OnSubscribedChanged -= HandleChanged;
        subscription.Received -= HandleReceived;

        Assert.Equal(expectedNumberOfEvents, count);
        return;

        void HandleReceived(TItem _) => ++count;
        void HandleChanged() => ++count;
    }
}
