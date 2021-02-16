using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base streaming item information from data streaming API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IStreamBase
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }
    }
}
