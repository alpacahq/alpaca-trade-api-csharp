namespace Alpaca.Markets;

/// <summary>
/// Represents exception information for request input data validation errors.
/// </summary>
[Serializable]
public sealed class RequestValidationException : Exception
{
    /// <summary>
    /// Creates new instance of <see cref="RequestValidationException"/> class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public RequestValidationException()
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="RequestValidationException"/> class with specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    [ExcludeFromCodeCoverage]
    public RequestValidationException(
        String message)
        : base(message)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="RequestValidationException"/> class with
    /// specified error message and reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of this exception.</param>
    [ExcludeFromCodeCoverage]
    public RequestValidationException(
        String message,
        Exception inner)
        : base(message, inner)
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="RequestValidationException"/> class with specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="propertyName">The invalid property name.</param>
    public RequestValidationException(
        String message,
        String propertyName)
        : base(message)
    {
        PropertyName = propertyName;
    }

    [ExcludeFromCodeCoverage]
    private RequestValidationException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }

    /// <summary>
    /// Gets the invalid property name that causes this validation exception.
    /// </summary>
    public String PropertyName { get; } = String.Empty;
}
