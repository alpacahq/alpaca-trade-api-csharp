using System;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// </summary>
    public interface IWebSocket : IDisposable
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task OpenAsync();

        /// <summary>
        /// </summary>
        /// <returns></returns>
        Task CloseAsync();

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        void Send(String message);

        /// <summary>
        /// </summary>
        event Action Opened;

        /// <summary>
        /// </summary>
        event Action Closed;

        /// <summary>
        /// </summary>
        event Action<Byte[]> DataReceived;

        /// <summary>
        /// </summary>
        event Action<Exception> Error;
    }
}
