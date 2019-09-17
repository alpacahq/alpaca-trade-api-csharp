using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca REST API and Polygon REST API endpoints.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed partial class RestClient : IDisposable
    {
        private const Int32 DEFAULT_API_VERSION_NUMBER = 2;

        private const Int32 DEFAULT_DATA_API_VERSION_NUMBER = 1;

        // TODO: olegra - use built-in HttpMethod.Patch property in .NET Standard 2.1
        private static readonly HttpMethod _httpMethodPatch = new HttpMethod("PATCH");

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
        /// <param name="oauthKey">Key for alternative authentication via oauth. keyId and secretKey will be ignored if provided.</param>
        public RestClient(
            String keyId,
            String secretKey,
            String alpacaRestApi = null,
            String polygonRestApi = null,
            String alpacaDataApi = null,
            Int32? apiVersion = null,
            Int32? dataApiVersion = null,
            Boolean? isStagingEnvironment = null,
            ThrottleParameters throttleParameters = null,
            String oauthKey = null)
            : this(
                keyId,
                secretKey,
                new Uri(alpacaRestApi ?? "https://api.alpaca.markets"),
                new Uri(polygonRestApi ?? "https://api.polygon.io"),
                new Uri(alpacaDataApi ?? "https://data.alpaca.markets"),
                apiVersion ?? DEFAULT_API_VERSION_NUMBER,
                dataApiVersion ?? DEFAULT_DATA_API_VERSION_NUMBER,
                isStagingEnvironment ?? false,
                throttleParameters ?? ThrottleParameters.Default,
                oauthKey ?? "")
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
        {
            keyId = keyId ?? throw new ArgumentException(
                        "Application key id should not be null", nameof(keyId));
            secretKey = secretKey ?? throw new ArgumentException(
                            "Application secret key id should not be null", nameof(secretKey));

            if (!_supportedApiVersions.Contains(apiVersion))
            {
                throw new ArgumentException(
                    "Supported REST API versions are '1' and '2' only", nameof(apiVersion));
            }
            if (!_supportedDataApiVersions.Contains(dataApiVersion))
            {
                throw new ArgumentException(
                    "Supported Data REST API versions are '1' and '2' only", nameof(dataApiVersion));
            }

            throttleParameters = throttleParameters ?? ThrottleParameters.Default;
            _alpacaRestApiThrottler = throttleParameters.GetThrottler();

            if (string.IsNullOrEmpty(oauthKey))
            {
                _alpacaHttpClient.DefaultRequestHeaders.Add(
                    "APCA-API-KEY-ID", keyId);
                _alpacaHttpClient.DefaultRequestHeaders.Add(
                    "APCA-API-SECRET-KEY", secretKey);
            }
            else
            {
                _alpacaHttpClient.DefaultRequestHeaders.Add(
                    "Authorization", "Bearer " + oauthKey);
            }
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
#pragma warning disable CA5364 // Do Not Use Deprecated Security Protocols
                System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11;
#pragma warning restore CA5364 // Do Not Use Deprecated Security Protocols
#endif
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _alpacaHttpClient?.Dispose();
            _alpacaDataClient?.Dispose();
            _polygonHttpClient?.Dispose();
        }

        private async Task<TApi> callAndDeserializeSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            Uri endpointUri,
            CancellationToken cancellationToken,
            HttpMethod method = null)
            where TJson : TApi
        {
            var exceptions = new Queue<Exception>();

            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);
                try
                {
                    using (var request = new HttpRequestMessage(method ?? HttpMethod.Get, endpointUri))
                    using (var response = await httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        // Check response for server and caller specified waits and retries
                        if (!throttler.CheckHttpResponse(response))
                        {
                            continue;
                        }

                        return await deserializeAsync<TApi, TJson>(response).ConfigureAwait(false);
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

        private static async Task<TApi> deserializeAsync<TApi, TJson>(
            HttpResponseMessage response)
            where TJson : TApi
        {
            using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
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

        private Task<TApi> getSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            callAndDeserializeSingleObjectAsync<TApi, TJson>(
                httpClient, throttler, uriBuilder.Uri, cancellationToken);

        private async Task<IEnumerable<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IEnumerable<TApi>) await callAndDeserializeSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(
                httpClient, throttler, uriBuilder.Uri, cancellationToken)
                .ConfigureAwait(false);
        private async Task<IEnumerable<TApi>> deleteObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IEnumerable<TApi>) await callAndDeserializeSingleObjectAsync<IEnumerable<TJson>, List<TJson>>(
                    httpClient, throttler, uriBuilder.Uri, cancellationToken, HttpMethod.Delete)
                .ConfigureAwait(false);

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
