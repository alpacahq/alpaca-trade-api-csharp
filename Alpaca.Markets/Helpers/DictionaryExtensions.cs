using Newtonsoft.Json.Linq;

namespace Alpaca.Markets;

internal static class DictionaryExtensions
{
    private static IEnumerable<String> getConditions(
        Object? conditions) =>
        conditions switch
        {
            JArray jArray => jArray.SelectMany(getConditions),
            JValue jValue => getConditions(jValue.Value),
            String value => Enumerable.Repeat(value, 1),
            _ => []
        };

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

    public static IEnumerable<String> GetConditions(
        this IReadOnlyDictionary<String, Object> extensionData) =>
        extensionData.TryGetValue("c", out var conditions)
            ? getConditions(conditions) : [];
}
