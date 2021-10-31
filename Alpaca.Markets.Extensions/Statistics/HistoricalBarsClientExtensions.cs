using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IHistoricalBarsClient{TRequest}"/> interface.
    /// </summary>
    public static partial class HistoricalBarsClientExtensions
    {
        /// <summary>
        /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="window">Size of the moving average window.</param>
        /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request,
            Int32 window)
            where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
            GetSimpleMovingAverageAsync(
                client, request, window, CancellationToken.None);

        /// <summary>
        /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IHistoricalBarsClient{TRequest}"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="window">Size of the moving average window.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync<TRequest>(
            this IHistoricalBarsClient<TRequest> client,
            TRequest request,
            Int32 window,
            CancellationToken cancellationToken)
            where TRequest : HistoricalRequestBase, IHistoricalRequest<TRequest, IBar> =>
            client.GetHistoricalBarsAsAsyncEnumerable(request, cancellationToken)
                .GetSimpleMovingAverageAsync(window, cancellationToken);
    }
}