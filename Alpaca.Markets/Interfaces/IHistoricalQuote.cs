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
#pragma warning disable 618
    public interface IHistoricalQuote : IQuoteBase<String>, IQuoteBase<Int64>, ITimestamps, IHistoricalBase
#pragma warning restore 618
    {
        /// <summary>
        /// Gets indicators.
        /// </summary>
        IReadOnlyList<Int64> Indicators { get; }
    }
}
