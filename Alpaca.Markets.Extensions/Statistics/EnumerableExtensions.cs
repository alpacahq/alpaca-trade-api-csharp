using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets the average trade volume for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAsyncEnumerable<IBar> bars) =>
            GetAverageDailyTradeVolumeAsync(bars, CancellationToken.None);

        /// <summary>
        /// Gets the average trade volume for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static async Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAsyncEnumerable<IBar> bars,
            CancellationToken cancellationToken)
        {
            var accumulator = 0M;
            var count = 0U;

            await foreach (var bar in bars.WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                accumulator += bar.Volume;
                ++count;
            }

            return count == 0 ? (0M, 0U) : (accumulator / count, count);
        }

        /// <summary>
        /// Gets the average trade volume for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static (Decimal, UInt32) GetAverageDailyTradeVolume(
            this IEnumerable<IBar> bars)
        {
            var accumulator = 0M;
            var count = 0U;

            foreach (var bar in bars.EnsureNotNull(nameof(bars)))
            {
                accumulator += bar.Volume;
                ++count;
            }

            return count == 0 ? (0M, 0U) : (accumulator / count, count);
        }
    }
}