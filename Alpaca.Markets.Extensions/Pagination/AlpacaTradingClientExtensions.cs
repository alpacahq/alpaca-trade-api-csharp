namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaTradingClient"/> interface.
/// </summary>
public static partial class AlpacaTradingClientExtensions
{
    /// <summary>
    /// Gets all account activities from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="request">Account activities request parameters.</param>
    /// <returns>Account activity record objects obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IAccountActivity> ListAccountActivitiesAsAsyncEnumerable(
        this IAlpacaTradingClient client,
        AccountActivitiesRequest request) =>
        ListAccountActivitiesAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all account activities from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaDataClient"/> object instance.</param>
    /// <param name="request">Account activities request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Account activity record objects obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IAccountActivity> ListAccountActivitiesAsAsyncEnumerable(
        this IAlpacaTradingClient client,
        AccountActivitiesRequest request,
        CancellationToken cancellationToken) =>
        getAllAccountActivitiesPages(client.EnsureNotNull(),
            getRequestWithoutPageToken(request.EnsureNotNull()), cancellationToken);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static AccountActivitiesRequest getRequestWithoutPageToken(
        AccountActivitiesRequest request) =>
        new (request.ActivityTypes)
        {
            Direction = request.Direction
        };

    private static async IAsyncEnumerable<IAccountActivity> getAllAccountActivitiesPages(
        IAlpacaTradingClient client,
        AccountActivitiesRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        do
        {
            var activities = await client
                .ListAccountActivitiesAsync(request, cancellationToken).ConfigureAwait(false);
            if (activities.Count == 0)
            {
                break;
            }

            foreach (var item in activities)
            {
                yield return item;
            }

            request.PageToken = activities[^1].ActivityId;
        } while (!String.IsNullOrEmpty(request.PageToken));
    }
}
