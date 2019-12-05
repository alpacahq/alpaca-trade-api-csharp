using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical trade information from Polygon REST API.
    /// </summary>
    public interface IHistoricalTrade
    {
        /// <summary>
        /// Gets trade source exchange.
        /// </summary>
        [Obsolete("Exchange is deprecated in API v2, use ExchangeId instead", false)]
        String Exchange { get; }

        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        Int64 ExchangeId { get; }

        /// <summary>
        /// Gets trade timestamp.
        /// </summary>
        [Obsolete("TimeOffset is deprecated in API v2, use SipTimestamp instead", false)]
        Int64 TimeOffset  { get; }

        /// <summary>
        /// Gets trade price.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        Int64 Size { get; }

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
        /// Gets trade conditions.
        /// </summary>
        IReadOnlyList<Int64> Conditions { get; }
    }
}
