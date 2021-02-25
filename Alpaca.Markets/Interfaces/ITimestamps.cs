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
        /// Gets SIP timestamp in UTC time zone.
        /// </summary>
        DateTime? TimestampUtc { get; }

        /// <summary>
        /// Gets participant/exchange timestamp in UTC time zone.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", false)]
        DateTime? ParticipantTimestampUtc { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp in UTC time zone.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", false)]
        DateTime? TradeReportingFacilityTimestampUtc { get; }
    }
}
