using System.Net;

namespace Alpaca.Markets;

internal static class HttpStatusCodeExtensions
{
    private static Boolean isSuccessHttpStatusCode(
        this HttpStatusCode httpStatusCode) =>
        httpStatusCode is >= HttpStatusCode.OK and < HttpStatusCode.Ambiguous;

    public static Boolean IsSuccessHttpStatusCode(
        this Int64 httpStatusCode) =>
        isSuccessHttpStatusCode((HttpStatusCode)httpStatusCode);
}
