namespace Alpaca.Markets;

/// <summary>
/// 
/// </summary>
public interface IRateLimitValues
{
    /// <summary>
    /// 
    /// </summary>
    Int32 Limit { get; }

    /// <summary>
    /// 
    /// </summary>
    Int32 Remaining { get; }

    /// <summary>
    /// 
    /// </summary>
    DateTime ResetTimeUtc { get; }
}
