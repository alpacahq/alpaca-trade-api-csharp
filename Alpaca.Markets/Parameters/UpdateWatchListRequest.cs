using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.UpdateWatchListByIdAsync(UpdateWatchListRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class UpdateWatchListRequest : Validation.IRequest
    {
        private readonly List<String> _assets = new List<String>();

        /// <summary>
        /// Creates new instance of <see cref="UpdateWatchListRequest"/> object.
        /// </summary>
        /// <param name="watchListId">Unique watch list identifier.</param>
        /// <param name="name">User defined watch list name.</param>
        /// <param name="assets">List of asset names for new watch list.</param>
        public UpdateWatchListRequest(
            Guid watchListId,
            String name,
            IEnumerable<String> assets)
        {
            WatchListId = watchListId;
            Name = name;
            _assets.AddRange(
                (assets ?? Enumerable.Empty<String>())
                .Distinct(StringComparer.Ordinal));
        }

        /// <summary>
        /// Gets the target watch list unique identifier.
        /// </summary>
        public Guid WatchListId { get; }

        /// <summary>
        /// Gets the target watch list name.
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets list of asset names for new watch list.
        /// </summary>
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
