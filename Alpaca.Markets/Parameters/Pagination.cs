namespace Alpaca.Markets;

/// <summary>
/// Encapsulates all data required for the pagination support in Alpaca Data API v2
/// </summary>
public sealed class Pagination
{
    internal const UInt32 MinPageSize = 1;

    /// <summary>
    /// Gets the maximum valid page size for requests supported by Alpaca Data API v2.
    /// </summary>
    [CLSCompliant(false)]
    public static UInt32 MaxPageSize => 10_000;

    /// <summary>
    /// Gets the maximum valid page size for news requests supported by Alpaca Data API v2.
    /// </summary>
    internal static UInt32 MaxNewsPageSize => 50;

    /// <summary>
    /// Gets and sets the request page size. If equals to <c>null</c> default size will be used.
    /// </summary>
    [CLSCompliant(false)]
    public UInt32? Size { get; set; }

    /// <summary>
    /// Gets and sets the page token for the request. Should be <c>null</c> for the first request.
    /// </summary>
    public String? Token { get; set; }

    internal QueryBuilder QueryBuilder =>
        new QueryBuilder()
            .AddParameter("page_token", Token)
            .AddParameter("limit", Size);
}
