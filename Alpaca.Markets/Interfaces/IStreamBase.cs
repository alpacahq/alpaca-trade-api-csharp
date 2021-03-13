using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates base streaming item information from data streaming API.
    /// </summary>
    [Obsolete("This interface will be removed in the next major SDK release.", false)]
    public interface IStreamBase
    {
        /// <summary>
        /// Gets asset name.
        /// </summary>
        [Obsolete("This property will be moved up in the interface hierarchy in the next major SDK release.", false)]
        String Symbol { get; }
    }
}
