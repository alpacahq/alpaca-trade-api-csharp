using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// </summary>
    public interface IWebSocketFactory
    {
        /// <summary>
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IWebSocket CreateWebSocket(Uri url);
    }
}
