#if NETFRAMEWORK || NETSTANDARD2_0

namespace System;

internal readonly struct Range : IEquatable<Range>
{
    [UsedImplicitly]
    public Index Start { get; }

    [UsedImplicitly]
    public Index End { get; }

    public Range(
        Index start,
        Index end)
    {
        Start = start;
        End = end;
    }

    public override Boolean Equals(Object? value) =>
        value is Range r &&
        r.Start.Equals(Start) &&
        r.End.Equals(End);

    public Boolean Equals(Range other) => other.Start.Equals(Start) && other.End.Equals(End);

    public override Int32 GetHashCode() => Start.GetHashCode() * 31 + End.GetHashCode();

    public override String ToString() => Start + ".." + End;

    [UsedImplicitly]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public (Int32 Offset, Int32 Length) GetOffsetAndLength(Int32 length)
    {
        var startIndex = Start;
        var start = startIndex.IsFromEnd ? length - startIndex.Value : startIndex.Value;

        var endIndex = End;
        var end = endIndex.IsFromEnd ? length - endIndex.Value : endIndex.Value;

        if ((UInt32)end > (UInt32)length || (UInt32)start > (UInt32)end)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }

        return (start, end - start);
    }
}

#endif
