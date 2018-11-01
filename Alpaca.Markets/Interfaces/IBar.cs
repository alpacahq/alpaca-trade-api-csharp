using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates bar information from Polygon REST API.
    /// </summary>
    public interface IBar : IBarBase
    {
        /// <summary>
        /// Gets bar timestamp.
        /// </summary>
        DateTime Time { get; }
    }
}