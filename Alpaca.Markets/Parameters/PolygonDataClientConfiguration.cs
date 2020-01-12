using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="PolygonDataClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public sealed class PolygonDataClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="PolygonDataClientConfiguration"/> class.
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
        /// Gets or sets Polygon Data API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets if <see cref="PolygonDataClient"/> connects for staging environment.
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