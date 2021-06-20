using System;
using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates full account information from Alpaca REST API.
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IAccount
    {
        /// <summary>
        /// Gets unique account identifier.
        /// </summary>
        Guid AccountId { get; }

        /// <summary>
        /// Gets updated account status.
        /// </summary>
        AccountStatus Status { get; }

        /// <summary>
        /// Gets main account currency.
        /// </summary>
        String? Currency { get; }

        /// <summary>
        /// Gets amount of money available for trading.
        /// </summary>
        Decimal TradableCash { get; }

        /// <summary>
        /// Gets timestamp of account creation event in UTC.
        /// </summary>
        DateTime CreatedAtUtc { get; }

        /// <summary>
        /// Gets account number (string identifier).
        /// </summary>
        String? AccountNumber { get; }

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
        /// User setting. If <c>true</c>, the account is not allowed to place orders.
        /// </summary>
        Boolean TradeSuspendedByUser { get; }

        /// <summary>
        /// Flag to denote whether or not the account is permitted to short.
        /// </summary>
        Boolean ShortingEnabled { get; }

        /// <summary>
        /// Buying power multiplier that represents account margin classification.
        /// </summary>
        Multiplier Multiplier { get; }

        /// <summary>
        /// Current available buying power.
        /// </summary>
        Decimal BuyingPower { get; }

        /// <summary>
        /// Your buying power for day trades (continuously updated value).
        /// </summary>
        Decimal DayTradingBuyingPower { get; }

        /// <summary>
        /// Your buying power under Regulation T (your excess equity - equity minus margin value - times your margin multiplier).
        /// </summary>
        Decimal RegulationBuyingPower { get; }

        /// <summary>
        /// Real-time MtM value of all long positions held in the account.
        /// </summary>
        Decimal LongMarketValue { get; }

        /// <summary>
        /// Real-time MtM value of all short positions held in the account.
        /// </summary>
        Decimal ShortMarketValue { get; }

        /// <summary>
        /// Cash + LongMarketValue + ShortMarketValue.
        /// </summary>
        Decimal Equity { get; }

        /// <summary>
        /// Equity as of previous trading day at 16:00:00 ET.
        /// </summary>
        Decimal LastEquity { get; }

        /// <summary>
        /// Reg T initial margin requirement (continuously updated value).
        /// </summary>
        Decimal InitialMargin { get; }

        /// <summary>
        /// Maintenance margin requirement (continuously updated value).
        /// </summary>
        Decimal MaintenanceMargin { get; }

        /// <summary>
        /// Your maintenance margin requirement on the previous trading day
        /// </summary>
        Decimal LastMaintenanceMargin { get; }

        /// <summary>
        /// the current number of day trades that have been made in the last 5 trading days (inclusive of today).
        /// </summary>
        UInt64 DayTradeCount { get; }

        /// <summary>
        /// value of special memorandum account.
        /// </summary>
        Decimal Sma { get; }

        /// <summary>
        /// Gets returns <c>true</c> if account is completely blocked.
        /// </summary>
        Boolean IsAccountBlocked { get; }
    }
}
