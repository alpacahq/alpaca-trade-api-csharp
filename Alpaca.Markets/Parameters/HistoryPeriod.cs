using System.Globalization;

namespace Alpaca.Markets;

/// <summary>
/// Encapsulates account history period request duration - value and unit pair.
/// </summary>
public readonly record struct HistoryPeriod
{
    /// <summary>
    /// Creates new instance of the <see cref="HistoryPeriod"/> structure.
    /// </summary>
    public HistoryPeriod()
    {
        Value = 0;
        Unit = HistoryPeriodUnit.Day;
    }

    /// <summary>
    /// Creates new instance of <see cref="HistoryPeriod"/> object.
    /// </summary>
    /// <param name="value">Duration value in units.</param>
    /// <param name="unit">Duration units (days, weeks, etc.)</param>
    [UsedImplicitly]
    public HistoryPeriod(
        Int32 value,
        HistoryPeriodUnit unit)
    {
        Value = value;
        Unit = unit;
    }

    /// <summary>
    /// Gets specified duration units.
    /// </summary>
    [UsedImplicitly]
    public HistoryPeriodUnit Unit { get; }

    /// <summary>
    /// Gets specified duration value.
    /// </summary>
    [UsedImplicitly]
    public Int32 Value { get; }

    /// <inheritdoc />
    public override String ToString() =>
        $"{Value.ToString("D", CultureInfo.InvariantCulture)}{Unit.ToEnumString()}";
}
