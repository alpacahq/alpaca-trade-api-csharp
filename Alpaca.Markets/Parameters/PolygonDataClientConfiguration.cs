using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class PolygonDataClientConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public PolygonDataClientConfiguration()
        {
            KeyId = String.Empty;
            ApiEndpoint = Environments.Live.AlpacaDataApi;
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca data API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsStaging { get; set; }

        internal void EnsureIsValid()
        {
            if (String.IsNullOrEmpty(KeyId))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(KeyId)}' property shouldn't be null or empty.");
            }

            if (ApiEndpoint == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }
        }
    }
}