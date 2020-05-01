using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="AlpacaDataStreamingClient"/> class.
    /// </summary>
    public sealed class AlpacaDataStreamingClientConfiguration : StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="AlpacaDataStreamingClientConfiguration"/> class.
        /// </summary>
        public AlpacaDataStreamingClientConfiguration()
            : base(Environments.Live.PolygonStreamingApi)
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
