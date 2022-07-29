namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for snapshot crypto data requests on Alpaca Data API v2.
/// </summary>
[ExcludeFromCodeCoverage]
[Obsolete("This class will be removed in the next major release of SDK.", true)]
public sealed class SnapshotDataRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="SnapshotDataRequest"/> object.
    /// </summary>
    /// <param name="symbol">Asset symbol for data retrieval.</param>
    /// <param name="exchange">Crypto exchange for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public SnapshotDataRequest(
        String symbol,
        CryptoExchange exchange)
    {
        Symbol = symbol.EnsureNotNull();
        Exchange = exchange;
    }

    /// <summary>
    /// Gets asset symbol for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    /// <summary>
    /// Gets crypto exchange for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public CryptoExchange Exchange { get; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("exchange", Exchange.ToEnumString())
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"../../v1beta1/crypto/{Symbol}/snapshot");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
