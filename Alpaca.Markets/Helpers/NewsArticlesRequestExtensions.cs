using System;
using JetBrains.Annotations;

namespace Alpaca.Markets
{
    /// <summary>
    /// Set of extension methods for <see cref="NewsArticlesRequest"/> initialization.
    /// </summary>
    public static class NewsArticlesRequestExtensions
    {
        /// <summary>
        /// Sets the request page size using the fluent interface approach.
        /// </summary>
        /// <param name="request">Request parameters object.</param>
        /// <param name="pageSize">The request page size.</param>
        /// <returns>The original request parameters object.</returns>
        [UsedImplicitly]
        [CLSCompliant(false)]
        public static NewsArticlesRequest WithPageSize(
            this NewsArticlesRequest request,
            UInt32 pageSize)
        {
            request.EnsureNotNull(nameof(request)).Pagination.Size = pageSize;
            return request;
        }

        /// <summary>
        /// Sets the request page size using the fluent interface approach.
        /// </summary>
        /// <param name="request">Request parameters object.</param>
        /// <param name="pageToken">The request page size.</param>
        /// <returns>The original request parameters object.</returns>
        [UsedImplicitly]
        public static NewsArticlesRequest WithPageToken(
            this NewsArticlesRequest request,
            String pageToken)
        {
            request.EnsureNotNull(nameof(request)).Pagination.Token = pageToken;
            return request;
        }
    }
}
