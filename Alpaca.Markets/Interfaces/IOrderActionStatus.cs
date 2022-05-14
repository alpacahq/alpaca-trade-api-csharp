namespace Alpaca.Markets;

/// <summary>
/// Encapsulates order action status information from Alpaca REST API.
/// </summary>
public interface IOrderActionStatus
{
    /// <summary>
    /// Gets unique server-side order identifier.
    /// </summary>
    [UsedImplicitly]
    Guid OrderId { get; }

    /// <summary>
    /// Returns <c>true</c> if requested action completed successfully.
    /// </summary>
    [UsedImplicitly]
    Boolean IsSuccess { get; }
}
