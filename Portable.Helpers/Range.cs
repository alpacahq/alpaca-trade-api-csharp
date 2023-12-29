#if NETFRAMEWORK || NETSTANDARD2_0

namespace System;

internal readonly record struct Range(
    Index Start, Index End)
{
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
