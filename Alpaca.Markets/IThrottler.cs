﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    internal interface IThrottler
    {
        /// <summary>
        /// Gets flag indicating we are currently being rate limited.
        /// </summary>
        Int32 MaxRetryAttempts { get; set;  }

        /// <summary>
        /// Gets list of Http status codes which when recieved should initiate a retry of the affected request
        /// </summary>
        HashSet<Int32> RetryHttpStatuses { get; set; }

        /// <summary>
        /// Blocks the current thread indefinitely until allowed to proceed.
        /// </summary>
        Task WaitToProceed();

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
        Boolean CheckHttpResponse(HttpResponseMessage response);
    }
}