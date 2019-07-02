using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides way for creating instance of <see cref="IWebSocket"/> interface implementation.
    /// </summary>
    public interface IWebSocketFactory
    {
        /// <summary>
        /// Creates new instance of <see cref="IWebSocket"/> interface implementation.
        /// </summary>
        /// <param name="url">Base URL for underlying web socket connection.</param>
        /// <returns>Instance of class which implements <see cref="IWebSocket"/> interface.</returns>
        IWebSocket CreateWebSocket(
            Uri url);
    }
}
