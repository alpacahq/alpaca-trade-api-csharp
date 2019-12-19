using System;
using System.Linq;
using System.Globalization;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API endpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="apiVersion">Version of Alpaca API to call.  Valid values are "1" or "2".</param>
        /// <param name="dataApiVersion">Version of Alpaca data API to call.  The only valid value is currently "1".</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="throttleParameters">Parameters for requests throttling.</param>
        /// <param name="oauthKey">Key for alternative authentication via oauth. keyId and secretKey will be ignored if provided.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public RestClient(
            String keyId,
            String secretKey,
            String? alpacaRestApi = null,
            String? polygonRestApi = null,
            String? alpacaDataApi = null,
            Int32? apiVersion = null,
            Int32? dataApiVersion = null,
            Boolean? isStagingEnvironment = null,
            ThrottleParameters? throttleParameters = null,
            String? oauthKey = null)
            : this(createConfiguration(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? LiveEnvironment.TradingApiUrl.AbsoluteUri),
                new Uri(polygonRestApi ?? LiveEnvironment.PolygonRestApi.AbsoluteUri),
                new Uri(alpacaDataApi ?? LiveEnvironment.DataApiUrl.AbsoluteUri),
                apiVersion ?? RestClientConfiguration.DEFAULT_API_VERSION_NUMBER,
                dataApiVersion ?? RestClientConfiguration.DEFAULT_DATA_API_VERSION_NUMBER,
                throttleParameters ?? ThrottleParameters.Default,
                oauthKey ?? String.Empty))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API endpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="apiVersion">Version of Alpaca API to call.  Valid values are "1" or "2".</param>
        /// <param name="dataApiVersion">Version of Alpaca data API to call.  The only valid value is currently "1".</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="throttleParameters">Parameters for requests throttling.</param>
        /// <param name="oauthKey">Key for alternative authentication via oauth. keyId and secretKey will be ignored if provided.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public RestClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Int32 apiVersion,
            Int32 dataApiVersion,
            Boolean isStagingEnvironment,
            ThrottleParameters throttleParameters,
            String oauthKey)
            : this(createConfiguration(
                keyId, secretKey, alpacaRestApi, polygonRestApi, alpacaDataApi, apiVersion, dataApiVersion, throttleParameters, oauthKey))
        {
        }

#if NETSTANDARD2_0 || NETSTANDARD2_1
        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        [Obsolete("This constructor is deprecated and will be removed in upcoming releases.", false)]
        public RestClient(
            Microsoft.Extensions.Configuration.IConfiguration configuration)
            : this(createConfiguration(configuration))
        {
            System.Diagnostics.Contracts.Contract.Requires(configuration != null);
        }

        private static RestClientConfiguration createConfiguration(
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            return createConfiguration(
                configuration?["keyId"] ?? throw new ArgumentException("Provide 'keyId' configuration parameter.", nameof(configuration)),
                configuration["secretKey"] ?? throw new ArgumentException("Provide 'secretKey' configuration parameter.", nameof(configuration)),
                new Uri(configuration["alpacaRestApi"] ?? LiveEnvironment.TradingApiUrl.AbsoluteUri),
                new Uri(configuration["polygonRestApi"] ?? LiveEnvironment.PolygonRestApi.AbsoluteUri),
                new Uri(configuration["alpacaDataApi"] ?? LiveEnvironment.DataApiUrl.AbsoluteUri),
                toInt32OrNull(configuration["apiVersion"]) ?? RestClientConfiguration.DEFAULT_API_VERSION_NUMBER,
                toInt32OrNull(configuration["dataApiVersion"]) ?? RestClientConfiguration.DEFAULT_DATA_API_VERSION_NUMBER,
                new ThrottleParameters(null, null,
                    toInt32OrNull(configuration["maxRetryAttempts"]),
                    configuration?.GetSection("retryHttpStatuses")
                        .GetChildren().Select(item => Convert.ToInt32(item.Value, CultureInfo.InvariantCulture))),
                String.Empty);
        }
        private static Int32? toInt32OrNull(
            String value) => 
            value != null ? Convert.ToInt32(value, CultureInfo.InvariantCulture) : (Int32?)null;

        private static Boolean? toBooleanOrNull(
            String value) =>
            value != null ? Convert.ToBoolean(value, CultureInfo.InvariantCulture) : (Boolean?)null;
#endif

        private static RestClientConfiguration createConfiguration(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Int32 apiVersion,
            Int32 dataApiVersion,
            ThrottleParameters throttleParameters,
            String oauthKey)
        {
            return new RestClientConfiguration
            {
                KeyId = keyId ?? throw new ArgumentException("Application key id should not be null.", nameof(keyId)),
                SecretKey = secretKey ?? throw new ArgumentException("Application secret key should not be null.", nameof(secretKey)),
                TradingApiUrl = alpacaRestApi ?? LiveEnvironment.TradingApiUrl,
                PolygonApiUrl = polygonRestApi ?? LiveEnvironment.PolygonRestApi,
                DataApiUrl = alpacaDataApi ?? LiveEnvironment.DataApiUrl,
                TradingApiVersion = apiVersion,
                DataApiVersion = dataApiVersion,
                ThrottleParameters = throttleParameters ?? ThrottleParameters.Default,
                OAuthKey = oauthKey ?? throw new ArgumentException("Application OAuth key should not be null.", nameof(oauthKey))
            };

        }

    }
}
