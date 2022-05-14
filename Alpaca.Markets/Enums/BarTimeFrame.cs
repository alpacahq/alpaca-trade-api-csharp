using System.Globalization;

namespace Alpaca.Markets;

/// <summary>
/// Supported bar duration for Alpaca Data API.
/// </summary>
public readonly record struct BarTimeFrame
{
    /// <summary>
    /// Creates new instance of <see cref="BarTimeFrame"/> object.
    /// </summary>
    /// <param name="value">Duration value in units.</param>
    /// <param name="unit">Duration units (minutes, hours, days)</param>
    [UsedImplicitly]
    public BarTimeFrame(
        Int32 value,
        BarTimeFrameUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    /// <summary>
    /// Gets specified duration units.
    /// </summary>
    [UsedImplicitly]
    public BarTimeFrameUnit Unit { get; }

    /// <summary>
    /// Gets specified duration value.
    /// </summary>
    [UsedImplicitly]
    public Int32 Value { get; }

    /// <inheritdoc />
    public override String ToString() =>
        $"{Value.ToString("D", CultureInfo.InvariantCulture)}{Unit.ToEnumString()}";

    /// <summary>
    /// Minute bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Minute => new(1, BarTimeFrameUnit.Minute);

    /// <summary>
    /// Hour bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Hour => new(1, BarTimeFrameUnit.Hour);

    /// <summary>
    /// Daily bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Day => new(1, BarTimeFrameUnit.Day);

    /// <summary>
    /// Weekly bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Week => new(1, BarTimeFrameUnit.Week);

    /// <summary>
    /// Monthly bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Month => new(1, BarTimeFrameUnit.Month);

    /// <summary>
    /// Quarterly (3 months) bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Quarter => new(3, BarTimeFrameUnit.Month);

    /// <summary>
    /// Half year (6 month) bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame HalfYear => new(6, BarTimeFrameUnit.Month);

    /// <summary>
    /// Year (12 months) bars.
    /// </summary>
    [UsedImplicitly]
    public static BarTimeFrame Year => new(12, BarTimeFrameUnit.Month);
}
