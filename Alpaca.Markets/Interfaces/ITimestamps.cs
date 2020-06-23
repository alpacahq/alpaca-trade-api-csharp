using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates timestamps information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface ITimestamps
    {
        /// <summary>
        /// Gets SIP timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the TimestampUtc property instead.", false)]
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets SIP timestamp in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }

        /// <summary>
        /// Gets participant/exchange timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the ParticipantTimestampUtc property instead.", false)]
        DateTime ParticipantTimestamp { get; }

        /// <summary>
        /// Gets participant/exchange timestamp in UTC time zone.
        /// </summary>
        DateTime ParticipantTimestampUtc { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp.
        /// </summary>
        [Obsolete("This property will be removed in the next major release. Use the TradeReportingFacilityTimestampUtc property instead.", false)]
        DateTime TradeReportingFacilityTimestamp { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp in UTC time zone.
        /// </summary>
        DateTime TradeReportingFacilityTimestampUtc { get; }
    }
}
