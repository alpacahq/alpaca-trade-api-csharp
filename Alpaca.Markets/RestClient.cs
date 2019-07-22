using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca REST API and Polygon REST API endpoints.
    /// </summary>
    public sealed partial class RestClient : IDisposable
    {
        private const Int32 DEFAULT_API_VERSION_NUMBER = 2;

        private const Int32 DEFAULT_DATA_API_VERSION_NUMBER = 1;

        private static readonly HashSet<Int32> _supportedApiVersions = new HashSet<Int32> { 1, 2 };

        private static readonly HashSet<Int32> _supportedDataApiVersions = new HashSet<Int32> { 1 };

        private readonly HttpClient _alpacaHttpClient = new HttpClient();

        private readonly HttpClient _alpacaDataClient = new HttpClient();

        private readonly HttpClient _polygonHttpClient = new HttpClient();

        private static IThrottler _alpacaRestApiThrottler;

        private readonly Boolean _isPolygonStaging;

        private readonly String _polygonApiKey;

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
        public RestClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = null,
            String polygonRestApi = null,
            String alpacaDataApi = null,
            Int32? apiVersion = null,
            Int32? dataApiVersion = null,
            Boolean? isStagingEnvironment = null,
            ThrottleParameters throttleParameters = null)
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"),
                new Uri(polygonRestApi ?? "https://api.polygon.io"),
                new Uri(alpacaDataApi ?? "https://data.alpaca.markets"),
                apiVersion ?? DEFAULT_API_VERSION_NUMBER,
                dataApiVersion ?? DEFAULT_DATA_API_VERSION_NUMBER,
                isStagingEnvironment ?? false,
                throttleParameters ?? ThrottleParameters.Default)
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
        /// <param name="apiVersion">Version of Alpaca api to call.  Valid values are "1" or "2".</param>
        /// <param name="dataApiVersion">Version of Alpaca data API to call.  The only valid value is currently "1".</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="throttleParameters">Parameters for requests throttling.</param>
        public RestClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Int32 apiVersion,
            Int32 dataApiVersion,
            Boolean isStagingEnvironment,
            ThrottleParameters throttleParameters)
        {
            keyId = keyId ?? throw new ArgumentException(nameof(keyId));
            secretKey = secretKey ?? throw new ArgumentException(nameof(secretKey));

            if (!_supportedApiVersions.Contains(apiVersion))
                throw new ArgumentException(nameof(apiVersion));
            if (!_supportedDataApiVersions.Contains(dataApiVersion))
                throw new ArgumentException(nameof(dataApiVersion));

            _alpacaRestApiThrottler = throttleParameters.GetThrottler();

            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-KEY-ID", keyId);
            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-SECRET-KEY", secretKey);
            _alpacaHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _alpacaHttpClient.BaseAddress = addApiVersionNumberSafe(
                alpacaRestApi ?? new Uri("https://api.alpaca.markets"), apiVersion);

            _alpacaDataClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _alpacaDataClient.BaseAddress = addApiVersionNumberSafe(
                alpacaDataApi ?? new Uri("https://data.alpaca.markets"), dataApiVersion);

            _polygonApiKey = keyId;
            _polygonHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _polygonHttpClient.BaseAddress =
                polygonRestApi ?? new Uri("https://api.polygon.io");
            _isPolygonStaging = isStagingEnvironment ||
                _alpacaHttpClient.BaseAddress.Host.Contains("staging");

#if NET45
            System.Net.ServicePointManager.SecurityProtocol =
                System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _alpacaHttpClient?.Dispose();
            _alpacaDataClient?.Dispose();
            _polygonHttpClient?.Dispose();
        }

        private async Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            String endpointUri)
            where TJson : TApi
        {
            var exceptions = new Queue<Exception>();

            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed();
                try
                {
                    using (var response = await httpClient.GetAsync(endpointUri, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // Check response for server and caller specified waits and retries
                        if(!throttler.CheckHttpResponse(response))
                        {
                            continue;
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new JsonTextReader(new StreamReader(stream)))
                        {
                            var serializer = new JsonSerializer();
                            if (response.IsSuccessStatusCode)
                            {
                                return serializer.Deserialize<TJson>(reader);
                            }

                            try
                            {
                                throw new RestClientErrorException(
                                    serializer.Deserialize<JsonError>(reader));
                            }
                            catch (Exception exception)
                            {
                                throw new RestClientErrorException(response, exception);
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    exceptions.Enqueue(ex);
                    break;
                }
            }

            throw new AggregateException(exceptions);
        }

        private Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder)
            where TJson : TApi =>
            getSingleObjectAsync<TApi, TJson>(httpClient, throttler, uriBuilder.ToString());

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            String endpointUri)
            where TJson : TApi =>
            (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, throttler, endpointUri);

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder)
            where TJson : TApi =>
            (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, throttler, uriBuilder);

        private static Uri addApiVersionNumberSafe(Uri baseUri, Int32 apiVersion)
        {
            var builder = new UriBuilder(baseUri);

            if (builder.Path.Equals("/", StringComparison.Ordinal))
            {
                builder.Path = $"v{apiVersion}/";
            }
            if (!builder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                builder.Path += "/";
            }

            return builder.Uri;
        }
    }
}
