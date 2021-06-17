using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates the basic trade information from Alpaca APIs.
    /// </summary>
    [CLSCompliant(false)]
    public interface ITrade
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        [UsedImplicitly]
        String Symbol { get; }

        /// <summary>
        /// Gets trade timestamp in UTC time zone.
        /// </summary>
        [UsedImplicitly]
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets trade price level.
        /// </summary>
        [UsedImplicitly]
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        [UsedImplicitly]
        UInt64 Size { get; }

        /// <summary>
        /// Gets trade identifier.
        /// </summary>
        [UsedImplicitly]
        UInt64 TradeId { get; }

        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        [UsedImplicitly]
        String Exchange { get; }

        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        [UsedImplicitly]
        String Tape { get; }

        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        [UsedImplicitly]
        IReadOnlyList<String> Conditions { get; }
    }
}
