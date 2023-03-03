namespace Alpaca.Markets.Extensions;

public static partial class AlpacaTradingClientExtensions
{
    private static SpinLock _lock = new(false);

    private static IClock? _clock;

    /// <summary>
    /// Gets the <see cref="IClock.IsOpen"/> state from cached API result call.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>If trading day is open now returns <c>true</c> value.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<Boolean> IsMarketOpenAsync(
        this IAlpacaTradingClient client) =>
        IsMarketOpenAsync(client.EnsureNotNull(), CancellationToken.None);

    /// <summary>
    /// Gets the <see cref="IClock.IsOpen"/> state from cached API result call.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>If trading day is open now returns <c>true</c> value.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static async ValueTask<Boolean> IsMarketOpenAsync(
        this IAlpacaTradingClient client,
        CancellationToken cancellationToken) =>
        (await client.EnsureNotNull().GetClockCachedAsync(cancellationToken).ConfigureAwait(false)).IsOpen;

    /// <summary>
    /// Gets the cached <see cref="IClock"/> instance with the current session state.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Cached or value of <see cref="IClock"/> instance or result of REST API call.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static ValueTask<IClock> GetClockCachedAsync(
        this IAlpacaTradingClient client) =>
        GetClockCachedAsync(client.EnsureNotNull(), CancellationToken.None);

    /// <summary>
    /// Gets the cached <see cref="IClock"/> instance with the current session state.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <exception cref="HttpRequestException">
    /// The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.
    /// </exception>
    /// <exception cref="RestClientErrorException">
    /// The response contains an error message or the received response cannot be deserialized properly due to JSON schema mismatch.
    /// </exception>
    /// <exception cref="SocketException">
    /// The initial TPC socket connection failed due to an underlying low-level network connectivity issue.
    /// </exception>
    /// <exception cref="TaskCanceledException">
    /// .NET Core and .NET 5 and later only: The request failed due to timeout.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="client"/> argument is <c>null</c>.
    /// </exception>
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
            true when clock.NextCloseUtc <= DateTime.UtcNow => true, //-V3125
            false when clock.NextOpenUtc <= DateTime.UtcNow => true,
            null => true,
            _ => false
        };
}
