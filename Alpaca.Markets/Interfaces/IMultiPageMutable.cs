using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    internal interface IMultiPageMutable<TItems> : IMultiPage<TItems>
    {
        public new String? NextPageToken { set; }

        public new IReadOnlyDictionary<String, IReadOnlyList<TItems>> Items { set; }
    }
}
