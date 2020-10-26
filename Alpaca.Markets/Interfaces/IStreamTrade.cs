using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates trade information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStreamTrade : IStreamBase
    {
        /// <summary>
        /// Gets trade identifier.
        /// </summary>
        String TradeId { get; }

        /// <summary>
        /// Gets asset's exchange identifier.
        /// </summary>
        Int64 Exchange { get; }

        /// <summary>
        /// Gets trade price level.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        Int64 Size { get; }

        /// <summary>
        /// Gets trade timestamp in UTC time zone.
        /// </summary>
        DateTime TimeUtc { get; }
    }
}
