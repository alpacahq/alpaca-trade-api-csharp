using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Alpaca Crypto Data API via HTTP/REST.
    /// </summary>
    [CLSCompliant(false)]
    public interface IHistoricalQuotesClient<in TRequest>
        where TRequest : IHistoricalRequest<TRequest, IQuote>
    {
        /// <summary>
        /// Gets historical quotes list for single asset from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only list of historical quotes for specified asset (with pagination data).</returns>
        [UsedImplicitly]
        Task<IPage<IQuote>> ListHistoricalQuotesAsync(
            TRequest request,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets historical quotes dictionary for several assets from Alpaca REST API endpoint.
        /// </summary>
        /// <param name="request">Historical quotes request parameters.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Read-only dictionary of historical quotes for specified assets (with pagination data).</returns>
        [UsedImplicitly]
        Task<IMultiPage<IQuote>> GetHistoricalQuotesAsync(
            TRequest request,
            CancellationToken cancellationToken = default);
    }
}
