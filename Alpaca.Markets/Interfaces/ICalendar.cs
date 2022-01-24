namespace Alpaca.Markets;

/// <summary>
/// Encapsulates single trading day information from Alpaca REST API.
/// </summary>
[Obsolete("This interface will be removed in the next major release. Use the IIntervalCalender interface instead.", false)]
public interface ICalendar
{
    /// <summary>
    /// Gets trading date in EST time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingDate() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingDateEst { get; }

    /// <summary>
    /// Gets trading date in UTC time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingDate() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingDateUtc { get; }

    /// <summary>
    /// Gets trading date open time in EST time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingOpenTimeEst() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingOpenTimeEst { get; }

    /// <summary>
    /// Gets trading date open time in UTC time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingOpenTimeUtc() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingOpenTimeUtc { get; }

    /// <summary>
    /// Gets trading date close time in EST time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingCloseTimeEst() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingCloseTimeEst { get; }

    /// <summary>
    /// Gets trading date close time in UTC time zone.
    /// </summary>
    [UsedImplicitly]
    [Obsolete("This property will be removed in the next major release. Use the GetTradingCloseTimeUtc() extension method of the IIntervalCalender interface instead.", false)]
    DateTime TradingCloseTimeUtc { get; }
}
