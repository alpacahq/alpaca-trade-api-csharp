using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates trade update information from Alpaca streaming API.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ITradeUpdate
    {
        /// <summary>
        /// Gets trade update reason.
        /// </summary>
        [SuppressMessage(
            "Naming", "CA1716:Identifiers should not match keywords",
            Justification = "Already used by clients and creates conflict only in VB.NET")]
        TradeEvent Event { get; }

        /// <summary>
        /// Gets updated trade price level.
        /// </summary>
        Decimal? Price { get; }

        /// <summary>
        /// Gets updated trade quantity (with the fractional part).
        /// </summary>
        Decimal? Quantity { get; }

        /// <summary>
        /// Gets updated trade quantity (rounded to the nearest integer).
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        Int64? IntegerQuantity { get; }

        /// <summary>
        /// Gets update timestamp in UTC time zone.
        /// </summary>
        DateTime? TimestampUtc { get; }

        /// <summary>
        /// Gets related order object.
        /// </summary>
        IOrder Order { get; }
    }
}
