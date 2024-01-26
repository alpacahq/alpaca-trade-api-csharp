namespace Alpaca.Markets;

/// <summary>
/// Encapsulates full account information from Alpaca REST API.
/// </summary>
[CLSCompliant(false)]
public interface IAccount
{
    /// <summary>
    /// Gets unique account identifier.
    /// </summary>
    [UsedImplicitly]
    Guid AccountId { get; }

    /// <summary>
    /// Gets updated account status.
    /// </summary>
    [UsedImplicitly]
    AccountStatus Status { get; }

    /// <summary>
    /// Gets updated crypto account status.
    /// </summary>
    [UsedImplicitly]
    AccountStatus CryptoStatus { get; }

    /// <summary>
    /// Gets main account currency.
    /// </summary>
    [UsedImplicitly]
    String? Currency { get; }

    /// <summary>
    /// Gets amount of money available for trading.
    /// </summary>
    [UsedImplicitly]
    Decimal TradableCash { get; }

    /// <summary>
    /// Gets timestamp of account creation event in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime CreatedAtUtc { get; }

    /// <summary>
    /// Gets account number (string identifier).
    /// </summary>
    [UsedImplicitly]
    String? AccountNumber { get; }

    /// <summary>
    /// Returns <c>true</c> if account is linked to pattern day trader.
    /// </summary>
    [UsedImplicitly]
    Boolean IsDayPatternTrader { get; }

    /// <summary>
    /// Returns <c>true</c> if account trading functions are blocked.
    /// </summary>
    [UsedImplicitly]
    Boolean IsTradingBlocked { get; }

    /// <summary>
    ///  Returns <c>true</c> if account transfer functions are blocked.
    /// </summary>
    [UsedImplicitly]
    Boolean IsTransfersBlocked { get; }

    /// <summary>
    /// User setting. If <c>true</c>, the account is not allowed to place orders.
    /// </summary>
    [UsedImplicitly]
    Boolean TradeSuspendedByUser { get; }

    /// <summary>
    /// Flag to denote whether or not the account is permitted to short.
    /// </summary>
    [UsedImplicitly]
    Boolean ShortingEnabled { get; }

    /// <summary>
    /// Buying power multiplier that represents account margin classification.
    /// </summary>
    [UsedImplicitly]
    Multiplier Multiplier { get; }

    /// <summary>
    /// Current available buying power.
    /// </summary>
    [UsedImplicitly]
    Decimal? BuyingPower { get; }

    /// <summary>
    /// Your buying power for day trades (continuously updated value).
    /// </summary>
    [UsedImplicitly]
    Decimal? DayTradingBuyingPower { get; }

    /// <summary>
    /// Your buying power under Regulation T (your excess equity - equity minus margin value - times your margin multiplier).
    /// </summary>
    [UsedImplicitly]
    Decimal? RegulationBuyingPower { get; }

    /// <summary>
    /// Your non-marginable buying power for day trades (useful for crypto-trading).
    /// </summary>
    [UsedImplicitly]
    Decimal? NonMarginableBuyingPower { get; }

    /// <summary>
    /// Real-time MtM value of all long positions held in the account.
    /// </summary>
    [UsedImplicitly]
    Decimal? LongMarketValue { get; }

    /// <summary>
    /// Real-time MtM value of all short positions held in the account.
    /// </summary>
    [UsedImplicitly]
    Decimal? ShortMarketValue { get; }

    /// <summary>
    /// Cash + LongMarketValue + ShortMarketValue.
    /// </summary>
    [UsedImplicitly]
    Decimal? Equity { get; }

    /// <summary>
    /// Equity as of previous trading day at 16:00:00 ET.
    /// </summary>
    [UsedImplicitly]
    Decimal LastEquity { get; }

    /// <summary>
    /// Reg T initial margin requirement (continuously updated value).
    /// </summary>
    [UsedImplicitly]
    Decimal? InitialMargin { get; }

    /// <summary>
    /// Maintenance margin requirement (continuously updated value).
    /// </summary>
    [UsedImplicitly]
    Decimal MaintenanceMargin { get; }

    /// <summary>
    /// Your maintenance margin requirement on the previous trading day
    /// </summary>
    [UsedImplicitly]
    Decimal LastMaintenanceMargin { get; }

    /// <summary>
    /// the current number of day trades that have been made in the last 5 trading days (inclusive of today).
    /// </summary>
    [UsedImplicitly]
    UInt64 DayTradeCount { get; }

    /// <summary>
    /// Value of special memorandum account.
    /// </summary>
    [UsedImplicitly]
    Decimal Sma { get; }

    /// <summary>
    /// Returns <c>true</c> if account is completely blocked.
    /// </summary>
    [UsedImplicitly]
    Boolean IsAccountBlocked { get; }

    /// <summary>
    /// Gets fees collected value (if any).
    /// </summary>
    [UsedImplicitly]
    Decimal? AccruedFees { get; }

    /// <summary>
    /// Gets cash pending transfer in.
    /// </summary>
    [UsedImplicitly]
    Decimal? PendingTransferIn { get; }

    /// <summary>
    /// Gets cash pending transfer out.
    /// </summary>
    [UsedImplicitly]
    Decimal? PendingTransferOut { get; }

    /// <summary>
    /// Gets the effective options trading level of the account. This is the minimum between the
    /// <see cref="OptionsApprovedLevel"/> and the <see cref="IAccountConfiguration.MaxOptionsTradingLevel"/>.
    /// </summary>
    [UsedImplicitly]
    OptionsTradingLevel? OptionsTradingLevel { get; }

    /// <summary>
    /// Gets the options trading level that was approved for this account.
    /// </summary>
    [UsedImplicitly]
    OptionsTradingLevel? OptionsApprovedLevel { get; }

    /// <summary>
    /// Gets the option buying power that was approved for this account.
    /// </summary>
    [UsedImplicitly]
    Decimal? OptionsBuyingPower { get; }
}
