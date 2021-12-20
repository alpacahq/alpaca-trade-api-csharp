using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets
{

    internal static class DictionaryExtensions
    {
        public static IReadOnlyDictionary<String, IReadOnlyList<TInto>> SetSymbol<TInto, TFrom>(
            this Dictionary<String, List<TFrom>?>? dictionary)
            where TFrom : TInto, ISymbolMutable =>
            dictionary?.ToDictionary(
                _ => _.Key,
                _ => _.Value.SetSymbol(_.Key).EmptyIfNull<TInto, TFrom>(),
                StringComparer.Ordinal)
            ?? new Dictionary<String, IReadOnlyList<TInto>>(StringComparer.Ordinal);
    }
}
