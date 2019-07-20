using System;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Represents Alpaca/Polygon REST API specific error information.
    /// </summary>
    public sealed class RestClientErrorException : Exception
    {
        /// <summary>
        /// Creates new instance of <see cref="RestClientErrorException"/> class.
        /// </summary>
        public RestClientErrorException()
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClientErrorException"/> class with specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RestClientErrorException(
            String message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="RestClientErrorException"/> class with
        /// specified error message and reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The  exception that is the cause of this exception.</param>
        public RestClientErrorException(
            String message,
            Exception inner)
            : base(message, inner)
        {
        }

        internal RestClientErrorException(
            JsonError error)
            : base(error.Message) =>
            ErrorCode = error.Code;

        internal RestClientErrorException(
            HttpResponseMessage message,
            Exception innerException)
            : base(message.ReasonPhrase ?? String.Empty, innerException) =>
            ErrorCode = (Int32)message.StatusCode;

        /// <summary>
        /// Original error code returned by REST API endpoint.
        /// </summary>
        public Int32 ErrorCode { get; }
    }
}
