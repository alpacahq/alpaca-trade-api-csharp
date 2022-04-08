namespace Alpaca.Markets;

/// <summary>
/// Represents the trailing stop order offset in dollars or as percent of HWM (High Water Mark).
/// </summary>
public readonly record struct TrailOffset
{
    /// <summary>
    /// Creates new instance of the <see cref="TrailOffset"/> structure.
    /// </summary>
    public TrailOffset()
    {
        Value = 0M;
        IsInDollars = false;
    }

    private TrailOffset(
        Decimal value,
        Boolean isInDollars)
    {
        Value = value;
        IsInDollars = isInDollars;
    }

    /// <summary>
    /// Gets the trailing stop order price offset value.
    /// </summary>
    public Decimal Value { get; }

    /// <summary>
    /// Returns <c>true</c> if trail offset is an amount in dollars.
    /// </summary>
    public Boolean IsInDollars { get; }

    /// <summary>
    /// Returns <c>true</c> if trail offset is a percentage of HWM value.
    /// </summary>
    [UsedImplicitly]
    public Boolean IsInPercent => !IsInDollars;

    /// <summary>
    /// Creates new instance of the <see cref="TrailOffset"/> object
    /// initialized with <paramref name="value"/> as dollars amount.
    /// </summary>
    /// <param name="value">Trailing stop order offset in dollars.</param>
    /// <returns>Initialized <see cref="TrailOffset"/> object.</returns>
    [UsedImplicitly]
    public static TrailOffset InDollars(
        Decimal value) =>
        new(value, true);

    /// <summary>
    /// Creates new instance of the <see cref="TrailOffset"/> object
    /// initialized with <paramref name="value"/> as percent of HWM.
    /// </summary>
    /// <param name="value">Trailing stop order offset in percents.</param>
    /// <returns>Initialized <see cref="TrailOffset"/> object.</returns>
    [UsedImplicitly]
    public static TrailOffset InPercent(
        Decimal value) =>
        new(value, false);
}
