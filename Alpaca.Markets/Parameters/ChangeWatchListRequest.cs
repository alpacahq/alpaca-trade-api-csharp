using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for
    /// <see cref="AlpacaTradingClient.AddAssetIntoWatchListByIdAsync(ChangeWatchListRequest{Guid},System.Threading.CancellationToken)"/>,
    /// <see cref="AlpacaTradingClient.AddAssetIntoWatchListByNameAsync(ChangeWatchListRequest{String},System.Threading.CancellationToken)"/>,
    /// <see cref="AlpacaTradingClient.DeleteAssetFromWatchListByIdAsync(ChangeWatchListRequest{Guid},System.Threading.CancellationToken)"/>, and
    /// <see cref="AlpacaTradingClient.DeleteAssetFromWatchListByNameAsync(ChangeWatchListRequest{String},System.Threading.CancellationToken)"/>
    /// calls.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public sealed class ChangeWatchListRequest<TKey> : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="ChangeWatchListRequest{TKey}"/> object.
        /// </summary>
        /// <param name="key">Unique watch list identifier or name.</param>
        /// <param name="asset">Asset name for adding into watch list.</param>
        public ChangeWatchListRequest(
            TKey key,
            String asset)
        {
            Key = key;
            Asset = asset ?? throw new ArgumentException(
                "Asset name cannot be null.", nameof(asset));
        }

        /// <summary>
        /// Gets unique watch list identifier or name.
        /// </summary>
        [JsonIgnore]
        public TKey Key { get; }

        /// <summary>
        /// Gets asset name for adding/deleting into watch list.
        /// </summary>
        [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
        public String Asset { get; }
       
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
#pragma warning disable CA1508 // Avoid dead conditional code
            if (Key is String name &&
#pragma warning restore CA1508 // Avoid dead conditional code
                name.IsWatchListNameInvalid())
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
