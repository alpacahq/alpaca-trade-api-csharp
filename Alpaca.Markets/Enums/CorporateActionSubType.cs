namespace Alpaca.Markets;

/// <summary>
/// Corporate action subtype in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CorporateActionSubType
{
    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Dividend"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "cash")]
    DividendCash,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Dividend"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock")]
    DividendStock,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Merger"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "merger_update")]
    MergerUpdate,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Merger"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "merger_completion")]
    MergerCompletion,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.SpinOff"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "spinoff")]
    SpinOff,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock_split")]
    StockSplit,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "unit_split")]
    UnitSplit,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "reverse_split")]
    ReverseSplit,

    /// <summary>
    /// Subtype for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "recapitalization")]
    Recapitalizaiton
}
