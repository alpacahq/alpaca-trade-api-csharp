using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical quote information from Polygon REST API.
    /// </summary>
    public interface IHistoricalQuote : IQuoteBase<String>
    {
        /// <summary>
        /// Gets time offset of quote.
        /// </summary>
        Int64 TimeOffset { get; }
    }
}
