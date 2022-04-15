namespace Alpaca.Markets.Tests;

internal sealed class SubscriptionHelper<TItem> : IAsyncDisposable
{
    private sealed record Handler(
        IAlpacaDataSubscription<TItem> Subscription,
        Action<TItem> Validator,
        AutoResetEvent Event)
    {
        private Boolean _subscribed;

        public void Subscribe()
        {
            Subscription.OnSubscribedChanged += handleSubscribedChanged;
            Subscription.Received += handleReceived;
        }

        public void Unsubscribe()
        {
            Subscription.OnSubscribedChanged -= handleSubscribedChanged;
            Subscription.Received -= handleReceived;
        }

        private void handleSubscribedChanged()
        {
            Assert.NotEqual(_subscribed, Subscription.Subscribed);
            _subscribed = Subscription.Subscribed;
        }

        private void handleReceived(TItem item)
        {
            Validator(item);
            Event.Set();
        }
    }

    private readonly ISubscriptionHandler _client;

    private readonly List<Handler> _handlers;

    private SubscriptionHelper(
        ISubscriptionHandler client,
        List<Handler> handlers)
    {
        _handlers = handlers;
        _client = client;
    }

    public Boolean WaitAll() =>
        WaitHandle.WaitAll(
            _handlers.Select(_ => _.Event).Cast<WaitHandle>().ToArray(),
            TimeSpan.FromSeconds(1));

    public void Subscribe(
        Action<TItem> handler) =>
        _handlers.ForEach(_ => _.Subscription.Received += handler);

    public void Unsubscribe(
        Action<TItem> handler) =>
        _handlers.ForEach(_ => _.Subscription.Received -= handler);

    public ValueTask DisposeAsync() => unsubscribeAll();

    public static async ValueTask<SubscriptionHelper<TItem>> Create<TClient>(
        TClient client, Action<TItem> validator,
        params Func<TClient, IAlpacaDataSubscription<TItem>>[] factories)
        where TClient : ISubscriptionHandler
    {
        var handlers = factories
            .Select(factory => new Handler(
                factory(client), validator, new AutoResetEvent(false)))
            .ToList();

        var handler = new SubscriptionHelper<TItem>(client, handlers);
        await handler.subscribeAll();

        return handler;
    }

    private ValueTask subscribeAll()
    {
        _handlers.ForEach(_ => _.Subscribe());
        return _handlers.Count == 1
            ? _client.SubscribeAsync(_handlers.Single().Subscription)
            : _client.SubscribeAsync(_handlers.Select(_ => _.Subscription));
    }

    private ValueTask unsubscribeAll()
    {
        _handlers.ForEach(_ => _.Unsubscribe());
        return _handlers.Count == 1
            ? _client.UnsubscribeAsync(_handlers.Single().Subscription)
            : _client.UnsubscribeAsync(_handlers.Select(_ => _.Subscription));
    }
}
