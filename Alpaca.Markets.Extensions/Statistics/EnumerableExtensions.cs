namespace Alpaca.Markets.Extensions;

/// <summary>
/// Set of extensions methods for obtaining simple statistical functions from bars list.
/// </summary>
public static class EnumerableExtensions
{
    private struct Bar : IBar, IEquatable<Bar>
    {
        public String Symbol { get; private set; }

        public DateTime TimeUtc { get; private set; }

        public Decimal Open { get; private set; }

        public Decimal High { get; private set; }

        public Decimal Low { get; private set; }

        public Decimal Close { get; private set; }

        public Decimal Volume { get; private set; }

        public Decimal Vwap { get; private set; }

        public UInt64 TradeCount { get; private set; }

        public static Bar operator +(Bar lhs, IBar rhs) =>
            new()
            {
                Symbol = rhs.Symbol,
                TimeUtc = rhs.TimeUtc,
                Open = lhs.Open + rhs.Open,
                High = lhs.High + rhs.High,
                Low = lhs.Low + rhs.Low,
                Close = lhs.Close + rhs.Close,
                Volume = lhs.Volume + rhs.Volume,
                Vwap = lhs.Vwap + rhs.Vwap,
                TradeCount = lhs.TradeCount + rhs.TradeCount
            };

        public static Bar operator -(Bar lhs, IBar rhs) =>
            new()
            {
                Symbol = rhs.Symbol,
                TimeUtc = rhs.TimeUtc,
                Open = lhs.Open - rhs.Open,
                High = lhs.High - rhs.High,
                Low = lhs.Low - rhs.Low,
                Close = lhs.Close - rhs.Close,
                Volume = lhs.Volume - rhs.Volume,
                Vwap = lhs.Vwap - rhs.Vwap,
                TradeCount = lhs.TradeCount - rhs.TradeCount
            };

        public static Bar operator /(Bar lhs, Int32 count) =>
            new()
            {
                Symbol = lhs.Symbol,
                TimeUtc = lhs.TimeUtc,
                Open = lhs.Open / count,
                High = lhs.High / count,
                Low = lhs.Low / count,
                Close = lhs.Close / count,
                Volume = lhs.Volume / (UInt64)count,
                Vwap = lhs.Vwap / count,
                TradeCount = lhs.TradeCount / (UInt64)count
            };

        public Boolean Equals(Bar other) =>
            String.Equals(Symbol, other.Symbol, StringComparison.Ordinal) &&
            TimeUtc.Equals(other.TimeUtc) &&
            Open == other.Open &&
            High == other.High &&
            Low == other.Low &&
            Close == other.Close &&
            Volume == other.Volume &&
            Vwap == other.Vwap &&
            TradeCount == other.TradeCount;

        public override Boolean Equals(Object? obj) =>
            obj is Bar other && Equals(other);

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override Int32 GetHashCode()
        {
            unchecked
            {
#if NET5_0_OR_GREATER || NETSTANDARD2_1
                    var hashCode = Symbol.GetHashCode(StringComparison.Ordinal);
#else
                var hashCode = Symbol.GetHashCode();
#endif
                hashCode = (hashCode * 397) ^ TimeUtc.GetHashCode();
                hashCode = (hashCode * 397) ^ Open.GetHashCode();
                hashCode = (hashCode * 397) ^ High.GetHashCode();
                hashCode = (hashCode * 397) ^ Low.GetHashCode();
                hashCode = (hashCode * 397) ^ Close.GetHashCode();
                hashCode = (hashCode * 397) ^ Volume.GetHashCode();
                hashCode = (hashCode * 397) ^ Vwap.GetHashCode();
                hashCode = (hashCode * 397) ^ TradeCount.GetHashCode();
                return hashCode;
            }
        }

        public static Boolean operator ==(Bar left, Bar right) => left.Equals(right);

        public static Boolean operator !=(Bar left, Bar right) => !left.Equals(right);
    }

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

    /// <summary>
    /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
    /// </summary>
    /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
    /// <param name="window">Size of the moving average window.</param>
    /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync(
        this IAsyncEnumerable<IBar> bars,
        Int32 window) =>
        GetSimpleMovingAverageAsync(bars, window, CancellationToken.None);

    /// <summary>
    /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
    /// </summary>
    /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
    /// <param name="window">Size of the moving average window.</param>
    /// <param name="cancellationToken">
    /// A cancellation token that can be used by other objects or threads to receive notice of cancellation.
    /// </param>
    /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
    [CLSCompliant(false)]
    public static async IAsyncEnumerable<IBar> GetSimpleMovingAverageAsync(
        this IAsyncEnumerable<IBar> bars,
        Int32 window,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        if (window < 1)
        {
            yield break;
        }

        var buffer = new Queue<IBar>(window);
        var accumulator = new Bar();

        await foreach (var bar in bars
            .WithCancellation(cancellationToken)
            .ConfigureAwait(false))
        {
            if (buffer.Count != window)
            {
                buffer.Enqueue(bar);
                accumulator += bar;
                continue;
            }

            yield return accumulator / window;

            accumulator -= buffer.Dequeue();
            accumulator += bar;

            buffer.Enqueue(bar);
        }

        yield return accumulator / window;
    }

    /// <summary>
    /// Gets the simple moving average values for the given list of <see cref="IBar"/> objects.
    /// </summary>
    /// <param name="bars">Target list of the <see cref="IBar"/> instances.</param>
    /// <param name="window">Size of the moving average window.</param>
    /// <returns>The list of bars with SMA values for all <see cref="IBar"/> properties.</returns>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public static IEnumerable<IBar> GetSimpleMovingAverage(
        this IEnumerable<IBar> bars,
        Int32 window) =>
        GetSimpleMovingAverageAsync(bars.ToAsyncEnumerable(), window).ToEnumerable();
}
