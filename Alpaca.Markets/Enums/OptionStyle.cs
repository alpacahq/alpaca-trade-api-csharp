namespace Alpaca.Markets;

/// <summary>
/// Supported option contract styles for Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OptionStyle
{
    /// <summary>
    /// American option contract execution style.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "american")]
    American,

    /// <summary>
    /// European option contract execution style.
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "european")]
    European
}
