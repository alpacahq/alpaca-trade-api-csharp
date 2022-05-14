namespace Alpaca.Markets;

/// <summary>
/// Encapsulates single page response in Alpaca Data API v2.
/// </summary>
/// <typeparam name="TItems">Type of paged item (bar, trade or quote)</typeparam>
public interface IPage<out TItems>
{
    /// <summary>
    /// Gets the asset symbol for all items on page.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    /// <summary>
    /// Gets the next page token for continuation. If value of this property
    /// equals to <c>null</c> this page is the last one and no more data is available.
    /// </summary>
    [UsedImplicitly]
    public String? NextPageToken { get; }

    /// <summary>
    /// Gets list of items for this response.
    /// </summary>
    public IReadOnlyList<TItems> Items { get; }
}
