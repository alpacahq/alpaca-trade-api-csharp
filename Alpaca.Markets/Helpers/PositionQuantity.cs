using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Represents the position liquidation quantity in (possible fractional) number of shares or in percents.
    /// </summary>
    public readonly struct PositionQuantity : IEquatable<PositionQuantity>
    {
        private PositionQuantity(
            Decimal value,
            Boolean isInShares)
        {
            if (!isInShares &&
                value is <= 0 or > 100)
            {
                throw new ArgumentException(
                    "The percentage value should be between 0 and 100", nameof(value));
            }

            Value = value;
            IsInShares = isInShares;
        }

        /// <summary>
        /// Gets the position liquidation quantity in shares or in percentage value.
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public Decimal Value { get; }

        /// <summary>
        /// Returns <c>true</c> if <see cref="Value"/> is a number of shares (fractional or integer).
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public Boolean IsInShares { get; }

        /// <summary>
        /// Returns <c>true</c> if <see cref="Value"/> is an amount in percents (from 0 to 100).
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public Boolean IsInPercents => !IsInShares;

        /// <summary>
        /// Creates new instance of the <see cref="PositionQuantity"/> object
        /// initialized with <paramref name="value"/> as number of shares.
        /// </summary>
        /// <param name="value">Amount of dollars to buy or sell.</param>
        /// <returns>Initialized <see cref="PositionQuantity"/> object.</returns>
        [UsedImplicitly]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static PositionQuantity InShares(
            Decimal value) => new (value, true);

        /// <summary>
        /// Creates new instance of the <see cref="PositionQuantity"/> object
        /// initialized with <paramref name="value"/> as percentage value.
        /// </summary>
        /// <param name="value">Number of shares (integer or fractional).</param>
        /// <returns>Initialized <see cref="PositionQuantity"/> object.</returns>
        [UsedImplicitly]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static PositionQuantity InPercents(
            Decimal value) => new (value, false);

        internal Decimal? AsPercentage() => IsInPercents ? Value : null;

        internal Decimal? AsFractional() => IsInShares ? Value : null;

        /// <inheritdoc />
        public override Boolean Equals(
            Object obj) =>
            obj is PositionQuantity positionQuantity &&
            positionQuantity.Equals(this);

        /// <inheritdoc />
        public override Int32 GetHashCode() => 
            Value.GetHashCode();

        /// <inheritdoc />
        public Boolean Equals(
            PositionQuantity other) =>
            IsInShares == other.IsInShares &&
            Decimal.Equals(Value, other.Value);
        
        /// <summary>
        /// Returns <c>true</c> if <paramref name="lhs"/> are equal to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if both objects are equal.</returns>
        public static Boolean operator ==(
            PositionQuantity lhs, 
            PositionQuantity rhs) =>
            lhs.Equals(rhs);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="lhs"/> are not equal to <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">Left hand side object.</param>
        /// <param name="rhs">Right hand side object.</param>
        /// <returns>True if both objects are not equal.</returns>
        public static Boolean operator !=(
            PositionQuantity lhs, 
            PositionQuantity rhs) => 
            !(lhs == rhs);
     }
}
