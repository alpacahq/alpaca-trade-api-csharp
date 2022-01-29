namespace Alpaca.Markets.Extensions;

#if !NETSTANDARD2_1 && !NET5_0_OR_GREATER
internal static class KeyValuePairExtensions
{
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
