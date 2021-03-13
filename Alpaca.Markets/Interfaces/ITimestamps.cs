using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates timestamps information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [Obsolete("This interface will be removed in the next major SDK release.", false)]
    public interface ITimestamps
    {
        /// <summary>
        /// Gets SIP timestamp in UTC time zone.
        /// </summary>
        [Obsolete("This property will be moved into the IHistoricalBase interface in the next major SDK release.", false)]
        DateTime? TimestampUtc { get; }

        /// <summary>
        /// Gets participant/exchange timestamp in UTC time zone.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        DateTime? ParticipantTimestampUtc { get; }

        /// <summary>
        /// Gets trade reporting facility timestamp in UTC time zone.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        DateTime? TradeReportingFacilityTimestampUtc { get; }
    }
}
