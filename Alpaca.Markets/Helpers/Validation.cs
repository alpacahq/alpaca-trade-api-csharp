using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    internal static class Validation
    {
        internal interface IRequest
        {
            IEnumerable<RequestValidationException> GetExceptions();
        }

        public static void Validate<TRequest>(this TRequest request)
            where TRequest : class, IRequest
        {
            var exception = new AggregateException(request.GetExceptions());
            if (exception.InnerExceptions.Count != 0)
            {
                throw exception;
            }
        }
    }
}
