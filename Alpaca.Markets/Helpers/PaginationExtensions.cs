namespace Alpaca.Markets;

internal static class PaginationExtensions
{
    public static async Task<IPage<TItem>> AsPageAsync<TItem, TPage>(
        this Task<IMultiPage<TItem>> response) where TPage : IPageMutable<TItem>, new() =>
        (await response.ConfigureAwait(false)).asPage<TItem, TPage>();

    public static async Task<IMultiPage<TItem>> AsMultiPageAsync<TItem, TPage>(
        this Task<IPage<TItem>> response) where TPage : IMultiPageMutable<TItem>, new() =>
        (await response.ConfigureAwait(false)).asMultiPage<TItem, TPage>();

    private static IPage<TItem> asPage<TItem, TPage>(
        this IMultiPage<TItem> response) where TPage : IPageMutable<TItem>, new() =>
        new TPage
        {
            Items = new List<TItem>(response.Items.SelectMany(_ => _.Value)),
            NextPageToken = response.NextPageToken
        };

    private static IMultiPage<TItem> asMultiPage<TItem, TPage>(
        this IPage<TItem> response) where TPage : IMultiPageMutable<TItem>, new() =>
        new TPage
        {
            Items = new Dictionary<String, IReadOnlyList<TItem>>(StringComparer.Ordinal)
            {
                    { response.Symbol, response.Items }
            },
            NextPageToken = response.NextPageToken
        };
}
