namespace Alpaca.Markets;

/// <summary>
/// Supported sort directions in Alpaca REST API.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SortDirection
{
    /// <summary>
    /// Descending sort order
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "desc")]
    Descending,

    /// <summary>
    /// Ascending sort order
    /// </summary>
    [UsedImplicitly]
    [EnumMember(Value = "asc")]
    Ascending
}
