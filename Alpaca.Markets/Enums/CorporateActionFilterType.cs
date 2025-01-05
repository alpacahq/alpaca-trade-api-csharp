namespace Alpaca.Markets;

/// <summary>
/// Supported bar corporate action adjustment types for Alpaca Data API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CorporateActionFilterType
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "reverse_split")]
    ReverseSplit,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "forward_split")]
    ForwardSplit,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "unit_split")]
    UnitSplit,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "cash_dividend")]
    CashDividend,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock_dividend")]
    StockDividend,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "spin_off")]
    SpinOff,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "cash_merger")]
    CashMerger,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock_merger")]
    StockMerger,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock_and_cash_merger")]
    StockAndCashMerger,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "redemption")]
    Redemption,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "name_change")]
    NameChange,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "worthless_removal")]
    WorthlessRemoval,

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "rights_distribution")]
    RightsDistribution
}
