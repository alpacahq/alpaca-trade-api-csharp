using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.CreateWatchListAsync(NewWatchListRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class NewWatchListRequest : Validation.IRequest
    {
        private readonly List<String> _assets = new List<String>();

        /// <summary>
        /// Creates new instance of <see cref="NewWatchListRequest"/> object.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        public NewWatchListRequest(String name) => Name = name;

        /// <summary>
        /// Creates new instance of <see cref="NewWatchListRequest"/> object.
        /// </summary>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        public NewWatchListRequest(
            String name,
            IEnumerable<String> assets)
            : this(name) => 
            _assets.AddRange(
                // ReSharper disable once ConstantNullCoalescingCondition
                (assets ?? Enumerable.Empty<String>())
                .Distinct(StringComparer.Ordinal));

        /// <summary>
        /// Gets user defined watch list name.
        /// </summary>
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public String Name { get; }

        /// <summary>
        /// Gets list of asset names for new watch list.
        /// </summary>
        [JsonProperty(PropertyName = "symbols", Required = Required.Always)]
        public IReadOnlyList<String> Assets => _assets;
       
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (Name.IsWatchListNameInvalid())
            {
                yield return new RequestValidationException(
                    "Watch list name should be from 1 to 64 characters length.", nameof(Name));
            }

            if (Assets.Any(String.IsNullOrEmpty))
            {
                yield return new RequestValidationException(
                    "Assets list shouldn't contain null or empty items.", nameof(Assets));
            }
        }
    }
}
