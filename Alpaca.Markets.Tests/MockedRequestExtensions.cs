using Newtonsoft.Json;
using RichardSzalay.MockHttp;

namespace Alpaca.Markets.Tests;

internal static class MockedRequestExtensions
{
    public static void RespondJson<TJson>(
        this MockedRequest request,
        TJson response) =>
        request.Respond(
            "application/json",
            JsonConvert.SerializeObject(response));
}
