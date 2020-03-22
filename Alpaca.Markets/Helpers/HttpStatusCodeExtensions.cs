using System;
using System.Net;

namespace Alpaca.Markets
{
    internal static class HttpStatusCodeExtensions
    {
        private static Boolean isSuccessHttpStatusCode(
            this HttpStatusCode httpStatusCode) =>
            httpStatusCode >= HttpStatusCode.OK &&
            httpStatusCode < HttpStatusCode.Ambiguous;

        public static Boolean IsSuccessHttpStatusCode(
            this Int64 httpStatusCode) =>
            isSuccessHttpStatusCode((HttpStatusCode) httpStatusCode);
    }
}
