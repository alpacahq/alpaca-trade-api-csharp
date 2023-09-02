using Newtonsoft.Json.Linq;

namespace Alpaca.Markets;

internal static class ActionExtensions
{
    public static void DeserializeAndInvoke<TApi, TJson>(
        this Action<TApi>? eventHandler,
        JToken eventArg)
        where TJson : class, TApi =>
        eventHandler?.Invoke(eventArg.ToObject<TJson>() ?? throw new RestClientErrorException(
            "Unable to deserialize JSON response message."));
}
