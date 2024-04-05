namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest options data requests on Alpaca Data API v2.
/// </summary>
public sealed class OptionChainRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="OptionChainRequest"/> object.
    /// </summary>
    /// <param name="underlyingSymbol">Option underlying symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="underlyingSymbol"/> argument is <c>null</c>.
    /// </exception>
    public OptionChainRequest(
        String underlyingSymbol) =>
        UnderlyingSymbol = underlyingSymbol.EnsureNotNull();

    /// <summary>
    /// Gets options symbols list for data retrieval.
    /// </summary>
    [UsedImplicitly]

    public String UnderlyingSymbol { get; }

    /// <summary>
    /// Gets options feed for data retrieval.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    public OptionsFeed? OptionsFeed { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await new QueryBuilder()
                .AddParameter("feed", OptionsFeed)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"snapshots/{UnderlyingSymbol}");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return UnderlyingSymbol.TryValidateSymbolName();
    }
}
