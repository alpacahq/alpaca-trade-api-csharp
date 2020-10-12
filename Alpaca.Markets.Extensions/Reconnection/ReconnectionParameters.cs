using System;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Automatic reconnection parameters for streaming client with auto-reconnection support.
    /// </summary>
    public sealed class ReconnectionParameters
    {
        /// <summary>
        /// Gets or sets the maximal number of reconnection attempts in case of connection close.
        /// </summary>
        public Int32 MaxReconnectionAttempts { get; set; } = 5;

        /// <summary>
        /// Gets or sets the minimal delay between different reconnection attempts.
        /// </summary>
        public TimeSpan MinReconnectionDelay { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Gets or sets the maximal delay between different reconnection attempts.
        /// </summary>
        public TimeSpan MaxReconnectionDelay { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Gets the default reconnection parameters - 5 attempts with delay from 1 to 5 seconds.
        /// </summary>
        public static ReconnectionParameters Default => new ReconnectionParameters();
    }
}
