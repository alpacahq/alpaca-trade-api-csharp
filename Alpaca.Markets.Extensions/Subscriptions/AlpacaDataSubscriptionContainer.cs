namespace Alpaca.Markets.Extensions;

internal sealed class AlpacaDataSubscriptionContainer<TItem>
    : IAlpacaDataSubscription<TItem>
{
    private readonly IReadOnlyList<IAlpacaDataSubscription<TItem>> _subscriptions;

    internal AlpacaDataSubscriptionContainer(
        IEnumerable<IAlpacaDataSubscription<TItem>> subscriptions) =>
        _subscriptions = subscriptions.ToList();

    public IEnumerable<String> Streams =>
        _subscriptions.SelectMany(subscription => subscription.Streams);

    public Boolean Subscribed =>
        _subscriptions.All(subscription => subscription.Subscribed);

    public event Action? OnSubscribedChanged
    {
        add
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.OnSubscribedChanged += value;
            }
        }
        remove
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.OnSubscribedChanged -= value;
            }
        }
    }

    public event Action<TItem>? Received
    {
        add
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Received += value;
            }
        }
        remove
        {
            foreach (var subscription in _subscriptions)
            {
                subscription.Received -= value;
            }
        }
    }
}
