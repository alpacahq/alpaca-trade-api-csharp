using JetBrains.Annotations;

namespace Alpaca.Markets.Tests;

internal sealed class SubscriptionHelper<TItem> : IAsyncDisposable
{
    private sealed record Handler(
        IAlpacaDataSubscription<TItem> Subscription,
        Action<TItem> Validator,
        AutoResetEvent Event)
    {
        public void Subscribe() =>
            Subscription.Received += handleReceived;

        public void Unsubscribe() =>
            Subscription.Received -= handleReceived;

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

    public ValueTask DisposeAsync() => unsubscribeAll();

    [UsedImplicitly]
    public async void Dispose() => await DisposeAsync();

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

    private async ValueTask subscribeAll()
    {
        _handlers.ForEach(_ => _.Subscribe());
        await _client.SubscribeAsync(_handlers.Select(_ => _.Subscription));
    }

    private async ValueTask unsubscribeAll()
    {
        _handlers.ForEach(_ => _.Unsubscribe());
        await _client.UnsubscribeAsync(_handlers.Select(_ => _.Subscription));
    }
}
