using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaDataClient"/> class.
    /// </summary>
    public sealed class AlpacaDataClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataClientConfiguration"/> class.
        /// </summary>
        public AlpacaDataClientConfiguration()
        {
            SecurityId = new SecretKey(String.Empty, String.Empty);
            ApiEndpoint = Environments.Live.AlpacaDataApi;
        }

        /// <summary>
        /// Security identifier for API authentication.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca Data API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HttpClient"/> instance for connecting.
        /// </summary>
        public HttpClient? HttpClient { get; [UsedImplicitly] set; }

        internal void EnsureIsValid()
        {
            if (ApiEndpoint == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }
        }
    }
}
