using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical trade information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IHistoricalTrade
    {
        /// <summary>
        /// Gets SIP timestamp in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        String Exchange { get; }

        /// <summary>
        /// Gets trade price.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        UInt64 Size { get; }
        
        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        UInt64 Tape { get; }

        /// <summary>
        /// Gets trade ID.
        /// </summary>
        String? TradeId { get; }

        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        IReadOnlyList<String> Conditions { get; }
    }
}
