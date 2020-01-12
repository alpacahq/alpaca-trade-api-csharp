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
        // TODO: olegra - use built-in HttpMethod.Patch property in .NET Standard 2.1
        private static readonly HttpMethod _httpMethodPatch = new HttpMethod("PATCH");

        private readonly HttpClient _alpacaHttpClient = new HttpClient();

        private readonly AlpacaDataClient _alpacaDataClient;

        private readonly HttpClient _polygonHttpClient = new HttpClient();

        private readonly RestClientConfiguration _configuration;

        private readonly IThrottler _alpacaRestApiThrottler;

        private readonly Boolean _isPolygonStaging;

        /// <summary>
        /// Creates new instance of <see cref="RestClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public RestClient(
            RestClientConfiguration configuration)
        {
            _configuration = configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _alpacaRestApiThrottler = configuration.ThrottleParameters.GetThrottler();
            
            configuration.SecurityId
                .AddAuthenticationHeader(_alpacaHttpClient, configuration.KeyId);

            _alpacaHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _alpacaHttpClient.BaseAddress = addApiVersionNumberSafe(
                configuration.TradingApiUrl, configuration.TradingApiVersion);

            _alpacaDataClient = new AlpacaDataClient(configuration.AlpacaDataClientConfiguration);

            _polygonHttpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _polygonHttpClient.BaseAddress = configuration.PolygonApiUrl;
            _isPolygonStaging =
#if NETSTANDARD2_1
                _alpacaHttpClient.BaseAddress.Host.Contains("staging", StringComparison.Ordinal);
#else
                _alpacaHttpClient.BaseAddress.Host.Contains("staging");
#endif

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

        private static async Task<TApi> callAndDeserializeSingleObjectAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            Uri endpointUri,
            CancellationToken cancellationToken,
            HttpMethod? method = null)
            where TJson : TApi
        {
            var exceptions = new Queue<Exception>();

            for(var attempts = 0; attempts < throttler.MaxRetryAttempts; ++attempts)
            {
                await throttler.WaitToProceed(cancellationToken).ConfigureAwait(false);
                try
                {
                    using var request = new HttpRequestMessage(method ?? HttpMethod.Get, endpointUri);
                    using var response = await httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false);

                    // Check response for server and caller specified waits and retries
                    if (!throttler.CheckHttpResponse(response))
                    {
                        continue;
                    }

                    return await deserializeAsync<TApi, TJson>(response).ConfigureAwait(false);
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
#if NETSTANDARD2_1
            await using var stream = await response.Content.ReadAsStreamAsync()
#else
            using var stream = await response.Content.ReadAsStreamAsync()
#endif
                .ConfigureAwait(false);
            using var reader = new JsonTextReader(new StreamReader(stream));

            var serializer = new JsonSerializer();
            if (response.IsSuccessStatusCode)
            {
                return serializer.Deserialize<TJson>(reader);
            }

            try
            {
                throw new RestClientErrorException(
                    serializer.Deserialize<JsonError>(reader) ?? new JsonError());
            }
            catch (Exception exception)
            {
                throw new RestClientErrorException(response, exception);
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

        private async Task<IReadOnlyList<TApi>> getObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                httpClient, throttler, uriBuilder.Uri, cancellationToken)
                .ConfigureAwait(false);

        private async Task<IReadOnlyList<TApi>> deleteObjectsListAsync<TApi, TJson>(
            HttpClient httpClient,
            IThrottler throttler,
            UriBuilder uriBuilder,
            CancellationToken cancellationToken)
            where TJson : TApi =>
            (IReadOnlyList<TApi>) await callAndDeserializeSingleObjectAsync<IReadOnlyList<TJson>, List<TJson>>(
                    httpClient, throttler, uriBuilder.Uri, cancellationToken, HttpMethod.Delete)
                .ConfigureAwait(false);

        private static Uri addApiVersionNumberSafe(Uri baseUri, ApiVersion apiVersion)
        {
            var builder = new UriBuilder(baseUri);

            if (builder.Path.Equals("/", StringComparison.Ordinal))
            {
                builder.Path = $"{apiVersion.ToEnumString()}/";
            }
            if (!builder.Path.EndsWith("/", StringComparison.Ordinal))
            {
                builder.Path += "/";
            }

            return builder.Uri;
        }
    }
}
