namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the stock dividend information from Alpaca APIs.
/// </summary>
public interface IStockDividend
{
    /// <summary>
    /// Gets the symbol
    /// </summary>
    public String Symbol { get; }

    /// <summary>
    /// Gets the dividend rate
    /// </summary>
    public Decimal Rate { get; }

    /// <summary>
    /// Gets the stock dividend process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the stock dividend execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the stock dividend record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the stock dividend payable date
    /// </summary>
    public DateOnly? PayableDate { get; }
}