using System;
using System.Collections.Generic;
using System.Text;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates portfolio history information from Alpaca REST API.
    /// </summary>
    public interface IPortfolioHistory
    {
        /// <summary>
        /// Gets historical equity values.
        /// </summary>
        IReadOnlyList<Decimal?>? Equity { get; }

        /// <summary>
        /// Gets historical profit/loss values.
        /// </summary>
        IReadOnlyList<Decimal?> ProfitLoss { get; }

        /// <summary>
        /// Gets historical profit/loss values as percentages.
        /// </summary>
        IReadOnlyList<Decimal?> ProfitLossPct { get; }

        /// <summary>
        /// Gets historical timestamp values.
        /// </summary>
        IReadOnlyList<DateTime> Timestamps { get; }

        /// <summary>
        /// Gets timeframe value for this historical view.
        /// </summary>
        HistoryTimeframe Timeframe { get; }

        /// <summary>
        /// Gets base value for this historical view.
        /// </summary>
        Decimal BaseValue { get; }
    }
}
