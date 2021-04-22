using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates historical quote information from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [CLSCompliant(false)]
    // ReSharper disable once PossibleInterfaceMemberAmbiguity
    public interface IHistoricalQuote : IQuote
    {
        /// <summary>
        /// Gets trade conditions list.
        /// </summary>
        IReadOnlyList<String> Conditions { get; }
    }
}
