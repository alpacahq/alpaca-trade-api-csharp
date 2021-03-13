using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base historical information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IHistoricalBase
    {
        /// <summary>
        /// Gets tape where trade occurred.
        /// </summary>
        Int64 Tape { get; }

        /// <summary>
        /// Gets sequence number of trade.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", false)]
        Int64 SequenceNumber { get; }

        /// <summary>
        /// Gets quote conditions.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", false)]
        IReadOnlyList<Int64> Conditions { get; }
    }
}
