namespace Alpaca.Markets;

internal readonly record struct RateLimitValues(
    Int32 Limit,
    Int32 Remaining,
    DateTime ResetTimeUtc)
    : IRateLimitValues;
