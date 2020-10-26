using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates portfolio history information item from Alpaca REST API.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IPortfolioHistoryItem
    {
        /// <summary>
        /// Gets historical equity value.
        /// </summary>
        Decimal? Equity { get; }

        /// <summary>
        /// Gets historical profit/loss value.
        /// </summary>
        Decimal? ProfitLoss { get; }

        /// <summary>
        /// Gets historical profit/loss value as percentages.
        /// </summary>
        Decimal? ProfitLossPercentage { get; }

        /// <summary>
        /// Gets historical timestamp value in UTC time zone.
        /// </summary>
        DateTime TimestampUtc { get; }
    }
}
