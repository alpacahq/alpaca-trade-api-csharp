using System;
 
namespace Alpaca.Markets
{
    internal sealed class AlpacaDataSubscription<TApi> : IAlpacaDataSubscription<TApi>
        where TApi : IStreamBase
    {
        internal AlpacaDataSubscription(
            String channelName,
            String symbol)
        {
            Stream = $"{channelName}.{symbol}";
        }

        public String Stream { get; }

        public Boolean Subscribed { get; private set; }

        public event Action<TApi>? Received;

        internal void OnReceived(TApi update) => Received?.Invoke(update);

        public void OnUpdate() => Subscribed = true;
    }
}
