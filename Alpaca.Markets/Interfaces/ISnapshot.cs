using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates snapshot information from the Alpaca REST API.
    /// </summary>
    public interface ISnapshot
    {
        /// <summary>
        /// Gets the snapshot's asset name.
        /// </summary>
        String Symbol { get; }

        /// <summary>
        /// Gets the latest trade information.
        /// </summary>
        IStreamQuote? Quote { get; }

        /// <summary>
        /// Gets the latest quote information.
        /// </summary>
        IStreamTrade? Trade { get; }

        /// <summary>
        /// Gets the current minute bar information.
        /// </summary>
        IAgg? MinuteBar { get; }

        /// <summary>
        /// Gets the current daily bar information.
        /// </summary>
        IAgg? CurrentDailyBar { get; }

        /// <summary>
        /// Gets the previous minute bar information.
        /// </summary>
        IAgg? PreviousDailyBar { get; }
    }
}
