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
            SecurityId = new SecretKey(String.Empty, String.Empty);
        }

        /// <summary>
        /// Gets or sets Alpaca secret key identifier.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        internal override void EnsureIsValid()
        {
            base.EnsureIsValid();

            if (String.IsNullOrEmpty(SecurityId.Value))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecurityId)}' property shouldn't be null or empty.");
            }
        }
    }
}
