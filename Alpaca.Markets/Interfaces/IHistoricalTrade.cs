using System;

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
        String Exchange { get; }

        /// <summary>
        /// Gets trade timestamp.
        /// </summary>
        Int64 TimeOffset  { get; }

        /// <summary>
        /// Gets trade price.
        /// </summary>
        Decimal Price { get; }

        /// <summary>
        /// Gets trade quantity.
        /// </summary>
        Int64 Size { get; }
    }
}
