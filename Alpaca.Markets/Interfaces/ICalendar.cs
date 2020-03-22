using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates single trading day information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ICalendar
    {
        /// <summary>
        /// Gets trading date (in UTC time zone).
        /// </summary>
        DateTime TradingDate { get; }

        /// <summary>
        /// Gets trading date open time (in UTC time zone).
        /// </summary>
        DateTime TradingOpenTime { get; }

        /// <summary>
        /// Gets trading date close time (in UTC time zone).
        /// </summary>
        DateTime TradingCloseTime { get; }
    }
}
