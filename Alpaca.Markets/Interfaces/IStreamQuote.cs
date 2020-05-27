using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates quote information from Polygon streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStreamQuote : IQuoteBase<Int64>, IStreamBase
    {
        /// <summary>
        /// Gets quote timestamp.
        /// </summary>
        DateTime Time { get; }
    }
}
