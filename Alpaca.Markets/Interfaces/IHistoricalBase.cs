using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base historical information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [Obsolete("This interface will be removed in the next major SDK release.", false)]
    public interface IHistoricalBase
    {
        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        [Obsolete("This property will be moved up in the interface hierarchy in the next major SDK release.", false)]
        Int64 Tape { get; }

        /// <summary>
        /// Gets sequence number of trade.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        Int64 SequenceNumber { get; }

        /// <summary>
        /// Gets quote conditions.
        /// </summary>
        [UsedImplicitly]
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        IReadOnlyList<Int64> Conditions { get; }
    }
}
