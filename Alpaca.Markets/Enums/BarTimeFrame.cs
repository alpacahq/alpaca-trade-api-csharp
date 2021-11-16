using System.Globalization;

namespace Alpaca.Markets;

/// <summary>
/// Supported bar duration for Alpaca Data API.
/// </summary>
public readonly struct BarTimeFrame : IEquatable<BarTimeFrame>
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
    public bool Equals(BarTimeFrame other) => Unit == other.Unit && Value == other.Value;

    /// <inheritdoc />
    public override String ToString() =>
        $"{Value.ToString("D", CultureInfo.InvariantCulture)}{Unit.ToEnumString()}";

    /// <inheritdoc />
    public override Boolean Equals(Object? obj) => obj is BarTimeFrame period && Equals(period);

    /// <inheritdoc />
    public override Int32 GetHashCode()
    {
        var hashCode = -2109781847;
        hashCode = hashCode * -1521134295 + Unit.GetHashCode();
        hashCode = hashCode * -1521134295 + Value.GetHashCode();
        return hashCode;
    }

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
    /// Returns <c>true</c> if compared objects are equal.
    /// </summary>
    /// <param name="lhs">Left hand side for compare./</param>
    /// <param name="rhs">Right hand side for compare.</param>
    /// <returns>Returns <c>true</c> if compared objects are equal.</returns>
    public static Boolean operator ==(BarTimeFrame lhs, BarTimeFrame rhs) => lhs.Equals(rhs);

    /// <summary>
    /// Returns <c>true</c> if compared objects are not equal.
    /// </summary>
    /// <param name="lhs">Left hand side for compare./</param>
    /// <param name="rhs">Right hand side for compare.</param>
    /// <returns>Returns <c>true</c> if compared objects are not equal.</returns>
    public static Boolean operator !=(BarTimeFrame lhs, BarTimeFrame rhs) => !(lhs == rhs);
}
