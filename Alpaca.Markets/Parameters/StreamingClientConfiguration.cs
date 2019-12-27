﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Configuration parameters object for <see cref="SockClient"/> class.
    /// </summary>
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    public abstract class StreamingClientConfiguration
    {
        /// <summary>
        /// Creates new instance of <see cref="StreamingClientConfiguration"/> class.
        /// </summary>
        protected internal StreamingClientConfiguration(Uri apiEndpoint)
        {
            KeyId = String.Empty;
            ApiEndpoint = apiEndpoint;
            WebSocketFactory = WebSocket4NetFactory.Instance;
        }

        /// <summary>
        /// Gets or sets Alpaca application key identifier.
        /// </summary>
        public String KeyId { get; set; }

        /// <summary>
        /// Gets or sets Alpaca streaming API base URL.
        /// </summary>
        public Uri ApiEndpoint { get; set; }

        /// <summary>
        /// Gets or sets web sockets connection factory.
        /// </summary>
        public IWebSocketFactory WebSocketFactory { get; set; }

        internal IWebSocket CreateWebSocket() => 
            WebSocketFactory.CreateWebSocket(ApiEndpoint);

        internal virtual void EnsureIsValid()
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

            if (WebSocketFactory == null)
            {
                throw new InvalidOperationException(
                    $"The value of '{nameof(WebSocketFactory)}' property shouldn't be null.");
            }
        }
    }
}
