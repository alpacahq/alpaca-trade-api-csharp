using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="RestClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class RestClientConfiguration
    {
        internal const ApiVersion DefaultTradingApiVersionNumber = ApiVersion.V2;

        internal const ApiVersion DefaultDataApiVersionNumber = ApiVersion.V1;

        private static readonly HashSet<ApiVersion> _supportedApiVersions = new HashSet<ApiVersion> { ApiVersion.V1, ApiVersion.V2 };

        private static readonly HashSet<ApiVersion> _supportedDataApiVersions = new HashSet<ApiVersion> { ApiVersion.V1 };

        /// <summary>
        /// Creates new instance of <see cref="RestClientConfiguration"/> class.
        /// </summary>
        public RestClientConfiguration()
        {
            KeyId = String.Empty;
            SecurityId = new SecretKey(String.Empty);

            TradingApiUrl = Environments.Live.AlpacaTradingApi;
            DataApiUrl = Environments.Live.AlpacaDataApi;
            PolygonApiUrl = Environments.Live.PolygonDataApi;

            TradingApiVersion = DefaultTradingApiVersionNumber;
            DataApiVersion = DefaultDataApiVersionNumber;

            ThrottleParameters = ThrottleParameters.Default;
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// 
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

        internal PolygonDataClientConfiguration PolygonDataClientConfiguration =>
            new PolygonDataClientConfiguration
            {
                KeyId = KeyId,
                ApiEndpoint = PolygonApiUrl
            };

        internal RestClientConfiguration EnsureIsValid()
        {
            if (String.IsNullOrEmpty(KeyId))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(KeyId)}' property shouldn't be null or empty.");
            }

            if (SecurityId == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecurityId)}' property shouldn't be null.");
            }

            if (TradingApiUrl == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(TradingApiUrl)}' property shouldn't be null.");
            }

            if (DataApiUrl == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(DataApiUrl)}' property shouldn't be null.");
            }

            if (PolygonApiUrl == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(PolygonApiUrl)}' property shouldn't be null.");
            }

            if (!_supportedApiVersions.Contains(TradingApiVersion))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(TradingApiVersion)}' property is invalid.");
            }

            if (!_supportedDataApiVersions.Contains(DataApiVersion))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(DataApiVersion)}' property is invalid.");
            }

            if (ThrottleParameters == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ThrottleParameters)}' property shouldn't be null.");
            }

            return this;
        }
    }
}
