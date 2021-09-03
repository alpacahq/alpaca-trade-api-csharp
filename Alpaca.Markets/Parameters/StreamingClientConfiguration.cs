using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="StreamingClientBase{TConfiguration}"/> class.
    /// </summary>
    public abstract class StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="StreamingClientConfiguration"/> class.
        /// </summary>
        protected internal StreamingClientConfiguration(Uri apiEndpoint)
        {
            SecurityId = new SecretKey(String.Empty, String.Empty);
            ApiEndpoint = apiEndpoint;
        }

        /// <summary>
        /// Gets or sets Alpaca streaming API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }
        
        /// <summary>
        /// Gets or sets Alpaca secret key identifier.
        /// </summary>
        public SecurityKey SecurityId { get; set; }

        internal virtual Uri GetApiEndpoint() => ApiEndpoint;

        internal void EnsureIsValid()
        {
            if (ApiEndpoint is null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }

            if (String.IsNullOrEmpty(SecurityId.Value))
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(SecurityId)}' property shouldn't be null or empty.");
            }
        }
    }
}
