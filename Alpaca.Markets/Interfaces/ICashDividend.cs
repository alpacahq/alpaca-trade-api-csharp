namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the cash dividend information from Alpaca APIs.
/// </summary>
public interface ICashDividend
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
    /// Gets the foreign flag
    /// </summary>
    public Boolean IsForeign { get; }

    /// <summary>
    /// Gets te special flag
    /// </summary>
    public Boolean IsSpecial { get; }

    /// <summary>
    /// Gets the cash dividend process date
    /// </summary>
    public DateOnly ProcessDate { get; }

    /// <summary>
    /// Gets the cash dividend execution date
    /// </summary>
    public DateOnly ExecutionDate { get; }

    /// <summary>
    /// Gets the cash dividend record date
    /// </summary>
    public DateOnly? RecordDate { get; }

    /// <summary>
    /// Gets the cash dividend payable date
    /// </summary>
    public DateOnly? PayableDate { get; }

    /// <summary>
    /// Gets the cash dividend due bill off date
    /// </summary>
    public DateOnly? DueBillOffDate { get; }

    /// <summary>
    /// Gets the cash dividend due bill on date
    /// </summary>
    public DateOnly? DueBillOnDate { get; }
}
