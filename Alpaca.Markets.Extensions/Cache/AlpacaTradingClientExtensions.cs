namespace Alpaca.Markets.Extensions;

public static partial class AlpacaTradingClientExtensions
{
    private static SpinLock _lock = new (false);

    private static IClock? _clock;

    /// <summary>
    /// Gets the <see cref="IClock.IsOpen"/> state from cached API result call.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <returns>If trading day is open now returns <c>true</c> value.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<Boolean> IsMarketOpenAsync(
        this IAlpacaTradingClient client) =>
        IsMarketOpenAsync(client, CancellationToken.None);

    /// <summary>
    /// Gets the <see cref="IClock.IsOpen"/> state from cached API result call.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>If trading day is open now returns <c>true</c> value.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static async ValueTask<Boolean> IsMarketOpenAsync(
        this IAlpacaTradingClient client,
        CancellationToken cancellationToken) =>
        (await client.GetClockCachedAsync(cancellationToken).ConfigureAwait(false)).IsOpen;

    /// <summary>
    /// Gets the cached <see cref="IClock"/> instance with the current session state.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <returns>Cached or value of <see cref="IClock"/> instance or result of REST API call.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IClock> GetClockCachedAsync(
        this IAlpacaTradingClient client) =>
        GetClockCachedAsync(client, CancellationToken.None);

    /// <summary>
    /// Gets the cached <see cref="IClock"/> instance with the current session state.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Cached or value of <see cref="IClock"/> instance or result of REST API call.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static async ValueTask<IClock> GetClockCachedAsync(
        this IAlpacaTradingClient client,
        CancellationToken cancellationToken)
    {
        var lockTaken = false;
        _lock.Enter(ref lockTaken);

        if (!lockTaken)
        {
            return await client.EnsureNotNull()
                .GetClockAsync(cancellationToken).ConfigureAwait(false);
        }

        try
        {
            return isCachedClockValueExpired(_clock)
                ? _clock = await client.EnsureNotNull()
                    .GetClockAsync(cancellationToken).ConfigureAwait(false)
                : _clock!;
        }
        finally
        {
            _lock.Exit(false);
        }
    }

    private static Boolean isCachedClockValueExpired(
        IClock? clock) =>
        clock?.IsOpen switch
        {
            true when clock.NextCloseUtc <= DateTime.UtcNow => true,
            false when clock.NextOpenUtc <= DateTime.UtcNow => true,
            null => true,
            _ => false
        };
}
