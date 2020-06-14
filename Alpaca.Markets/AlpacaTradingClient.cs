using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Trading API via HTTP/REST.
    /// </summary>
    public sealed partial class AlpacaTradingClient : IDisposable
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private readonly IThrottler _alpacaRestApiThrottler;

        /// <summary>
        /// Creates new instance of <see cref="AlpacaTradingClient"/> object.
        /// </summary>
        /// <param name="configuration">Configuration parameters object.</param>
        public AlpacaTradingClient(
            AlpacaTradingClientConfiguration configuration)
        {
            configuration
                .EnsureNotNull(nameof(configuration))
                .EnsureIsValid();

            _alpacaRestApiThrottler = configuration.ThrottleParameters.GetThrottler();

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);

            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.BaseAddress = configuration.ApiEndpoint;
            _httpClient.SetSecurityProtocol();
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();
    }
}
