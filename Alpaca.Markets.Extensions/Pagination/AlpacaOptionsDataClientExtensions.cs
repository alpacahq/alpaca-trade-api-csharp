namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extension methods for the <see cref="IAlpacaOptionsDataClient"/> interface.
/// </summary>
public static class AlpacaOptionsDataClientExtensions
{
    /// <summary>
    /// Gets all option snapshots from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaOptionsDataClient"/> object instance.</param>
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
    /// <returns>Option contacts' snapshots obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IOptionSnapshot> ListSnapshotsAsAsyncEnumerable(
        this IAlpacaOptionsDataClient client,
        OptionSnapshotRequest request) =>
        ListSnapshotsAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all option snapshots from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaOptionsDataClient"/> object instance.</param>
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
    /// <returns>Option contacts' snapshots obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IOptionSnapshot> ListSnapshotsAsAsyncEnumerable(
        this IAlpacaOptionsDataClient client,
        OptionSnapshotRequest request,
        CancellationToken cancellationToken) =>
        getAllOptionSnapshotsPages(client.EnsureNotNull(),
            getRequestWithoutPageToken(request.EnsureNotNull()), cancellationToken);

    /// <summary>
    /// Gets all option snapshots from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaOptionsDataClient"/> object instance.</param>
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
    /// <returns>Option contacts' snapshots obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IOptionSnapshot> GetOptionChainAsyncAsAsyncEnumerable(
        this IAlpacaOptionsDataClient client,
        OptionChainRequest request) =>
        GetOptionChainAsyncAsAsyncEnumerable(client, request, CancellationToken.None);

    /// <summary>
    /// Gets all option snapshots from Alpaca REST API endpoint as async enumerable stream.
    /// </summary>
    /// <param name="client">The <see cref="IAlpacaOptionsDataClient"/> object instance.</param>
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
    /// <returns>Option contacts' snapshots obtained page by page.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IOptionSnapshot> GetOptionChainAsyncAsAsyncEnumerable(
        this IAlpacaOptionsDataClient client,
        OptionChainRequest request,
        CancellationToken cancellationToken) =>
        getAllOptionSnapshotsPages(client.EnsureNotNull(),
            getRequestWithoutPageToken(request.EnsureNotNull()), cancellationToken);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static OptionSnapshotRequest getRequestWithoutPageToken(
        OptionSnapshotRequest request) =>
        new(request.Symbols)
        {
            //Pagination = { Size = Pagination.MaxPageSize },
            OptionsFeed = request.OptionsFeed
        };

    private static async IAsyncEnumerable<IOptionSnapshot> getAllOptionSnapshotsPages(
        IAlpacaOptionsDataClient client,
        OptionSnapshotRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        do
        {
            var page = await client.ListSnapshotsAsync(request, cancellationToken).ConfigureAwait(false);

            foreach (var item in page.Items)
            {
                yield return item.Value;
            }

            request.Pagination.Token = page.NextPageToken ?? String.Empty;
        } while (!String.IsNullOrEmpty(request.Pagination.Token));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static OptionChainRequest getRequestWithoutPageToken(
        OptionChainRequest request) =>
        new(request.UnderlyingSymbol)
        {
            ExpirationDateGreaterThanOrEqualTo = request.ExpirationDateGreaterThanOrEqualTo,
            ExpirationDateLessThanOrEqualTo = request.ExpirationDateLessThanOrEqualTo,
            StrikePriceGreaterThanOrEqualTo = request.StrikePriceGreaterThanOrEqualTo,
            StrikePriceLessThanOrEqualTo = request.StrikePriceLessThanOrEqualTo,
            ExpirationDateEqualTo = request.ExpirationDateEqualTo,
            Pagination = { Size = Pagination.MaxPageSize },
            OptionType = request.OptionType,
            RootSymbol = request.RootSymbol
        };

    private static async IAsyncEnumerable<IOptionSnapshot> getAllOptionSnapshotsPages(
        IAlpacaOptionsDataClient client,
        OptionChainRequest request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        do
        {
            var page = await client.GetOptionChainAsync(request, cancellationToken).ConfigureAwait(false);

            foreach (var item in page.Items)
            {
                yield return item.Value;
            }

            request.Pagination.Token = page.NextPageToken ?? String.Empty;
        } while (!String.IsNullOrEmpty(request.Pagination.Token));
    }
}
