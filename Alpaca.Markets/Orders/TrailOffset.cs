using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Represents the trailing stop order offset in dollars or as percent of HWM.
    /// </summary>
    public readonly struct TrailOffset : IEquatable<TrailOffset>
    {
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
        /// Returns <c>true</c> if trail offset is a percent of HWM value.
        /// </summary>
        public Boolean IsInPercent => !IsInDollars;
        
        /// <summary>
        /// Creates new instance of the <see cref="TrailOffset"/> object
        /// initialized with <paramref name="value"/> as dollars amount.
        /// </summary>
        /// <param name="value">Trailing stop order offset in dollars.</param>
        /// <returns>Initialized <see cref="TrailOffset"/> object.</returns>
        public static TrailOffset InDollars(
            Decimal value) =>
            new TrailOffset(value, true);
        
        /// <summary>
        /// Creates new instance of the <see cref="TrailOffset"/> object
        /// initialized with <paramref name="value"/> as percent of HWM.
        /// </summary>
        /// <param name="value">Trailing stop order offset in percents.</param>
        /// <returns>Initialized <see cref="TrailOffset"/> object.</returns>
        public static TrailOffset InPercent(
            Decimal value) =>
            new TrailOffset(value, false);

        /// <inheritdoc />
        public override Boolean Equals(Object? obj) =>
            obj is TrailOffset trailOffset &&
            trailOffset.Equals(this);

        /// <inheritdoc />
        public override Int32 GetHashCode() => 
            Value.GetHashCode();

        /// <inheritdoc />
        public Boolean Equals(
            TrailOffset other) =>
            IsInDollars == other.IsInDollars &&
            Decimal.Equals(Value, other.Value);
        
        /// <summary>
        /// Returns <c>true</c> if <paramref name="lhs"/> are equal to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if both objects are equal.</returns>
        public static Boolean operator ==(
            TrailOffset lhs, 
            TrailOffset rhs) =>
            lhs.Equals(rhs);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="lhs"/> are not equal to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if both objects are not equal.</returns>
        public static Boolean operator !=(
            TrailOffset lhs, 
            TrailOffset rhs) => 
            !(lhs == rhs);
    }
}
