namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaTradingClient"/> interface.
/// </summary>
public static partial class AlpacaTradingClientExtensions
{
    /// <summary>
    /// Get single trading day information from the Alpaca REST API.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="date">The trading date (time part will not be used).</param>
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
    /// <returns>Read-only trading date information object.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static async Task<IIntervalCalendar?> GetCalendarForSingleDayAsync(
        this IAlpacaTradingClient client,
        DateOnly date,
        CancellationToken cancellationToken = default)
    {
        var calendars = await client.EnsureNotNull()
            .ListIntervalCalendarAsync(CalendarRequest.GetForSingleDay(date), cancellationToken)
            .ConfigureAwait(false);
        return calendars.SingleOrDefault();
    }
}
