#if !NET6_0_OR_GREATER
namespace Alpaca.Markets;

internal static class EnumerableExtensions
{
    public static IEnumerable<TSource[]> Chunk<TSource>(
        this IEnumerable<TSource> source,
        Int32 size)
    {
        using var e = source.EnsureNotNull().GetEnumerator();
        while (e.MoveNext())
        {
            var chunk = new TSource[size];
            chunk[0] = e.Current;

            var i = 1;
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
