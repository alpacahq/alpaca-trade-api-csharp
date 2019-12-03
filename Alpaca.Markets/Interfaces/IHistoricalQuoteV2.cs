using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical trade information from Polygon REST API.
    /// </summary>
    public interface IHistoricalQuoteV2
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
        /// Gets quote source exchange ID for the bid.
        /// </summary>
        Int64 BidExchange { get; }

        /// <summary>
        /// Gets quote source exchange ID for the ask.
        /// </summary>
        Int64 AskExchange { get; }

        /// <summary>
        /// Gets bid quantity.
        /// </summary>
        Int64 BidSize { get; }

        /// <summary>
        /// Gets ask quantity.
        /// </summary>
        Int64 AskSize { get; }

        /// <summary>
        /// Gets bid price.
        /// </summary>
        Decimal BidPrice { get; }

        /// <summary>
        /// Gets ask price.
        /// </summary>
        Decimal AskPrice { get; }

        /// <summary>
        /// Gets quote conditions.
        /// </summary>
        IReadOnlyList<Int64> Conditions { get; }

        /// <summary>
        /// Gets tape where trade occured.
        /// </summary>
        Int64 Tape { get; }

        /// <summary>
        /// Gets sequence number of trade.
        /// </summary>
        Int64 SequenceNumber { get; }

        /// <summary>
        /// Gets indicators.
        /// </summary>
        IReadOnlyList<Int64> Indicators { get; }
    }
}
