﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal interface IThrottler
    {
        /// <summary>
        /// Gets maximal number of retry attempts for single request.
        /// </summary>
        Int32 MaxRetryAttempts { get; }

        /// <summary>
        /// Blocks the current thread indefinitely until allowed to proceed.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        Task WaitToProceed(
            CancellationToken cancellationToken);

        /// <summary>
        /// Evaluates the StatusCode of <paramref name="response"/>, initiates any server requested delays, 
        /// and returns false to indicate when a client should retry a request
        /// </summary>
        /// <param name="response">Server response to an Http request</param>
        /// <returns>False indicates that client should retry the request.
        /// True indicates that StatusCode was HttpStatusCode.OK.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// The HTTP response is unsuccessful, and caller did not indicate that requests with this StatusCode should be retried.
        /// </exception>
        Boolean CheckHttpResponse(
            HttpResponseMessage response);
    }
}
