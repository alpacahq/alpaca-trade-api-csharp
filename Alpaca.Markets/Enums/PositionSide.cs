namespace Alpaca.Markets;

/// <summary>
/// Position side in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
[SuppressMessage(
    "Naming", "CA1720:Identifier contains type name",
    Justification = "Both names are trading terms not CLR type names.")]
public enum PositionSide
{
    /// <summary>
    /// Long position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "long")]
    Long,

    /// <summary>
    /// Short position.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "short")]
    Short
}
