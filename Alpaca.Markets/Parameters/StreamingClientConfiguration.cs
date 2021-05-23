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
            ApiEndpoint = apiEndpoint;
        }

        /// <summary>
        /// Gets or sets Alpaca streaming API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        internal virtual void EnsureIsValid()
        {
            if (ApiEndpoint == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(ApiEndpoint)}' property shouldn't be null.");
            }
        }
    }
}
