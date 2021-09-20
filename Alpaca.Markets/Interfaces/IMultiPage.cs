using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates single page response in Alpaca Data API v2.
    /// </summary>
    /// <typeparam name="TItems">Type of paged item (bar, trade or quote)</typeparam>
    public interface IMultiPage<TItems>
    {
        /// <summary>
        /// Gets the next page token for continuation. If value of this property
        /// equals to <c>null</c> this page is the last one and no more data available.
        /// </summary>
        [UsedImplicitly]
        public String? NextPageToken { get; }

        /// <summary>
        /// Gets list of items for this response grouped by symbols.
        /// </summary>
        public IReadOnlyDictionary<String, IReadOnlyList<TItems>> Items { get; }
    }
}
