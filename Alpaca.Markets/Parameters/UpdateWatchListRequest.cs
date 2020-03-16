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
        /// 
        /// </summary>
        public Guid WatchListId { get; }

        /// <summary>
        /// 
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// Gets list of asset names for new watch list.
        /// </summary>
        public IReadOnlyList<String> Assets => _assets;
       
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            // TODO: olegra - add more validations here
            yield break;
        }

    }
}
