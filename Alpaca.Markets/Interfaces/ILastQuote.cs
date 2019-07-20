using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates last quote information from Alpaca REST API.
    /// </summary>
    public interface ILastQuote : IStreamQuote
    {
        /// <summary>
        /// Gets quote response status.
        /// </summary>
        String Status { get; }
    }
}
