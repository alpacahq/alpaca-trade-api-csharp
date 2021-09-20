using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    internal interface IPageMutable<TItems> : IPage<TItems>
    {
        public new String? NextPageToken { set; }

        public new IReadOnlyList<TItems> Items { set; }
    }
}
