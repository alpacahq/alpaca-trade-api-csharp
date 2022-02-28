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
    /// Gets last bar for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only last bar information.</returns>
    [UsedImplicitly]
    Task<IBar> GetLatestBarAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets last bars for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest bars information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IBar>> ListLatestBarsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets last trade for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only last trade information.</returns>
    [UsedImplicitly]
    Task<ITrade> GetLatestTradeAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets last trades for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest trades information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, ITrade>> ListLatestTradesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current quote for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current quote information.</returns>
    [UsedImplicitly]
    Task<IQuote> GetLatestQuoteAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets last quotes for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the latest quotes information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, IQuote>> ListLatestQuotesAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for singe asset from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbol">Asset name for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only current snapshot information.</returns>
    [UsedImplicitly]
    Task<ISnapshot> GetSnapshotAsync(
        String symbol,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, ISnapshot>> ListSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets current snapshot (latest trade/quote and minute/days bars) for several assets from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="symbols">List of asset names for data retrieval.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only dictionary with the current snapshot information.</returns>
    [UsedImplicitly]
    [Obsolete("This method will be removed in the next major release, use the ListSnapshotsAsync method instead.", false)]
    Task<IReadOnlyDictionary<String, ISnapshot>> GetSnapshotsAsync(
        IEnumerable<String> symbols,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with exchange code to the exchange name mappings.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListExchangesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with trades conditions code to the condition description mappings.
    /// </summary>
    /// <param name="tape">SIP tape identifier for retrieving trade conditions.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListTradeConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets dictionary with quotes conditions code to the condition description mappings.
    /// </summary>
    /// <param name="tape">SIP tape identifier for retrieving quote conditions.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns></returns>
    [UsedImplicitly]
    Task<IReadOnlyDictionary<String, String>> ListQuoteConditionsAsync(
        Tape tape,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical new articles list from Alpaca REST API endpoint.
    /// </summary>
    /// <param name="request">Historical news articles request parameters.</param>
    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Read-only list of historical news articles for specified parameters (with pagination data).</returns>
    [UsedImplicitly]
    Task<IPage<INewsArticle>> ListNewsArticlesAsync(
        NewsArticlesRequest request,
        CancellationToken cancellationToken = default);
}
