using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates quote information from Alpaca streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    public interface IRealTimeQuote : IQuote
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        IReadOnlyList<String> Conditions { get; }
    }
}
