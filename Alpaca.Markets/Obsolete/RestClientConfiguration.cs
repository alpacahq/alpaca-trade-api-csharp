using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="RestClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    internal sealed class RestClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="RestClientConfiguration"/> class.
        /// </summary>
        public RestClientConfiguration()
        {
            KeyId = String.Empty;
            SecurityId = new SecretKey(String.Empty);

            DataApiUrl = Environments.Live.AlpacaDataApi;
            TradingApiUrl = Environments.Live.AlpacaTradingApi;
            PolygonApiUrl = Environments.Live.PolygonDataApi;

            DataApiVersion = AlpacaDataClientConfiguration.DefaultApiVersion;
            TradingApiVersion = AlpacaTradingClientConfiguration.DefaultApiVersion;

            ThrottleParameters = ThrottleParameters.Default;
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// Security identifier for API authentication.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca trading REST API base URL.
        /// </summary>
        public Uri TradingApiUrl { get; set; }

        /// <summary>
        /// Gets or sets Alpaca data REST API base URL.
        /// </summary>
        public Uri DataApiUrl { get; set; }

        /// <summary>
        /// Gets or sets Polygon.io REST API base URL.
        /// </summary>
        public Uri PolygonApiUrl { get; set; }

        /// <summary>
        /// Gets or sets Alpaca Trading API version.
        /// </summary>
        public ApiVersion TradingApiVersion { get; set; }

        /// <summary>
        /// Gets or sets Alpaca data REST API version.
        /// </summary>
        public ApiVersion DataApiVersion { get; set; }

        /// <summary>
        /// Gets or sets REST API throttling parameters.
        /// </summary>
        public ThrottleParameters ThrottleParameters { get; set; }

        internal AlpacaDataClientConfiguration AlpacaDataClientConfiguration =>
            new AlpacaDataClientConfiguration
            {
                KeyId = KeyId,
                ApiEndpoint = DataApiUrl
            };

        internal AlpacaTradingClientConfiguration AlpacaTradingClientConfiguration =>
            new AlpacaTradingClientConfiguration
            {
                KeyId = KeyId,
                SecurityId = SecurityId,
                ApiEndpoint = TradingApiUrl
            };

        internal PolygonDataClientConfiguration PolygonDataClientConfiguration =>
            new PolygonDataClientConfiguration
            {
                KeyId = KeyId,
                ApiEndpoint = PolygonApiUrl
            };
    }
}
