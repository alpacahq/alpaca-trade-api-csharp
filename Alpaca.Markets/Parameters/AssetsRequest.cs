using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.ListAssetsAsync(AssetsRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class AssetsRequest
    {
        /// <summary>
        /// Gets or sets asset status for filtering.
        /// </summary>
        public AssetStatus? AssetStatus { get; set; }

        /// <summary>
        /// Gets or sets asset class for filtering.
        /// </summary>
        public AssetClass? AssetClass { get; set; }

        internal UriBuilder GetUriBuilder(HttpClient httpClient) =>
            new UriBuilder(httpClient.BaseAddress!)
            {
                Path = "v2/assets",
                Query = new QueryBuilder()
                    .AddParameter("status", AssetStatus)
                    .AddParameter("asset_class", AssetClass)
            };
    }
}
