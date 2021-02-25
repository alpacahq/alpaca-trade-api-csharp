using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    public static class HistoricalRequestBaseExtensions
    {
        /// <summary>
        /// Sets the request page size using the fluent interface approach.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pageSize">The request page size.</param>
        /// <returns>The original request parameters object.</returns>
        public static TRequest WithPageSize<TRequest>(
            this TRequest request,
            UInt32 pageSize)
            where TRequest: HistoricalRequestBase
        {
            request.Pagination.Size = pageSize;
            return request;
        }

        /// <summary>
        /// Sets the request page size using the fluent interface approach.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pageToken">The request page size.</param>
        /// <returns>The original request parameters object.</returns>
        public static TRequest WithPageToken<TRequest>(
            this TRequest request,
            String pageToken)
            where TRequest: HistoricalRequestBase
        {
            request.Pagination.Token = pageToken;
            return request;
        }
    }
}
