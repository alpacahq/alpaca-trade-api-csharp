using System;
using System.Net.Http;
using System.Threading.Tasks;
using JetBrains.Annotations;

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
        [UsedImplicitly]
        public AssetStatus? AssetStatus { get; set; }

        /// <summary>
        /// Gets or sets asset class for filtering.
        /// </summary>
        [UsedImplicitly]
        public AssetClass? AssetClass { get; set; }

        internal async ValueTask<UriBuilder> GetUriBuilderAsync(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress!)
            {
                Path = "v2/assets",
                Query = await new QueryBuilder()
                    .AddParameter("status", AssetStatus)
                    .AddParameter("asset_class", AssetClass)
                    .AsStringAsync().ConfigureAwait(false)
            };
    }
}
