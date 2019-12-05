using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates list of single day historical items from Polygon REST API.
    /// </summary>
    /// <typeparam name="TItem">Type of historical items inside this container.</typeparam>
    public interface IDayHistoricalItems<out TItem> : IHistoricalItems<TItem>
    {
        /// <summary>
        /// Gets historical items day.
        /// </summary>
        DateTime ItemsDay { get; }
    }
}
