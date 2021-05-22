using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates last quote information from Alpaca REST API.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ILastQuote
    {
        /// <summary>
        /// Gets quote response status.
        /// </summary>
        String Status { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets quote timestamp in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets identifier of bid source exchange.
        /// </summary>
        Int64 BidExchange { get; }

        /// <summary>
        /// Gets identifier of ask source exchange.
        /// </summary>
        Int64 AskExchange { get; }

        /// <summary>
        /// Gets bid price level.
        /// </summary>
        Decimal BidPrice { get; }

        /// <summary>
        /// Gets ask price level.
        /// </summary>
        Decimal AskPrice { get; }

        /// <summary>
        /// Gets bid quantity.
        /// </summary>
        UInt64 BidSize { get; }

        /// <summary>
        /// Gets ask quantity.
        /// </summary>
        UInt64 AskSize { get; }
    }
}
