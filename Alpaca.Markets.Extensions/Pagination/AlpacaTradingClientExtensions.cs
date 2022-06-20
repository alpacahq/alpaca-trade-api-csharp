using System.Net.Sockets;

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
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
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
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
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
    /// <exception cref="RequestValidationException">
    /// The <paramref name="request"/> argument contains invalid data or some required data is missing, unable to create a valid HTTP request.
    /// </exception>
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
    /// The <paramref name="client"/> or <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
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

            foreach (var item in activities)
            {
                yield return item;
            }

            request.PageToken = activities.Count != 0 ? activities[^1].ActivityId : String.Empty;
        } while (!String.IsNullOrEmpty(request.PageToken));
    }
}
