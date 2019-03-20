using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
#if NET45
using System.Net;
#endif
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
        private readonly HttpClient _alpacaHttpClient = new HttpClient();

        private readonly HttpClient _alpacaDataClient = new HttpClient();

        private readonly HttpClient _polygonHttpClient = new HttpClient();

        private readonly Boolean _isPolygonStaging;

        private readonly String _polygonApiKey;

        private readonly IEnumerable<Int32> _retryHttpStatuses;

        private readonly String _alpacaApiVersion;

        private static readonly IEnumerable<String> _supportedApiVersions = new[] { "1", "2" };

        private static readonly IThrottler _alpacaRestApiThrottler =
            new RateThrottler(200, 5, TimeSpan.FromMinutes(1));

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API endpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="maxRetryAttempts">Number of times to retry an Http request, if the status code is one of the <paramref name="retryHttpStatuses"/></param>
        /// <param name="retryHttpStatuses">Http status codes that trigger a retry, up to the <paramref name="maxRetryAttempts"/></param>
        /// <param name="apiVersion">Version of Alpaca api to call.  Valid values are "1" or "2".</param>
        public RestClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = null,
            String polygonRestApi = null,
            String alpacaDataApi = null,
            Boolean? isStagingEnvironment = null,
            Int32 maxRetryAttempts = 5,
            IEnumerable<Int32> retryHttpStatuses = null,
            String apiVersion = "1")
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"),
                new Uri(polygonRestApi ?? "https://api.polygon.io"),
                new Uri(alpacaDataApi ?? "https://data.alpaca.markets"),
                isStagingEnvironment ?? false,
                maxRetryAttempts,
                retryHttpStatuses,
                apiVersion)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="keyId">Application key identifier.</param>
        /// <param name="secretKey">Application secret key.</param>
        /// <param name="alpacaRestApi">Alpaca REST API endpoint URL.</param>
        /// <param name="polygonRestApi">Polygon REST API ennpoint URL.</param>
        /// <param name="alpacaDataApi">Alpaca REST data API endpoint URL.</param>
        /// <param name="isStagingEnvironment">If <c>true</c> use staging.</param>
        /// <param name="maxRetryAttempts">Number of times to retry an Http request, if the status code is one of the <paramref name="retryHttpStatuses"/></param>
        /// <param name="retryHttpStatuses">Http status codes that trigger a retry, up to the <paramref name="maxRetryAttempts"/></param>
        /// <param name="apiVersion">Version of Alpaca api to call.  Valid values are "1" or "2".</param>
        public RestClient(
            String keyId,
            String secretKey,
            Uri alpacaRestApi,
            Uri polygonRestApi,
            Uri alpacaDataApi,
            Boolean isStagingEnvironment,
            Int32 maxRetryAttempts,
            IEnumerable<Int32> retryHttpStatuses,
            String apiVersion)
        {
            keyId = keyId ?? throw new ArgumentException(nameof(keyId));
            secretKey = secretKey ?? throw new ArgumentException(nameof(secretKey));
            if (maxRetryAttempts < 1) throw new ArgumentException(nameof(maxRetryAttempts));
            _alpacaApiVersion = apiVersion ?? "1";
            if (!_supportedApiVersions.Contains(apiVersion)) throw new ArgumentException(nameof(apiVersion));

            _alpacaRestApiThrottler.MaxAttempts = maxRetryAttempts;
            _retryHttpStatuses = retryHttpStatuses ?? new Int32[] { };

            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-KEY-ID", keyId);
            _alpacaHttpClient.DefaultRequestHeaders.Add(
                "APCA-API-SECRET-KEY", secretKey);
            _alpacaHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _alpacaHttpClient.BaseAddress =
                alpacaRestApi ?? new Uri("https://api.alpaca.markets");

            _alpacaDataClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _alpacaDataClient.BaseAddress =
                alpacaDataApi ?? new Uri("https://data.alpaca.markets");

            _polygonApiKey = keyId;
            _polygonHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _polygonHttpClient.BaseAddress =
                polygonRestApi ?? new Uri("https://api.polygon.io");
            _isPolygonStaging = isStagingEnvironment ||
                _alpacaHttpClient.BaseAddress.Host.Contains("staging");


#if NET45
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
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

            for(var attempts = 0; attempts < throttler.MaxAttempts; ++attempts)
            {
                throttler.WaitToProceed();
                HttpStatusCode statusCode = HttpStatusCode.OK;
                try
                {
                    using (HttpResponseMessage response = await httpClient.GetAsync(endpointUri, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // Adhere to server reported instructions
                        statusCode = response.StatusCode;
                        if (statusCode != HttpStatusCode.OK)
                        {
                            if (_retryHttpStatuses.Contains((int)statusCode))
                            {
                                // Accomodate Retry-After header
                                var retryAfter = response.Headers.RetryAfter?.Delta?.TotalMilliseconds ?? (response.Headers.RetryAfter?.Date?.LocalDateTime - DateTime.Now)?.TotalMilliseconds;
                                if (retryAfter != null)
                                {
                                    throttler.AllStop((Int32)retryAfter);
                                }
                                // Server unavailable, or Too many requests (429 can happen when this client competes with another client, e.g. mobile app)
                                else if (statusCode == (HttpStatusCode)429 || statusCode == (HttpStatusCode)503)
                                {
                                    // TODO: If server logic fixed to provide Retry-After, this will be dead code to remove
                                    throttler.AllStop(3000);
                                }
                            }
                            response.EnsureSuccessStatusCode();
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var reader = new JsonTextReader(new StreamReader(stream)))
                        {
                            var serializer = new JsonSerializer();
                            return serializer.Deserialize<TJson>(reader);
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    exceptions.Enqueue(ex);
                    if (!_retryHttpStatuses.Contains((int)statusCode))
                        break;
                }
            }

            throw new AggregateException(exceptions);
        }

        private Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder)
            where TJson : TApi
        {
            return getSingleObjectAsync<TApi, TJson>(httpClient, throttler, uriBuilder.ToString());
        }

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            String endpointUri)
            where TJson : TApi
        {
            return (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, throttler, endpointUri);
        }

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder)
            where TJson : TApi
        {
            return (IEnumerable<TApi>) await
                getSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(httpClient, throttler, uriBuilder);
        }
    }
}
