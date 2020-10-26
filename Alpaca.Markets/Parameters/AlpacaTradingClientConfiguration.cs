using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaTradingClient"/> class.
    /// </summary>
    public sealed class AlpacaTradingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaTradingClientConfiguration"/> class.
        /// </summary>
        public AlpacaTradingClientConfiguration()
        {
            SecurityId = new SecretKey(String.Empty, String.Empty);
            ApiEndpoint = Environments.Live.AlpacaTradingApi;
            ThrottleParameters = ThrottleParameters.Default;
        }

        /// <summary>
        /// Security identifier for API authentication.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca Trading API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets REST API throttling parameters.
        /// </summary>
        public ThrottleParameters ThrottleParameters { get; set; }

        /// <summary>
        /// Gets or sets <see cref="HttpClient"/> instance for connecting.
        /// </summary>
        public HttpClient? HttpClient { get; [UsedImplicitly] set; }

        internal void EnsureIsValid()
        {
            if (SecurityId == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecurityId)}' property shouldn't be null.");
            }

            if (ApiEndpoint == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }

            if (ThrottleParameters == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ThrottleParameters)}' property shouldn't be null.");
            }
        }
    }
}
