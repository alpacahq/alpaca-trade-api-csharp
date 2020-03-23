using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    [SuppressMessage(
        "Globalization","CA1303:Do not pass literals as localized parameters",
        Justification = "We do not plan to support localized exception messages in this SDK.")]
    internal static class ActionExtensions
    {
        public static void DeserializeAndInvoke<TApi, TJson>(
            this Action<TApi>? eventHandler,
            JToken eventArg)
            where TJson : class, TApi =>
            eventHandler?.Invoke(eventArg.ToObject<TJson>() ??
                                 throw new RestClientErrorException("Unable to deserialize JSON response message."));
    }
}
