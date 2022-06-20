namespace Alpaca.Markets;

/// <summary>
/// Represents the position liquidation quantity in (possible fractional) number of shares or in percents.
/// </summary>
public readonly record struct PositionQuantity
{
    /// <summary>
    /// Creates new instance of the <see cref="PositionQuantity"/> structure.
    /// </summary>
    public PositionQuantity()
    {
        Value = 0M;
        IsInShares = false;
    }

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

        Value = value; //-V3142
        IsInShares = isInShares;
    }

    /// <summary>
    /// Gets the position liquidation quantity in shares or in percentage value.
    /// </summary>
    [UsedImplicitly]
    public Decimal Value { get; }

    /// <summary>
    /// Returns <c>true</c> if <see cref="Value"/> is a number of shares (fractional or integer).
    /// </summary>
    [UsedImplicitly]
    public Boolean IsInShares { get; }

    /// <summary>
    /// Returns <c>true</c> if <see cref="Value"/> is an amount in percents (from 0 to 100).
    /// </summary>
    [UsedImplicitly]
    public Boolean IsInPercents => !IsInShares;

    /// <summary>
    /// Creates new instance of the <see cref="PositionQuantity"/> object
    /// initialized with <paramref name="value"/> as number of shares.
    /// </summary>
    /// <param name="value">Amount of dollars to buy or sell.</param>
    /// <returns>Initialized <see cref="PositionQuantity"/> object.</returns>
    [UsedImplicitly]
    public static PositionQuantity InShares(
        Decimal value) => new(value, true);

    /// <summary>
    /// Creates new instance of the <see cref="PositionQuantity"/> object
    /// initialized with <paramref name="value"/> as percentage value.
    /// </summary>
    /// <param name="value">Number of shares (integer or fractional).</param>
    /// <exception cref="ArgumentException">
    /// The <paramref name="value"/> argument is less than 0 or greater than 100.
    /// </exception>
    /// <returns>Initialized <see cref="PositionQuantity"/> object.</returns>
    [UsedImplicitly]
    public static PositionQuantity InPercents(
        Decimal value) => new(value, false);

    internal Decimal? AsPercentage() => IsInPercents ? Value : null;

    internal Decimal? AsFractional() => IsInShares ? Value : null;
}
