namespace Alpaca.Markets;

internal static class DictionaryExtensions
{
    public static IReadOnlyDictionary<String, IReadOnlyList<TInto>> SetSymbol<TInto, TFrom>(
        this Dictionary<String, List<TFrom>?>? dictionary)
        where TFrom : TInto, ISymbolMutable =>
        dictionary?.ToDictionary(
            pair => pair.Key,
            pair => pair.Value.SetSymbol(pair.Key).EmptyIfNull<TInto, TFrom>(),
            StringComparer.Ordinal)
        ?? new Dictionary<String, IReadOnlyList<TInto>>(StringComparer.Ordinal);

#if NETFRAMEWORK || NETSTANDARD2_0
    public static TValue GetValueOrDefault<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> dictionary,
        TKey key, TValue defaultValue) =>
        dictionary.TryGetValue(key, out var value) ? value : defaultValue;
#endif
}
