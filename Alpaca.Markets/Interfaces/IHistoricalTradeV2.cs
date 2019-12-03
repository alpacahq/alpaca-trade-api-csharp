using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical trade information from Polygon REST API.
    /// </summary>
    public interface IHistoricalTradeV2
    {
        /// <summary>
        /// Gets SIP timestamp
        /// </summary>
        Int64 SipTimestamp { get; }

        /// <summary>
        /// Gets participant/exchange timestamp.
        /// </summary>
        Int64 ParticipantTimestamp { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp.
        /// </summary>
        Int64 TrfTimestamp { get; }

        /// <summary>
        /// Gets trade conditions.
        /// </summary>
        IReadOnlyList<Int64> Conditions { get; }

        /// <summary>
        /// Gets trade source exchange ID.
        /// </summary>
        Int64 Exchange { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        Int64 Size { get; }

        /// <summary>
        /// Gets trade price.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade reporting facility ID.
        /// </summary>
        Int64 TrfId { get; }

        /// <summary>
        /// Gets tape where trade occured.
        /// </summary>
        Int64 Tape { get; }

        /// <summary>
        /// Gets sequence number of trade.
        /// </summary>
        Int64 SequenceNumber { get; }

        /// <summary>
        /// Gets trade ID.
        /// </summary>
        String Id { get; }

        /// <summary>
        /// Gets original trade ID.
        /// </summary>
        String OrigId { get; }

        /// <summary>
        /// Gets trade correction.
        /// </summary>
        Int64 Correction { get; }
    }
}
