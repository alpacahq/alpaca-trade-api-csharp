namespace Alpaca.Markets;

/// <summary>
/// Represents the market day order quantity in dollars or as (possible fractional) number of shares.
/// </summary>
public readonly struct OrderQuantity : IEquatable<OrderQuantity>
{
    /// <summary>
    /// Creates new instance of the <see cref="OrderQuantity"/> structure.
    /// </summary>
    public OrderQuantity()
    {
        Value = 0M;
        IsInDollars = false;
    }

    private OrderQuantity(
        Decimal value,
        Boolean isInDollars)
    {
        Value = value;
        IsInDollars = isInDollars;
    }

    /// <summary>
    /// Gets the market day order quantity or notional value.
    /// </summary>
    public Decimal Value { get; }

    /// <summary>
    /// Returns <c>true</c> if <see cref="Value"/> is an amount in dollars.
    /// </summary>
    [UsedImplicitly]
    public Boolean IsInDollars { get; }

    /// <summary>
    /// Returns <c>true</c> if <see cref="Value"/> is a number of shares (fractional or integer).
    /// </summary>
    [UsedImplicitly]
    public Boolean IsInShares => !IsInDollars;

    /// <summary>
    /// Creates new instance of the <see cref="OrderQuantity"/> object
    /// initialized with <paramref name="value"/> as dollars amount.
    /// </summary>
    /// <param name="value">Amount of dollars to buy or sell.</param>
    /// <returns>Initialized <see cref="OrderQuantity"/> object.</returns>
    public static OrderQuantity Notional(
        Decimal value) => new(value, true);

    /// <summary>
    /// Creates new instance of the <see cref="OrderQuantity"/> object
    /// initialized with <paramref name="value"/> as number of shares.
    /// </summary>
    /// <param name="value">Number of shares (integer or fractional).</param>
    /// <returns>Initialized <see cref="OrderQuantity"/> object.</returns>
    public static OrderQuantity Fractional(
        Decimal value) => new(value, false);

    /// <summary>
    /// Creates new instance of the <see cref="OrderQuantity"/> object
    /// initialized with <paramref name="value"/> as number of shares.
    /// </summary>
    /// <param name="value">Integer number of shares.</param>
    /// <returns>Initialized <see cref="OrderQuantity"/> object.</returns>
    public static implicit operator OrderQuantity(
        Int64 value) => Fractional(value);

    /// <summary>
    /// Creates new instance of the <see cref="OrderQuantity"/> object
    /// initialized with <paramref name="value"/> as number of shares.
    /// </summary>
    /// <param name="value">Integer number of shares.</param>
    /// <returns>Initialized <see cref="OrderQuantity"/> object.</returns>
    [UsedImplicitly]
    public static OrderQuantity FromInt64(
        Int64 value) => Fractional(value);

    internal Decimal? AsNotional() => IsInDollars ? Value : null;

    internal Decimal? AsFractional() => IsInShares ? Value : null;

    /// <inheritdoc />
    public override Boolean Equals(
        Object? obj) =>
        obj is OrderQuantity orderQuantity &&
        orderQuantity.Equals(this);

    /// <inheritdoc />
    public override Int32 GetHashCode() =>
        Value.GetHashCode();

    /// <inheritdoc />
    public Boolean Equals(
        OrderQuantity other) =>
        IsInDollars == other.IsInDollars &&
        Decimal.Equals(Value, other.Value);

    /// <summary>
    /// Returns <c>true</c> if <paramref name="lhs"/> are equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs">Left hand side object.</param>
    /// <param name="rhs">Right hand side object.</param>
    /// <returns>True if both objects are equal.</returns>
    public static Boolean operator ==(
        OrderQuantity lhs,
        OrderQuantity rhs) =>
        lhs.Equals(rhs);

    /// <summary>
    /// Returns <c>true</c> if <paramref name="lhs"/> are not equal to <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs">Left hand side object.</param>
    /// <param name="rhs">Right hand side object.</param>
    /// <returns>True if both objects are not equal.</returns>
    public static Boolean operator !=(
        OrderQuantity lhs,
        OrderQuantity rhs) =>
        !(lhs == rhs);
}
