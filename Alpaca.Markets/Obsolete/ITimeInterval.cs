namespace Alpaca.Markets;

/// <summary>
/// 
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", false)]
public interface ITimeInterval
{
    /// <summary>
    /// 
    /// </summary>
    DateTime? From { get; }

    /// <summary>
    /// 
    /// </summary>
    DateTime? Into { get; }
}

/// <summary>
/// 
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", false)]
public interface IInclusiveTimeInterval : ITimeInterval, IEquatable<IInclusiveTimeInterval> { }

/// <summary>
/// 
/// </summary>
[Obsolete("Use the Interval<T> structure instead of this interface.", false)]
public interface IExclusiveTimeInterval : ITimeInterval, IEquatable<IExclusiveTimeInterval> { }
