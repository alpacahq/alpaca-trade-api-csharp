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
        [Obsolete("This property will be removed in the next major release. Use the TradingDateUtc property instead.", false)]
        DateTime TradingDate { get; }

        /// <summary>
        /// Gets trading date in EST time zone.
        /// </summary>
        DateTime TradingDateEst { get; }

        /// <summary>
        /// Gets trading date in UTC time zone.
        /// </summary>
        DateTime TradingDateUtc { get; }

        /// <summary>
        /// Gets trading date open time in UTC time zone.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the TradingOpenTimeUtc property instead.", false)]
        DateTime TradingOpenTime { get; }

        /// <summary>
        /// Gets trading date open time in EST time zone.
        /// </summary>
        DateTime TradingOpenTimeEst { get; }

        /// <summary>
        /// Gets trading date open time in UTC time zone.
        /// </summary>
        DateTime TradingOpenTimeUtc { get; }

        /// <summary>
        /// Gets trading date close time (in UTC time zone).
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the TradingCloseTimeUtc property instead.", false)]
        DateTime TradingCloseTime { get; }

        /// <summary>
        /// Gets trading date close time in EST time zone.
        /// </summary>
        DateTime TradingCloseTimeEst { get; }

        /// <summary>
        /// Gets trading date close time in UTC time zone.
        /// </summary>
        DateTime TradingCloseTimeUtc { get; }
    }
}
