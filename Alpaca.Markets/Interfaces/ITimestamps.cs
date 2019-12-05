using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates timestamps information from Polygon REST API.
    /// </summary>
    public interface ITimestamps
    {
        /// <summary>
        /// Gets SIP timestamp.
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// Gets participant/exchange timestamp.
        /// </summary>
        DateTime ParticipantTimestamp { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp.
        /// </summary>
        DateTime TradeReportingFacilityTimestamp { get; }
    }
}