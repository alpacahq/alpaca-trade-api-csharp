#if !NET6_0_OR_GREATER
namespace Alpaca.Markets;

internal static class EnumerableExtensions
{
    public static IEnumerable<TSource[]> Chunk<TSource>(
        this IEnumerable<TSource> source,
        int size)
    {
        using IEnumerator<TSource> e = source.GetEnumerator();
        while (e.MoveNext())
        {
            TSource[] chunk = new TSource[size];
            chunk[0] = e.Current;

            int i = 1;
            for (; i < chunk.Length && e.MoveNext(); i++)
            {
                chunk[i] = e.Current;
            }

            if (i == chunk.Length)
            {
                yield return chunk;
            }
            else
            {
                Array.Resize(ref chunk, i);
                yield return chunk;
                yield break;
            }
        }
    }
}
#endif
