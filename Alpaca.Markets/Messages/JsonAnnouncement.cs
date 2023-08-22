namespace Alpaca.Markets;

[SuppressMessage(
    "Microsoft.Performance", "CA1812:Avoid uninstantiated internal classes",
    Justification = "Object instances of this class will be created by Newtonsoft.JSON library.")]
internal sealed class JsonAnnouncement : IAnnouncement
{
    [JsonProperty(PropertyName = "id", Required = Required.Always)]
    public Guid Id { get; set; }

    [JsonProperty(PropertyName = "corporate_action_id", Required = Required.Always)]
    public String CorporateActionId { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "ca_type", Required = Required.Always)]
    public CorporateActionType Type { get; set; }

    [JsonProperty(PropertyName = "ca_sub_type", Required = Required.Always)]
    public CorporateActionSubType SubType { get; set; }

    [JsonProperty(PropertyName = "initiating_symbol", Required = Required.Always)]
    public String InitiatingSymbol { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "initiating_original_cusip", Required = Required.Always)]
    public String InitiatingCusip { get; set; } = String.Empty;

    [JsonProperty(PropertyName = "target_symbol", Required = Required.Default)]
    public String TargetSymbol { get; [ExcludeFromCodeCoverage] set; } = String.Empty;

    [JsonProperty(PropertyName = "target_original_cusip", Required = Required.Default)]
    public String TargetCusip { get; [ExcludeFromCodeCoverage] set; } = String.Empty;

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "declaration_date", Required = Required.Default)]
    public DateOnly? DeclarationDate { get; [ExcludeFromCodeCoverage] set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "ex_date", Required = Required.Default)]
    public DateOnly? ExecutionDate { get; [ExcludeFromCodeCoverage] set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "record_date", Required = Required.AllowNull)]
    public DateOnly? RecordDate { get; set; }

    [JsonConverter(typeof(DateOnlyConverter))]
    [JsonProperty(PropertyName = "payable_date", Required = Required.Default)]
    public DateOnly? PayableDate { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "cash", Required = Required.Always)]
    public Decimal Cash { get; set; }

    [JsonProperty(PropertyName = "old_rate", Required = Required.Default)]
    public Decimal OldRate { get; [ExcludeFromCodeCoverage] set; }

    [JsonProperty(PropertyName = "new_rate", Required = Required.Default)]
    public Decimal NewRate { get; [ExcludeFromCodeCoverage] set; }

    public DateOnly? GetDate(
        CorporateActionDateType dateType) =>
        dateType switch
        {
            CorporateActionDateType.DeclarationDate => DeclarationDate,
            CorporateActionDateType.ExecutionDate => ExecutionDate,
            CorporateActionDateType.PayableDate => PayableDate,
            CorporateActionDateType.RecordDate => RecordDate,
            _ => null
        };
}
