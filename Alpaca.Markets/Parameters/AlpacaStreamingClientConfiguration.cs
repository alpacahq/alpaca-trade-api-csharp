using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="SockClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
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
