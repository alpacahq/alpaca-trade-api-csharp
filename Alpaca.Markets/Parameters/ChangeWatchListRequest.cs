using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public sealed class ChangeWatchListRequest<TKey> : Validation.IRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Unique watch list identifier or name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        public ChangeWatchListRequest(
            TKey key,
            String asset)
        {
            Key = key;
            Asset = asset;
        }

        /// <summary>
        /// Gets unique watch list identifier or name.
        /// </summary>
        public TKey Key { get; }

        /// <summary>
        /// Gets asset name for adding/deleting into watch list.
        /// </summary>
        public String Asset { get; }
       
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (Key is String name &&
                !name.IsWatchListNameValid())
            {
                yield return new RequestValidationException(
                    "Watch list name should be from 1 to 64 characters length.", nameof(Key));
            }

            if (String.IsNullOrEmpty(Asset))
            {
                yield return new RequestValidationException(
                    "Asset name should be specified.", nameof(Asset));
            }
        }
    }
}
