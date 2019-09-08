using System;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    internal static class ActionExtensions
    {
        public static void DeserializeAndInvoke<TApi, TJson>(
            this Action<TApi> eventHandler,
            JToken eventArg)
            where TJson : class, TApi
        {
            eventHandler?.Invoke(eventArg.ToObject<TJson>());
        }
    }
}
