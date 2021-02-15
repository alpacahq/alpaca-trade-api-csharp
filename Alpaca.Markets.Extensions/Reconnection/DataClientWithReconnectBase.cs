using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Alpaca.Markets.Extensions
{
    internal abstract class DataClientWithReconnectBase<TClient> :
        ClientWithReconnectBase<TClient>, IStreamingDataClient
        where TClient : IStreamingDataClient
    {
        private readonly ConcurrentDictionary<String, IAlpacaDataSubscription> _subscriptions =
            new ConcurrentDictionary<String, IAlpacaDataSubscription>(StringComparer.Ordinal);

        protected DataClientWithReconnectBase(
            TClient client,
            ReconnectionParameters reconnectionParameters)
            : base(client, reconnectionParameters) { }
        
        public void Subscribe(
            IAlpacaDataSubscription subscription)
        {
            foreach (var stream in subscription.Streams)
            {
                _subscriptions.TryAdd(stream, subscription);
            }
            Client.Subscribe(subscription);
        }

        public void Subscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) => 
            Subscribe(subscriptions.ToArray());

        public void Subscribe(
            params IAlpacaDataSubscription[] subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryAdd(stream, subscription);
                }
            }

            Client.Subscribe(subscriptions);
        }

        public void Unsubscribe(
            IAlpacaDataSubscription subscription)
        {
            foreach (var stream in subscription.Streams)
            {
                _subscriptions.TryRemove(stream, out _);
            }
            Client.Unsubscribe(subscription);
        }

        public void Unsubscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            Unsubscribe(subscriptions.ToArray());

        public void Unsubscribe(
            params IAlpacaDataSubscription[] subscriptions)
        {
            foreach (var subscription in subscriptions)
            {
                foreach (var stream in subscription.Streams)
                {
                    _subscriptions.TryRemove(stream, out _);
                }
            }

            Client.Unsubscribe(subscriptions);
        }

        protected override void OnReconnection(
            CancellationToken cancellationToken)
        {
            foreach (var subscription in _subscriptions.Values)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Client.Subscribe(subscription);
            }
        }
    }
}