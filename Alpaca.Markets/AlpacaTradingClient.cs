using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Trading API via HTTP/REST.
    /// </summary>
    [Obsolete("This class will be marked as internal in the next major SDK release.", false)]
    public sealed partial class AlpacaTradingClient : IAlpacaTradingClient
    {
        private readonly HttpClient _httpClient;

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

            _httpClient = configuration.HttpClient ?? new HttpClient();

            _alpacaRestApiThrottler = configuration.ThrottleParameters.GetThrottler();

            _httpClient.AddAuthenticationHeaders(configuration.SecurityId);
            _httpClient.Configure(configuration.ApiEndpoint);
        }

        /// <inheritdoc />
        public void Dispose() => _httpClient.Dispose();
    }
}
