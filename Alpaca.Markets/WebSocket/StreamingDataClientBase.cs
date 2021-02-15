using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConfiguration"></typeparam>
    public abstract class StreamingDataClientBase<TConfiguration>
        : StreamingClientBase<TConfiguration>, IStreamingDataClient
        where TConfiguration : StreamingClientConfiguration
    {
        private interface ISubscription
        {
            void OnUpdate();

            void OnReceived(
                JToken token);
        }

        private sealed class AlpacaDataSubscription<TApi, TJson>
            : IAlpacaDataSubscription<TApi>, ISubscription
            where TJson : class, TApi
            where TApi : IStreamBase
        {
            internal AlpacaDataSubscription(
                String stream) =>
                Stream = stream;

            public String Stream { get; }

            public IEnumerable<String> Streams
            {
                get { yield return Stream; }
            }

            public Boolean Subscribed { get; private set; }

            public event Action<TApi>? Received;

            public void OnReceived(
                JToken token) =>
                Received?.Invoke(token.ToObject<TJson>()
                                 ?? throw new RestClientErrorException());

            public void OnUpdate() => Subscribed = !Subscribed;
        }

        private sealed class Subscriptions
        {
            private readonly ConcurrentDictionary<String, ISubscription> _subscriptions =
                new ConcurrentDictionary<String, ISubscription>(StringComparer.Ordinal);

            public IAlpacaDataSubscription<TApi> GetOrAdd<TApi, TJson>(
                String stream,
                String key)
                where TJson : class, TApi
                where TApi : IStreamBase =>
                (IAlpacaDataSubscription<TApi>) _subscriptions.GetOrAdd(
                    key, _ => new AlpacaDataSubscription<TApi, TJson>(stream));

            public void OnUpdate(String stream)
            {
                if (_subscriptions.TryGetValue(stream, out var subscription))
                {
                    subscription.OnUpdate();
                }
            }

            public Boolean OnReceived(
                String stream,
                JToken token)
            {
                var found = false;

                if (_subscriptions.TryGetValue(token["ev"]?.ToString() ?? String.Empty, out var subscription))
                {
                    subscription.OnReceived(token);
                    found = true;
                }

                if (_subscriptions.TryGetValue(stream, out subscription))
                {
                    subscription.OnReceived(token);
                    found = true;
                }

                return found;
            }
        }

        internal const String TradesChannel = "T";

        internal const String QuotesChannel = "Q";

        internal const String SecondAggChannel = "A";

        internal const String MinuteAggChannel = "AM";

        private readonly Subscriptions _subscriptions = new Subscriptions();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        protected StreamingDataClientBase(
            TConfiguration configuration)
            : base(configuration.EnsureNotNull(nameof(configuration)))
        {
        }

        /// <inheritdoc />
        public void Subscribe(
            IAlpacaDataSubscription subscription) =>
            Subscribe(subscription.EnsureNotNull(nameof(subscription)).Streams);

        /// <inheritdoc />
        public void Subscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Subscribe(subscriptions.AsEnumerable());

        /// <inheritdoc />
        public void Subscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            Subscribe(subscriptions.SelectMany(_ => _.Streams));

        /// <inheritdoc />
        public void Unsubscribe(
            IAlpacaDataSubscription subscription) =>
            Unsubscribe(subscription.EnsureNotNull(nameof(subscription)).Streams);

        /// <inheritdoc />
        public void Unsubscribe(
            params IAlpacaDataSubscription[] subscriptions) =>
            Unsubscribe(subscriptions.AsEnumerable());

        /// <inheritdoc />
        public void Unsubscribe(
            IEnumerable<IAlpacaDataSubscription> subscriptions) =>
            Unsubscribe(subscriptions.SelectMany(_ => _.Streams));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="symbol"></param>
        /// <typeparam name="TApi"></typeparam>
        /// <typeparam name="TJson"></typeparam>
        /// <returns></returns>
        protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
            String channelName,
            String symbol)
            where TJson : class, TApi
            where TApi : IStreamBase =>
            _subscriptions.GetOrAdd<TApi, TJson>(
                channelName, $"{channelName}.{symbol}");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelName"></param>
        /// <typeparam name="TApi"></typeparam>
        /// <typeparam name="TJson"></typeparam>
        /// <returns></returns>
        protected IAlpacaDataSubscription<TApi> GetSubscription<TApi, TJson>(
            String channelName)
            where TJson : class, TApi
            where TApi : IStreamBase =>
            _subscriptions.GetOrAdd<TApi, TJson>(
                channelName, $"{channelName}.*");

                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [SuppressMessage(
            "Design", "CA1031:Do not catch general exception types",
            Justification = "Expected behavior - we report exceptions via OnError event.")]
        protected Boolean HandleRealtimeDataUpdate(
            String stream,
            JToken token)
        {
            try
            {
                return _subscriptions.OnReceived(
                    stream, token.EnsureNotNull(nameof(token)));
            }
            catch (Exception exception)
            {
                HandleError(exception);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected void SubscriptionsOnUpdate(
            String stream) =>
            _subscriptions.OnUpdate(stream);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streams"></param>
        protected abstract void Subscribe(
            IEnumerable<String> streams);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streams"></param>
        protected abstract void Unsubscribe(
            IEnumerable<String> streams);
    }
}
