namespace Alpaca.Markets;

/// <summary>
/// Encapsulates time interval (from and till date/time points) for filtering requires.
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", true)]
public interface ITimeInterval
{
    /// <summary>
    /// Gets the starting date/time point of filtering interval.
    /// </summary>
    DateTime? From { get; }

    /// <summary>
    /// Gets the ending date/time point of filtering interval.
    /// </summary>
    DateTime? Into { get; }
}

/// <summary>
/// Represents the inclusive version of the <see cref="ITimeInterval"/> interface.
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", true)]
public interface IInclusiveTimeInterval : ITimeInterval, IEquatable<IInclusiveTimeInterval>;

/// <summary>
/// Represents the exclusive version of the <see cref="ITimeInterval"/> interface.
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", true)]
public interface IExclusiveTimeInterval : ITimeInterval, IEquatable<IExclusiveTimeInterval>;
