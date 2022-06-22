namespace Alpaca.Markets.Extensions;

internal sealed class DisposableAlpacaDataSubscription<TItem> :
    IDisposableAlpacaDataSubscription<TItem>
{
    private readonly IAlpacaDataSubscription<TItem> _subscription;

    private ISubscriptionHandler? _client;

    private DisposableAlpacaDataSubscription(
        IAlpacaDataSubscription<TItem> subscription,
        ISubscriptionHandler client)
    {
        _subscription = subscription;
        _client = client;
    }

    internal static async ValueTask<IDisposableAlpacaDataSubscription<TItem>> CreateAsync(
        IAlpacaDataSubscription<TItem> subscription,
        ISubscriptionHandler client)
    {
        await client.SubscribeAsync(subscription).ConfigureAwait(false);
        return new DisposableAlpacaDataSubscription<TItem>(subscription, client);
    }

    public IEnumerable<String> Streams => _subscription.Streams;

    public Boolean Subscribed => _subscription.Subscribed;

    public event Action? OnSubscribedChanged
    {
        add => _subscription.OnSubscribedChanged += value;
        remove => _subscription.OnSubscribedChanged -= value;
    }

    public event Action<TItem>? Received
    {
        add => _subscription.Received += value;
        remove => _subscription.Received -= value;
    }

    public async void Dispose() =>
        await DisposeAsync().ConfigureAwait(false);

    public async ValueTask DisposeAsync()
    {
        if (_client is null)
        {
            return;
        }

        await _client.UnsubscribeAsync(_subscription)
            .ConfigureAwait(false);
        _client = null;
    }
}
