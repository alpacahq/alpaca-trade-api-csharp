namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaCryptoStreamingClient"/> interface.
/// </summary>
public static partial class AlpacaCryptoStreamingClientExtensions
{
    /// <summary>
    /// Gets the order book updates subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaCryptoStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for order book updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IOrderBook}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(
        this IAlpacaCryptoStreamingClient client,
        params String[] symbols) =>
        getOrderBookSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the order book updates subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaCryptoStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for order book updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IOrderBook}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<IOrderBook> GetOrderBookSubscription(
        this IAlpacaCryptoStreamingClient client,
        IEnumerable<String> symbols) =>
        getOrderBookSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the order book updates subscription for the <paramref name="symbol"/> asset. This subscription is
    /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
    /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaCryptoStreamingClient"/> interface.</param>
    /// <param name="symbol">Alpaca asset name for order book updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IOrderBook}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IOrderBook>> SubscribeOrderBookAsync(
        this IAlpacaCryptoStreamingClient client,
        String symbol) =>
        DisposableAlpacaDataSubscription<IOrderBook>.CreateAsync(
            client.EnsureNotNull().GetOrderBookSubscription(symbol.EnsureNotNull()),
            client);

    /// <summary>
    /// Gets the order book updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaCryptoStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for order book updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IOrderBook}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IOrderBook>> SubscribeOrderBookAsync(
        this IAlpacaCryptoStreamingClient client,
        params String[] symbols) =>
        DisposableAlpacaDataSubscription<IOrderBook>.CreateAsync(
            client.EnsureNotNull().GetOrderBookSubscription(symbols.EnsureNotNull()),
            client);

    /// <summary>
    /// Gets the order book updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaCryptoStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for order book updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IOrderBook}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IOrderBook>> SubscribeOrderBookAsync(
        this IAlpacaCryptoStreamingClient client,
        IEnumerable<String> symbols) =>
        DisposableAlpacaDataSubscription<IOrderBook>.CreateAsync(
            client.EnsureNotNull().GetOrderBookSubscription(symbols.EnsureNotNull()),
            client);

    private static IAlpacaDataSubscription<IOrderBook> getOrderBookSubscription(
        IAlpacaCryptoStreamingClient client,
        IEnumerable<String> symbols) =>
        getSubscription(client.GetOrderBookSubscription, symbols);

    private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
        Func<String, IAlpacaDataSubscription<TItem>> selector,
        IEnumerable<String> symbols) =>
        new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
}
