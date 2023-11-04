using System.Threading.Channels;

namespace Alpaca.Markets.Extensions;

internal static class PaginationExtensions
{
    public static async IAsyncEnumerable<TItem> GetResponsesByItems<TRequest, TItem>(
        this TRequest singlePageOfItemsRequestWithEmptyPageToken,
        Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TRequest : class, IHistoricalRequest
    {
        await foreach (var page in GetResponsesByPages(
                singlePageOfItemsRequestWithEmptyPageToken, getSinglePage, cancellationToken)
            .ConfigureAwait(false))
        {
            foreach (var item in page)
            {
                yield return item;
            }
        }
    }

    public static IReadOnlyDictionary<String, IAsyncEnumerable<TItem>> GetResponsesByItems<TRequest, TItem>(
        this TRequest singlePageOfItemsRequestWithEmptyPageToken,
        Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
        CancellationToken cancellationToken = default)
        where TRequest : HistoricalRequestBase, IHistoricalRequest
    {
        var channelsBySymbols =
            singlePageOfItemsRequestWithEmptyPageToken.Symbols
                .ToDictionary(symbol => symbol, _ => Channel.CreateUnbounded<TItem>(),
                    StringComparer.Ordinal);

        Task.Run(GetResponsesByItemsImpl, cancellationToken);

        return channelsBySymbols.ToDictionary(
            pair => pair.Key, pair => ReadAllAsync(pair.Value.Reader, cancellationToken),
            StringComparer.Ordinal);

        async Task GetResponsesByItemsImpl()
        {
            await foreach (var page in GetResponsesByPages(
                    singlePageOfItemsRequestWithEmptyPageToken, getSinglePage, cancellationToken)
                .ConfigureAwait(false))
            {
                foreach (var (symbol, items) in page)
                {
                    await WriteAllAsync(channelsBySymbols[symbol].Writer, items, cancellationToken)
                        .ConfigureAwait(false);
                }
            }

            foreach (var channel in channelsBySymbols.Values)
            {
                channel.Writer.TryComplete();
            }
        }

        static async IAsyncEnumerable<T> ReadAllAsync<T>(
            ChannelReader<T> reader,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            while (await reader.WaitToReadAsync(cancellationToken).ConfigureAwait(false))
            {
                while (reader.TryRead(out var item))
                {
                    yield return item;
                }
            }
        }

        static async ValueTask WriteAllAsync<T>(
            ChannelWriter<T> writer,
            IEnumerable<T> items,
            CancellationToken cancellationToken)
        {
            foreach (var item in items)
            {
                await writer.WriteAsync(item, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public static IAsyncEnumerable<IReadOnlyList<TItem>> GetResponsesByPages<TRequest, TItem>(
        this TRequest singlePageOfItemsRequestWithEmptyPageToken,
        Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
        CancellationToken cancellationToken = default)
        where TRequest : class, IHistoricalRequest =>
        getResponses(
            singlePageOfItemsRequestWithEmptyPageToken,
            (request, ct) => getItemsAndNextPageToken(getSinglePage, request, ct),
            cancellationToken);

    public static IAsyncEnumerable<IReadOnlyDictionary<String, IReadOnlyList<TItem>>> GetResponsesByPages<TRequest, TItem>(
        this TRequest singlePageOfItemsRequestWithEmptyPageToken,
        Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
        CancellationToken cancellationToken = default)
        where TRequest : class, IHistoricalRequest =>
        getResponses(
            singlePageOfItemsRequestWithEmptyPageToken,
            (request, ct) => getItemsAndNextPageToken(getSinglePage, request, ct),
            cancellationToken);

    private static async IAsyncEnumerable<TResponse> getResponses<TRequest, TResponse>(
        TRequest singlePageOfItemsRequestWithEmptyPageToken,
        Func<TRequest, CancellationToken, Task<(TResponse, String?)>> getItemsAndNextPageToken,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TRequest : class, IHistoricalRequest
    {
        var request = singlePageOfItemsRequestWithEmptyPageToken;
        do
        {
            var (items, nextPageToken) = await getItemsAndNextPageToken(
                request, cancellationToken).ConfigureAwait(false);

            yield return items;

            request = request.WithPageToken(nextPageToken ?? String.Empty);
        } while (!String.IsNullOrEmpty(request.Pagination.Token));
    }

    private static async Task<(IReadOnlyList<TItem>, String?)> getItemsAndNextPageToken<TRequest, TItem>(
        this Func<TRequest, CancellationToken, Task<IPage<TItem>>> getSinglePage,
        TRequest request,
        CancellationToken cancellationToken)
    {
        var response = await getSinglePage(request, cancellationToken).ConfigureAwait(false);
        return (response.Items, response.NextPageToken);
    }

    private static async Task<(IReadOnlyDictionary<String, IReadOnlyList<TItem>>, String?)> getItemsAndNextPageToken<TRequest, TItem>(
        this Func<TRequest, CancellationToken, Task<IMultiPage<TItem>>> getSinglePage,
        TRequest request,
        CancellationToken cancellationToken)
    {
        var response = await getSinglePage(request, cancellationToken).ConfigureAwait(false);
        return (response.Items, response.NextPageToken);
    }
}
