namespace Alpaca.Markets;

/// <summary>
/// Supported option contract types for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OptionType
{
    /// <summary>
    /// Call option contract.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "call")]
    Call,

    /// <summary>
    /// Put option contract.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "put")]
    Put
}
