using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates trade information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IStreamTrade
    {
        /// <summary>
        /// Gets trade identifier.
        /// </summary>
        String TradeId { get; }

        /// <summary>
        /// Gets asset's exchange identifier.
        /// </summary>
        String Exchange { get; }

        /// <summary>
        /// Gets trade price level.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        UInt64 Size { get; }

        /// <summary>
        /// Gets trade timestamp in UTC time zone.
        /// </summary>
        DateTime TimeUtc { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        IReadOnlyList<String> Conditions { get; }
    }
}
