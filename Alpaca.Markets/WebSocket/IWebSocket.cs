using System;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulate logic required for communicating with web socket server from API.
    /// </summary>
    public interface IWebSocket : IDisposable
    {
        /// <summary>
        /// Opens web socket communication channel. Connection state changes will be reported
        /// using <see cref="Opened"/> event and errors - using <see cref="Error"/> event.
        /// </summary>
        /// <returns>Connection opening task for awaiting (if needed).</returns>
        Task OpenAsync();

        /// <summary>
        /// Closes web socket communication channel. Connection state changes will be reported
        /// using <see cref="Closed"/> event and errors - using <see cref="Error"/> event.
        /// </summary>
        /// <returns>Connection closing task for awaiting (if needed).</returns>
        Task CloseAsync();

        /// <summary>
        /// Sends text message into opened web socket connection.
        /// </summary>
        /// <param name="message"></param>
        void Send(
            String message);

        /// <summary>
        /// Occurred after successful web socket connection (at protocol level).
        /// </summary>
        event Action Opened;

        /// <summary>
        /// Occurred after successful web socket disconnection (at protocol level).
        /// </summary>
        event Action Closed;

        /// <summary>
        /// Occurred on each new completed web socket message receiving data or text.
        /// </summary>
        event Action<Byte[]> DataReceived;

        /// <summary>
        /// Occurred on each new completed web socket message receiving text.
        /// </summary>
        event Action<String> MessageReceived;

        /// <summary>
        /// Occurred in case of any communication errors (on opening/close/listening/send).
        /// </summary>
        event Action<Exception> Error;
    }
}
