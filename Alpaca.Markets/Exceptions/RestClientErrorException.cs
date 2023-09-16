using System.Net;

namespace Alpaca.Markets;

/// <summary>
/// Represents Alpaca REST and Streaming API specific error information.
/// </summary>
[Serializable]
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
    /// <param name="inner">The exception that is the cause of this exception.</param>
    [ExcludeFromCodeCoverage]
    public RestClientErrorException(
        String message,
        Exception inner)
        : base(message, inner)
    {
    }

    [ExcludeFromCodeCoverage]
    private RestClientErrorException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }

    internal RestClientErrorException(
        HttpResponseMessage message,
        JsonError error)
        : base(error.Message)
    {
        ErrorInformation = error;
        HttpStatusCode = message.StatusCode;
        ErrorCode = error.Code ?? (Int32)HttpStatusCode;
    }

    internal RestClientErrorException(
        JsonStreamError error)
        : base(error.Message) =>
        ErrorCode = error.Code;

    internal RestClientErrorException(
        HttpResponseMessage message)
        : base(message.ReasonPhrase ?? String.Empty) =>
        ErrorCode = (Int32)(HttpStatusCode = message.StatusCode);

    [ExcludeFromCodeCoverage]
    internal RestClientErrorException(
        HttpResponseMessage message,
        Exception exception)
        : base(message.ReasonPhrase ?? String.Empty, exception) =>
        ErrorCode = (Int32)(HttpStatusCode = message.StatusCode);

    /// <summary>
    /// Gets original error code returned by REST API endpoint.
    /// </summary>
    public Int32 ErrorCode { get; }

    /// <summary>
    /// Gets original HTTP status code returned by REST API endpoint.
    /// </summary>
    public HttpStatusCode? HttpStatusCode { get; }

    /// <summary>
    /// Gets extended error information if it provided by server.
    /// </summary>
    public IErrorInformation? ErrorInformation { get; }
}
