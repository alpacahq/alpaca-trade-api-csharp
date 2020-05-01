using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base streaming item information from data streaming API.
    /// </summary>
    public interface IStreamBase
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        String Symbol { get; }
    }
}
