using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates last trade information from Alpaca REST API.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ILastTrade
    {
        /// <summary>
        /// Gets request status.
        /// </summary>
        String Status { get; }

        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

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
        UInt64 Size { get; }

        /// <summary>
        /// Gets trade timestamp.
        /// </summary>
        DateTime TimeUtc { get; }
    }
}
