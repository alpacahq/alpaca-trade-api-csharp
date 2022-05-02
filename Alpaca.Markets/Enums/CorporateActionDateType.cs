namespace Alpaca.Markets;

/// <summary>
/// Corporate action type in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CorporateActionDateType
{
    /// <summary>
    /// Date the corporate action or subsequent terms update was announced.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "declaration_date")]
    DeclarationDate,

    /// <summary>
    /// The first date that purchasing a security will not
    /// result in a corporate action entitlement.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "ex_date")]
    ExecutionDate,

    /// <summary>
    /// The date an account must hold a settled position in the security
    /// in order to receive the corporate action entitlement.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "record_date")]
    RecordDate,

    /// <summary>
    /// The date the announcement will take effect. On this date, account
    /// stock and cash balances are expected to be processed accordingly.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "payable_date")]
    PayableDate
}
