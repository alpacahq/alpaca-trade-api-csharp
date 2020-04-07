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
