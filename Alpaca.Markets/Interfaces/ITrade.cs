using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic trade information from Alpaca APIs.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ITrade
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

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

        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        IReadOnlyList<String> Conditions { get; }
    }
}
