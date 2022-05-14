namespace Alpaca.Markets;

/// <summary>
/// Corporate action sub type in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CorporateActionSubType
{
    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Dividend"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "cash")]
    DividendCash,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Dividend"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock")]
    DividendStock,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Merger"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "merger_update")]
    MergerUpdate,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Merger"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "merger_completion")]
    MergerCompletion,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.SpinOff"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "spinoff")]
    SpinOff,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "stock_split")]
    StockSplit,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "unit_split")]
    UnitSplit,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "reverse_split")]
    ReverseSplit,

    /// <summary>
    /// Sub type for the <see cref="CorporateActionType.Split"/> type.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "recapitalization")]
    Recapitalizaiton
}
