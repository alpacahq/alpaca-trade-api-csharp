#if NETFRAMEWORK || NETSTANDARD2_0

namespace System;

internal readonly struct Index : IEquatable<Index>
{
    private readonly Int32 _value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Index(
        Int32 value, Boolean fromEnd = false) =>
        _value = fromEnd ? ~value : value;

    public Index(Int32 value) => _value = value;

    [UsedImplicitly]
    public Int32 Value => _value < 0 ? ~_value : _value;

    [UsedImplicitly]
    public Boolean IsFromEnd => _value < 0;

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Int32 GetOffset(Int32 length) =>
        IsFromEnd ? _value + length + 1 : _value;

    public override Boolean Equals(Object? value) =>
        value is Index index && _value == index._value;

    public Boolean Equals(Index other) =>
        _value == other._value;

    public override Int32 GetHashCode() => _value;

    public static implicit operator Index(Int32 value) => new(value);
}

#endif
