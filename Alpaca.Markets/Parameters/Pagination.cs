using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates all data required for the pagination support in Alpaca Data API v2
    /// </summary>
    public sealed class Pagination : Validation.IRequest
    {
        private const UInt32 MaxPageSize = 10_000;

        private const UInt32 MinPageSize = 1;

        /// <summary>
        /// Gets and sets the request page size. If equals to <c>null</c> default size will be used.
        /// </summary>
        [CLSCompliant(false)]
        public UInt32? Size { get; set; }

        /// <summary>
        /// Gets and sets the page token for the request. Should be <c>null</c> for the first request.
        /// </summary>
        public String? Token { get; set; }

        internal QueryBuilder QueryBuilder =>
            new QueryBuilder()
                .AddParameter("page_token", Token)
                .AddParameter("limit", Size);

        /// <inheritdoc />
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (Size < MinPageSize || Size > MaxPageSize)
            {
                yield return new RequestValidationException("Invalid page size");
            }
        }
    }
}
