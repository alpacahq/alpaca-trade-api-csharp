using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    internal static class Validation
    {
        private const Int32 ClientOrderIdMaxLength = 128;

        private const Int32 WatchListNameMaxLength = 64;

        internal interface IRequest
        {
            /// <summary>
            /// Gets all validation exceptions (inconsistent request data errors).
            /// </summary>
            /// <returns>Lazy-evaluated list of validation errors.</returns>
            IEnumerable<RequestValidationException> GetExceptions();
        }

        public static TRequest Validate<TRequest>(this TRequest request)
            where TRequest : class, IRequest
        {
            var exception = new AggregateException(request.GetExceptions());
            if (exception.InnerExceptions.Count != 0)
            {
                throw exception.InnerExceptions.Count == 1
                    ? exception.InnerExceptions[0]
                    : exception;
            }

            return request;
        }

        public static String? ValidateClientOrderId(this String? clientOrderId) => 
            clientOrderId?.Length > ClientOrderIdMaxLength
                ? clientOrderId.Substring(0, ClientOrderIdMaxLength) 
                : clientOrderId;

        public static Boolean IsWatchListNameInvalid(this String? watchListName) =>
            watchListName == null ||
#pragma warning disable CA1508 // Avoid dead conditional code
            String.IsNullOrEmpty(watchListName) ||
#pragma warning restore CA1508 // Avoid dead conditional code
            watchListName.Length > WatchListNameMaxLength;

        public static String? ValidateWatchListName(this String? watchListName) =>
            IsWatchListNameInvalid(watchListName) ? null : watchListName;
    }
}
