﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates basic bar information for Polygon APIs.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IAggBase
    {
        /// <summary>
        /// Gets bar open price.
        /// </summary>
        Decimal Open { get; }

        /// <summary>
        /// Gets bar high price.
        /// </summary>
        Decimal High { get; }

        /// <summary>
        /// Gets bar low price.
        /// </summary>
        Decimal Low { get; }

        /// <summary>
        /// Gets bar close price.
        /// </summary>
        Decimal Close { get; }

        /// <summary>
        /// Gets bar trading volume.
        /// </summary>
        UInt64 Volume { get; }

        /// <summary>
        /// Number of items in aggregate window.
        /// Polygon v2 API only.
        /// </summary>
        [Obsolete("This property will be removed from the next major version of SDK.", false)]
        Int32 ItemsInWindow { get; }
    }
}
