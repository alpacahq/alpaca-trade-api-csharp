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
        /// Gets tape where trade occured.
        /// </summary>
        Int64 Tape { get; }

        /// <summary>
        /// Gets sequence number of trade.
        /// </summary>
        Int64 SequenceNumber { get; }

        /// <summary>
        /// Gets quote conditions.
        /// </summary>
        IReadOnlyList<Int64> Conditions { get; }
    }
}
