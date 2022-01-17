namespace Alpaca.Markets;

/// <summary>
/// Encapsulates all data required for the pagination support in Alpaca Data API v2
/// </summary>
public sealed class Pagination
{
    private const UInt32 MinPageSize = 1;

    /// <summary>
    /// Gets ths maximal valid page size for the request supported by Alpaca Data API v2.
    /// </summary>
    [CLSCompliant(false)]
    public static UInt32 MaxPageSize => 10_000;

    /// <summary>
    /// Gets ths maximal valid page size for the news request supported by Alpaca Data API v2.
    /// </summary>
    internal static UInt32 MaxNewsPageSize => 100;

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

    internal Boolean TryGetException(
        UInt32 maxPageSize,
        out RequestValidationException? exception)
    {
        if (Size >= MinPageSize && Size <= maxPageSize)
        {
            exception = null;
            return false;
        }

        exception = new RequestValidationException("Invalid page size");
        return true;
    }
}
