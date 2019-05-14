using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates list of historical aggregates (bars) from Polygon's v2 REST API.
    /// </summary>
    /// <typeparam name="TItem">Type of historical items inside this container.</typeparam>
    public interface IHistoricalItemsV2<out TItem> : IHistoricalItems<TItem>
    {
        /// <summary>
        /// Indicates if this response was adjusted for splits.
        /// </summary>
        Boolean Adjusted { get; }

        /// <summary>
        /// Number of aggregates (minutes or days) used to generate the response.
        /// </summary>
        Int64 QueryCount { get; }

        /// <summary>
        /// Total number of results generated.
        /// </summary>
        Int64 ResultsCount { get; }
    }
}