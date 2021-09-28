using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Set of extension methods for the <see cref="IAlpacaCryptoDataClient"/> interface.
    /// </summary>
    public static partial class AlpacaCryptoDataClientExtensions
    {
        /// <summary>
        /// Gets the average trade volume for the given <paramref name="symbol"/> and time interval
        /// between <paramref name="from"/> date to the <paramref name="into"/> date (inclusive).
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="symbol">Asset name for the data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        // TODO: olegra - good candidate for the DateOnly type usage
        public static Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAlpacaCryptoDataClient client,
            String symbol,
            DateTime from,
            DateTime into) =>
            GetAverageDailyTradeVolumeAsync(
                client, symbol, TimeInterval.GetInclusive(from, into), CancellationToken.None);

        /// <summary>
        /// Gets the average trade volume for the given <paramref name="symbol"/> and time interval
        /// between <paramref name="from"/> date to the <paramref name="into"/> date (inclusive).
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="symbol">Asset name for the data retrieval.</param>
        /// <param name="from">Filter data equal to or after this time.</param>
        /// <param name="into">Filter data equal to or before this time.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAlpacaCryptoDataClient client,
            String symbol,
            DateTime from,
            DateTime into,
            CancellationToken cancellationToken) =>
            GetAverageDailyTradeVolumeAsync(
                client, symbol, TimeInterval.GetInclusive(from, into), cancellationToken);

        /// <summary>
        /// Gets the average trade volume for the given <paramref name="symbol"/> and <paramref name="timeInterval"/>.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="symbol">Asset name for the data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for the ADTV calculation.</param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAlpacaCryptoDataClient client,
            String symbol,
            IInclusiveTimeInterval timeInterval) =>
            GetAverageDailyTradeVolumeAsync(
                client, symbol, timeInterval, CancellationToken.None);

        /// <summary>
        /// Gets the average trade volume for the given <paramref name="symbol"/> and <paramref name="timeInterval"/>.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="symbol">Asset name for the data retrieval.</param>
        /// <param name="timeInterval">Inclusive time interval for the ADTV calculation.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The pair of ADTV value and number of processed day bars.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static Task<(Decimal, UInt32)> GetAverageDailyTradeVolumeAsync(
            this IAlpacaCryptoDataClient client,
            String symbol,
            IInclusiveTimeInterval timeInterval,
            CancellationToken cancellationToken) =>
            client.GetHistoricalBarsAsAsyncEnumerable(
                    new HistoricalCryptoBarsRequest(symbol, BarTimeFrame.Day, timeInterval), cancellationToken)
                .GetAverageDailyTradeVolumeAsync(cancellationToken);

        /// <summary>
        /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="window">Size of the moving average window.</param>
        /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            Int32 window) =>
            GetSimpleMovingAverageAsync(
                client, request, window, CancellationToken.None);

        /// <summary>
        /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
        /// </summary>
        /// <param name="client">Target instance of the <see cref="IAlpacaCryptoDataClient"/> interface.</param>
        /// <param name="request">Original historical minute bars request (with empty next page token).</param>
        /// <param name="window">Size of the moving average window.</param>
        /// <param name="cancellationToken">
        /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
        /// </param>
        /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync(
            this IAlpacaCryptoDataClient client,
            HistoricalCryptoBarsRequest request,
            Int32 window,
            CancellationToken cancellationToken) =>
            client.GetHistoricalBarsAsAsyncEnumerable(request, cancellationToken)
                .GetSimpleMovingAverageAsync(window, cancellationToken);

    }
}