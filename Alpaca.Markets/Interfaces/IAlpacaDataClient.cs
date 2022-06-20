namespace Alpaca.Markets;

/// <summary>
/// Provides unified type-safe access for Alpaca Data API via HTTP/REST.
/// </summary>
[CLSCompliant(false)]
public interface IAlpacaDataClient :
    IHistoricalQuotesClient<HistoricalQuotesRequest>,
    IHistoricalTradesClient<HistoricalTradesRequest>,
    IHistoricalBarsClient<HistoricalBarsRequest>,
    IDisposable
{
    /// <summary>
    /// Gets most recent bar for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only latest bar information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IBar> GetLatestBarAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent bar for single asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest bar data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only latest bar information.</returns>
    [UsedImplicitly]
    Task<IBar> GetLatestBarAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent bars for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset symbols for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest bars information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent bars for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest bar data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only dictionary with the latest bars information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trade for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only latest trade information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<ITrade> GetLatestTradeAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trade for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest trade data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only latest trade information.</returns>
    [UsedImplicitly]
    Task<ITrade> GetLatestTradeAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trades for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset symbols for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest trades information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent trades for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest trade data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only dictionary with the latest trades information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quote for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only latest quote information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IQuote> GetLatestQuoteAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quote for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest quote data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only current quote information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestQuoteAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quotes for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset symbols for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest quotes information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets most recent quotes for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest quote data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only dictionary with the latest quotes information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset symbols for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current snapshot information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<ISnapshot> GetSnapshotAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest snapshot data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only current snapshot information.</returns>
    [UsedImplicitly]
    Task<ISnapshot> GetSnapshotAsync(
        LatestMarketDataRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset symbols for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the overload with LatestMarketDataRequest type instead.", true)]
    Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Latest snapshot data request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        LatestMarketDataListRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the ListSnapshotsAsync method instead.", true)]
    Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with exchange code to the exchange name mappings.
    /// </summary>
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
    /// <returns>Read-only dictionary where the key is the exchange code and the value is the code's corresponding exchange name.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with trades conditions code to the condition description mappings.
    /// </summary>
    /// <param name="tape">SIP tape identifier for retrieving trade conditions.</param>
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
    /// <returns>Read-only dictionary where the key is the trade conditions code and the value is the corresponding condition description.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with quotes conditions code to the condition description mappings.
    /// </summary>
    /// <param name="tape">SIP tape identifier for retrieving quote conditions.</param>
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
    /// <returns>Read-only dictionary where the key is the quote conditions code and the value is the corresponding condition description.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical news articles list from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical news articles request parameters.</param>
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
    /// The <paramref name="request"/> argument is <c>null</c>.
    /// </exception>
    /// <returns>Read-only list of historical news articles for specified parameters (with pagination data).</returns>
    [UsedImplicitly]
    Task<IPage<INewsArticle>> ListNewsArticlesAsync(
        NewsArticlesRequest request,
        CancellationToken cancellationToken = default);
}
