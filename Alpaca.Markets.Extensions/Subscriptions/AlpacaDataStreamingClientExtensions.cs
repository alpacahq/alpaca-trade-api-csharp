namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaDataStreamingClient"/> interface.
/// </summary>
public static partial class AlpacaDataStreamingClientExtensions
{
    /// <summary>
    /// Gets the trading statuses updates subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<IStatus> GetStatusSubscription(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        getStatusSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trading statuses subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<IStatus> GetStatusSubscription(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getStatusSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        getCorrectionSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ICorrection> GetCorrectionSubscription(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getCorrectionSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trade cancellation subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ITrade> GetCancellationSubscription(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        getCancellationSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trade cancellation subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ITrade> GetCancellationSubscription(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getCancellationSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the LULD (limit up / limit down) updates subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        getLimitUpLimitDownSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the LULD (limit up / limit down) subscription for the all assets from the <paramref name="symbols"/> list.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute aggregate (bar) updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{TApi}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAlpacaDataSubscription<ILimitUpLimitDown> GetLimitUpLimitDownSubscription(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getLimitUpLimitDownSubscription(
            client.EnsureNotNull(),
            symbols.EnsureNotNull());

    /// <summary>
    /// Gets the trading status updates subscription for the <paramref name="symbol"/> asset. This subscription is
    /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
    /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
        this IAlpacaDataStreamingClient client,
        String symbol) =>
        DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
            client.EnsureNotNull().GetStatusSubscription(symbol.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trading status updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
            client.EnsureNotNull().GetStatusSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<IStatus>> SubscribeStatusAsync(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        DisposableAlpacaDataSubscription<IStatus>.CreateAsync(
            client.EnsureNotNull().GetStatusSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ICorrection>> SubscribeCorrectionAsync(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        DisposableAlpacaDataSubscription<ICorrection>.CreateAsync(
            client.EnsureNotNull().GetCorrectionSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections subscription for the <paramref name="symbol"/> asset. This subscription is
    /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
    /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ICorrection>> SubscribeCorrectionAsync(
        this IAlpacaDataStreamingClient client,
        String symbol) =>
        DisposableAlpacaDataSubscription<ICorrection>.CreateAsync(
            client.EnsureNotNull().GetCorrectionSubscription(symbol.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade corrections updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ICorrection>> SubscribeCorrectionAsync(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        DisposableAlpacaDataSubscription<ICorrection>.CreateAsync(
            client.EnsureNotNull().GetCorrectionSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade cancellations updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeCancellationAsync(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
            client.EnsureNotNull().GetCancellationSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade cancellations subscription for the <paramref name="symbol"/> asset. This subscription is
    /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
    /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeCancellationAsync(
        this IAlpacaDataStreamingClient client,
        String symbol) =>
        DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
            client.EnsureNotNull().GetCancellationSubscription(symbol.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the trade cancellations updates subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ITrade>> SubscribeCancellationAsync(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        DisposableAlpacaDataSubscription<ITrade>.CreateAsync(
            client.EnsureNotNull().GetCancellationSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the LULD (limit up / limit down) subscription for the <paramref name="symbol"/> asset. This subscription is
    /// returned with pending subscription state and will be unsubscribed on disposing so you can use it
    /// inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbol">Alpaca asset name for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamAgg}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
        this IAlpacaDataStreamingClient client,
        String symbol) =>
        DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
            client.EnsureNotNull().GetLimitUpLimitDownSubscription(symbol.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the LULD (limit up / limit down) subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
        this IAlpacaDataStreamingClient client,
        params String[] symbols) =>
        DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
            client.EnsureNotNull().GetLimitUpLimitDownSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    /// <summary>
    /// Gets the LULD (limit up / limit down) subscription for all assets from the <paramref name="symbols"/> list.
    /// This subscription is returned with pending subscription state and will be unsubscribed on disposing
    /// so you can use it inside the <c>using</c> or <c>await using</c> statements for more clear resource management.
    /// </summary>
    /// <param name="client">Target instance of the <see cref="IAlpacaDataStreamingClient"/> interface.</param>
    /// <param name="symbols">Alpaca asset names list (non-empty) for minute bar updates subscribing.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> or <paramref name="symbols"/> argument is <c>null</c>.
    /// </exception>
    /// <exception cref="OverflowException">
    /// The underlying subscriptions dictionary contains too many elements.
    /// </exception>
    /// <returns>
    /// Subscription object for tracking updates via the <see cref="IAlpacaDataSubscription{IStreamTrade}.Received"/> event.
    /// </returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IDisposableAlpacaDataSubscription<ILimitUpLimitDown>> SubscribeLimitUpLimitDownAsync(
        this IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        DisposableAlpacaDataSubscription<ILimitUpLimitDown>.CreateAsync(
            client.EnsureNotNull().GetLimitUpLimitDownSubscription(symbols.EnsureNotNull()),
            client.EnsureNotNull());

    private static IAlpacaDataSubscription<IStatus> getStatusSubscription(
        IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getSubscription(client.GetStatusSubscription, symbols);

    private static IAlpacaDataSubscription<ILimitUpLimitDown> getLimitUpLimitDownSubscription(
        IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getSubscription(client.GetLimitUpLimitDownSubscription, symbols);

    private static IAlpacaDataSubscription<ITrade> getCancellationSubscription(
        IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getSubscription(client.GetCancellationSubscription, symbols);

    private static IAlpacaDataSubscription<ICorrection> getCorrectionSubscription(
        IAlpacaDataStreamingClient client,
        IEnumerable<String> symbols) =>
        getSubscription(client.GetCorrectionSubscription, symbols);

    private static IAlpacaDataSubscription<TItem> getSubscription<TItem>(
        Func<String, IAlpacaDataSubscription<TItem>> selector,
        IEnumerable<String> symbols) =>
        new AlpacaDataSubscriptionContainer<TItem>(symbols.Select(selector));
}
