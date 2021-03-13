using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical trade information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
#pragma warning disable 618
    public interface IHistoricalTrade : ITimestamps, IHistoricalBase
#pragma warning restore 618
    {
        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        Int64 ExchangeId { get; }

        /// <summary>
        /// Gets trade source exchange identifier.
        /// </summary>
        String Exchange { get; }

        /// <summary>
        /// Gets trade price.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        Int64 Size { get; }

        /// <summary>
        /// Gets trade reporting facility ID.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        Int64 TradeReportingFacilityId { get; }

        /// <summary>
        /// Gets trade ID.
        /// </summary>
        String? TradeId { get; }

        /// <summary>
        /// Gets original trade ID.
        /// </summary>
        [Obsolete("This property will be removed in the next major SDK release.", true)]
        String? OriginalTradeId { get; }
    }
}
