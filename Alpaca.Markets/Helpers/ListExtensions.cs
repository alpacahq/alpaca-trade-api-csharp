namespace Alpaca.Markets;

internal static class ListExtensions
{
    public static IReadOnlyList<TInto> EmptyIfNull<TInto, TFrom>(this List<TFrom>? list)
        where TFrom : TInto => EmptyIfNull((IReadOnlyList<TInto>?)list);

    public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? list) =>
        list ?? Array.Empty<T>();

    public static List<T>? NullIfEmpty<T>(this List<T> list) =>
        list.Count != 0 ? list : null;
}

internal static class DictionaryExtensions
{
    public static IReadOnlyDictionary<String, IReadOnlyList<TInto>> EmptyIfNull<TInto, TFrom>(
        this Dictionary<String, List<TFrom>?>? dictionary,
        Action<String, List<TFrom>?>? transformer = null)
        where TFrom : TInto =>
        dictionary?.ToDictionary(
            _ => _.Key,
            _ =>
            {
                transformer?.Invoke(_.Key, _.Value);
                return _.Value.EmptyIfNull<TInto, TFrom>();
            },
            StringComparer.Ordinal)
        ?? new Dictionary<String, IReadOnlyList<TInto>>(StringComparer.Ordinal);
}
