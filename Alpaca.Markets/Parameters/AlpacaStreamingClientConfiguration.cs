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
        }

        /// <summary>
        /// Gets or sets Alpaca secret key identifier.
        /// </summary>
        public String SecretKey { get; set; }

        internal override void EnsureIsValid()
        {
            base.EnsureIsValid();

            if (String.IsNullOrEmpty(SecretKey))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecretKey)}' property shouldn't be null or empty.");
            }
        }
    }
}
