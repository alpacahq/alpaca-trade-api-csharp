using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates last trade information from Polygon REST API.
    /// </summary>
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
        [UsedImplicitly]
        Int64 Exchange { get; }

        /// <summary>
        /// Gets trade price level.
        /// </summary>
        [UsedImplicitly]
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        [UsedImplicitly]
        Int64 Size { get; }

        /// <summary>
        /// Gets trade timestamp.
        /// </summary>
        [UsedImplicitly]
        DateTime TimeUtc { get; }
    }
}
