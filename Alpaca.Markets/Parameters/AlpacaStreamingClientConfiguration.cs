using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaStreamingClient"/> class.
    /// </summary>
    public sealed class AlpacaStreamingClientConfiguration : StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaStreamingClientConfiguration"/> class.
        /// </summary>
        public AlpacaStreamingClientConfiguration()
            : base(Environments.Live.AlpacaStreamingApi)
        {
            SecretKey = String.Empty;
            OAuthToken = String.Empty;
        }

        /// <summary>
        /// Gets or sets Alpaca secret key identifier.
        /// </summary>
        public String SecretKey { get; set; }

        /// <summary>
        /// Gets or sets Alpaca oauth token identifier.
        /// </summary>
        public String OAuthToken { get; set; }

        internal override void EnsureIsValid()
        {
            base.EnsureIsValid();

            if (String.IsNullOrEmpty(OAuthToken) && String.IsNullOrEmpty(SecretKey))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecretKey)}' property shouldn't be null or empty.");
            }
        }
    }
}
