﻿namespace Alpaca.Markets.Extensions;

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
    /// <returns>Read-only trading date information object.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    [ExcludeFromCodeCoverage]
    [Obsolete("Use another method overload that takes the DateOnly argument.", false)]
    public static async Task<ICalendar?> GetCalendarForSingleDayAsync(
        this IAlpacaTradingClient client,
        DateTime date,
        CancellationToken cancellationToken = default)
    {
        var calendars = await client.EnsureNotNull()
            .ListCalendarAsync(CalendarRequest.GetForSingleDay(DateOnly.FromDateTime(date)), cancellationToken)
            .ConfigureAwait(false);
        return calendars.SingleOrDefault();
    }

    /// <summary>
    /// Get single trading day information from the Alpaca REST API.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="date">The trading date (time part will not be used).</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
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
