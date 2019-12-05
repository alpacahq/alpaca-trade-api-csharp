using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical quote information from Polygon REST API.
    /// </summary>
    public interface IHistoricalQuote : IQuoteBase<String>, IQuoteBase<Int64>
    {
        /// <summary>
        /// Gets time offset of quote.
        /// </summary>
        [Obsolete("TimeOffset is deprecated in API v2, use SipTimestamp instead", false)]
        Int64 TimeOffset { get; }

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

        /// <summary>
        /// Gets indicators.
        /// </summary>
        IReadOnlyList<Int64> Indicators { get; }
    }
}
