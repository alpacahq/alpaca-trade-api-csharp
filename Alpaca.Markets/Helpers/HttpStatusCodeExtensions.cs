using System;
using System.Net;

namespace Alpaca.Markets
{
    internal static class HttpStatusCodeExtensions
    {
        public static Boolean IsSuccessHttpStatusCode(
            this HttpStatusCode httpStatusCode) =>
            httpStatusCode >= HttpStatusCode.OK &&
            httpStatusCode < HttpStatusCode.Ambiguous;

        public static Boolean IsSuccessHttpStatusCode(
            this Int64 httpStatusCode) =>
            IsSuccessHttpStatusCode((HttpStatusCode) httpStatusCode);
    }
}
