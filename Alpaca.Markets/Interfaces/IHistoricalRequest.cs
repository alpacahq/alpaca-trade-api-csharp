namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access to the historical data request parameters.
    /// </summary>
    /// <typeparam name="TRequest">Historical data request type.</typeparam>
    /// <typeparam name="TItem">Historical response data item type.</typeparam>
    // ReSharper disable once UnusedTypeParameter
    public interface IHistoricalRequest<out TRequest, TItem>
    {
        /// <summary>
        /// Gets copy of the current request without page token and max allowed page size.
        /// </summary>
        TRequest GetValidatedRequestWithoutPageToken();
    }
}
