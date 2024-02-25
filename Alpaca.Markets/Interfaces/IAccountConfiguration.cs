namespace Alpaca.Markets;

/// <summary>
/// Encapsulates account configuration settings from Alpaca REST API.
/// </summary>
public interface IAccountConfiguration
{
    /// <summary>
    /// Gets or sets day trade margin call protection mode for account.
    /// </summary>
    [UsedImplicitly]
    DayTradeMarginCallProtection DayTradeMarginCallProtection { get; set; }

    /// <summary>
    /// Gets or sets notification level for order fill emails.
    /// </summary>
    [UsedImplicitly]
    TradeConfirmEmail TradeConfirmEmail { get; set; }

    /// <summary>
    /// Gets or sets control flag for blocking new orders placement.
    /// </summary>
    [UsedImplicitly]
    Boolean IsSuspendTrade { get; set; }

    /// <summary>
    /// Gets or sets control flag for enabling long-only account mode.
    /// </summary>
    [UsedImplicitly]
    Boolean IsNoShorting { get; set; }

    /// <summary>
    /// Gets or sets control flag for enabling orders acceptance for PTP symbols with no exception.
    /// </summary>
    [UsedImplicitly]
    public Boolean IsPtpNoExceptionEntry { get; set; }

    /// <summary>
    /// Gets the desired maximum options trading level.
    /// </summary>
    [UsedImplicitly]
    OptionsTradingLevel? MaxOptionsTradingLevel { get; }
}
