#if NETFRAMEWORK || NETSTANDARD2_0

namespace System;

internal static class KeyValuePairExtensions
{
    [UsedImplicitly]
    public static void Deconstruct<TKey, TValue>(
        // ReSharper disable once UseDeconstructionOnParameter
        this KeyValuePair<TKey, TValue> pair,
        out TKey key,
        out TValue value)
    {
        key = pair.Key;
        value = pair.Value;
    }
}

#endif
