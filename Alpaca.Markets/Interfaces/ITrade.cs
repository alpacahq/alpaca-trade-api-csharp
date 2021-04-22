using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic trade information from Alpaca APIs.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface ITrade
    {
        /// <summary>
        /// Gets trade timestamp in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets trade price level.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        UInt64 Size { get; }

        /// <summary>
        /// Gets trade identifier.
        /// </summary>
        UInt64 TradeId { get; }

        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        String Exchange { get; }

        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        String Tape { get; }
    }
}
