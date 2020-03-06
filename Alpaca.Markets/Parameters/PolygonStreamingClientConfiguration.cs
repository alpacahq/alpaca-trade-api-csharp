using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="PolygonStreamingClient"/> class.
    /// </summary>
    public sealed class PolygonStreamingClientConfiguration : StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonStreamingClientConfiguration"/> class.
        /// </summary>
        public PolygonStreamingClientConfiguration()
            : base(Environments.Live.PolygonStreamingApi)
        {
            KeyId = String.Empty;
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        internal override void EnsureIsValid()
        {
            base.EnsureIsValid();

            if (String.IsNullOrEmpty(KeyId))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(KeyId)}' property shouldn't be null or empty.");
            }
        }
    }
}
