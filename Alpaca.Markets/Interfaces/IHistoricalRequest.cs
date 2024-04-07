namespace Alpaca.Markets;

// TODO: olegra - rename this interface into IRequestWithPagination

/// <summary>
/// Provides unified type-safe access to the historical data request parameters.
/// </summary>
public interface IHistoricalRequest
{
    /// <summary>
    /// Gets the pagination parameters for the request (page size and token).
    /// </summary>
    Pagination Pagination { get; }
}

/// <summary>
/// Provides unified type-safe access to the historical data request parameters.
/// </summary>
/// <typeparam name="TRequest">Historical data request type.</typeparam>
/// <typeparam name="TItem">Historical response data item type.</typeparam>
// ReSharper disable once UnusedTypeParameter
public interface IHistoricalRequest<out TRequest, TItem> : IHistoricalRequest
{
    /// <summary>
    /// Gets copy of the current request without page token and max allowed page size.
    /// </summary>
    [UsedImplicitly]
    [SuppressMessage("Design",
        "CA1024:Use properties where appropriate", Justification = "Factory method")]
    TRequest GetValidatedRequestWithoutPageToken();
}
