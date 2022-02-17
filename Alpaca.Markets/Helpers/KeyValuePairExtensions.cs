#if NETFRAMEWORK || NETSTANDARD2_0

namespace Alpaca.Markets;

internal static class KeyValuePairExtensions
{
    public static void Deconstruct<TKey, TValue>(
        // ReSharper disable once UseDeconstructionOnParameter
        this KeyValuePair<TKey, TValue> keyValuePair,
        out TKey key,
        out TValue value)
    {
        key = keyValuePair.Key;
        value = keyValuePair.Value;
    }
}

#endif
