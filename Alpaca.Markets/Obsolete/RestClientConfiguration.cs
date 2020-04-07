using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="RestClient"/> class.
    /// </summary>
    internal sealed class RestClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="RestClientConfiguration"/> class.
        /// </summary>
        public RestClientConfiguration()
        {
            KeyId = String.Empty;
            SecurityId = new SecretKey(String.Empty, String.Empty);

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

        public Boolean IsStagingEnvironment { get; set; }

        internal AlpacaDataClientConfiguration AlpacaDataClientConfiguration =>
            new AlpacaDataClientConfiguration
            {
                SecurityId = SecurityId,
                ApiEndpoint = DataApiUrl,
                ApiVersion = DataApiVersion
            };

        internal AlpacaTradingClientConfiguration AlpacaTradingClientConfiguration =>
            new AlpacaTradingClientConfiguration
            {
                SecurityId = SecurityId,
                ApiEndpoint = TradingApiUrl,
                ApiVersion = DataApiVersion,
                ThrottleParameters = ThrottleParameters
            };

        internal PolygonDataClientConfiguration PolygonDataClientConfiguration =>
            new PolygonDataClientConfiguration
            {
                KeyId = IsStagingEnvironment
                    ? $"{KeyId}-staging"
                    : KeyId,
                ApiEndpoint = PolygonApiUrl
            };
    }
}
