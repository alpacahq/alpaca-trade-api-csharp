namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListAssetsAsync(AssetsRequest,CancellationToken)"/> call.
/// </summary>
public sealed class AssetsRequest
{
    /// <summary>
    /// Gets or sets asset status for filtering.
    /// </summary>
    [UsedImplicitly]
    public AssetStatus? AssetStatus { get; set; }

    /// <summary>
    /// Gets or sets asset class for filtering. The <c>null</c> value is equal to <see cref="Markets.AssetClass.UsEquity"/> value.
    /// </summary>
    [UsedImplicitly]
    public AssetClass? AssetClass { get; set; }

    /// <summary>
    /// Gets or sets asset exchange for filtering. The <c>null</c> value means "no filtering by exchanges".
    /// </summary>
    [UsedImplicitly]
    public Exchange? Exchange { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/assets",
            Query = await new QueryBuilder()
                .AddParameter("exchange", Exchange)
                .AddParameter("status", AssetStatus)
                .AddParameter("asset_class", AssetClass)
                .AsStringAsync().ConfigureAwait(false)
        };
}
