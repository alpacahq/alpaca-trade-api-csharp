using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    internal static class ListExtensions
    {
        public static IReadOnlyList<TInto> EmptyIfNull<TInto, TFrom>(this List<TFrom>? list)
            where TFrom : TInto => EmptyIfNull((IReadOnlyList<TInto>?) list);

        public static IReadOnlyList<T> EmptyIfNull<T>(this IReadOnlyList<T>? list) =>
            list ?? Array.Empty<T>();

        public static List<T>? NullIfEmpty<T>(this List<T> list) =>
            list.Count != 0 ? list : null;
    }
}
