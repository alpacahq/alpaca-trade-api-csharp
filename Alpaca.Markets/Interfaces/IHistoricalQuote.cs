using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical quote information from Polygon REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IHistoricalQuote : IQuoteBase<String>, IQuoteBase<Int64>, ITimestamps, IHistoricalBase
    {
        /// <summary>
        /// Gets indicators.
        /// </summary>
        IReadOnlyList<Int64> Indicators { get; }
    }
}
