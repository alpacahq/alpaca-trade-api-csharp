using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates full account information from Alpaca REST API.
    /// </summary>
    public interface IAccount : IAccountBase
    {
        /// <summary>
        /// Gets total account portfolio value.
        /// </summary>
        Decimal PortfolioValue { get;  }

        /// <summary>
        /// Gets returns <c>true</c> if account is linked to day pattern trader.
        /// </summary>
        Boolean IsDayPatternTrader { get;  }

        /// <summary>
        /// Gets returns <c>true</c> if account trading functions are blocked.
        /// </summary>
        Boolean IsTradingBlocked { get; }

        /// <summary>
        /// Gets returns <c>true</c> if account transfer functions are blocked.
        /// </summary>
        Boolean IsTransfersBlocked { get; }

        /// <summary>
        /// Gets returns <c>true</c> if account is completely blocked.
        /// </summary>
        Boolean IsAccountBlocked { get; }
    }
}