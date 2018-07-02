using System;
using System.Collections.Generic;
using System.IO;
using NATS.Client;
using Newtonsoft.Json;

namespace Alpaca.Markets
{
    public sealed class NatsClient : IDisposable
    {
        private readonly IDictionary<String, IAsyncSubscription> _subscriptions =
            new Dictionary<String, IAsyncSubscription>(StringComparer.Ordinal);

        private readonly Options _options;

        private IConnection _connection;

        public NatsClient(
            String keyId)
        {
            _options = ConnectionFactory.GetDefaultOptions();
            _options.MaxReconnect = 3;
            _options.Servers = new[]
            {
                $"nats://{keyId}@nats1.polygon.io:31101",
                $"nats://{keyId}@nats2.polygon.io:31102",
                $"nats://{keyId}@nats3.polygon.io:31103"
            };

            _options.AsyncErrorEventHandler += (sender, args) => OnError?.Invoke(args.Error);
        }

        public event Action<IStreamTrade> TradeReceived;

        public event Action<IStreamQuote> QuoteReceived;

        public event Action<IStreamBar> BarReceived;

        public event Action<String> OnError;

        public void Open()
        {
            _connection = new ConnectionFactory()
                .CreateConnection(_options);
        }

        public void SubscribeTrade(
            String symbol)
        {
            subscribe($"T.{symbol}", handleTradeMessage);
        }

        public void SubscribeQuote(
            String symbol)
        {
            subscribe($"Q.{symbol}", handleQuoteMessage);
        }

        public void SubscribeSecondBar(
            String symbol)
        {
            subscribe($"A.{symbol}", handleBarMessage);
        }

        public void SubscribeMinuteBar(
            String symbol)
        {
            subscribe($"AM.{symbol}", handleBarMessage);
        }

        public void UnsubscribeTrade(
            String symbol)
        {
            unsubscribe($"T.{symbol}");
        }

        public void UnsubscribeQuote(
            String symbol)
        {
            unsubscribe($"Q.{symbol}");
        }

        public void UnsubscribeSecondBar(
            String symbol)
        {
            unsubscribe($"A.{symbol}");
        }

        public void UnsubscribeMinuteBar(
            String symbol)
        {
            unsubscribe($"AM.{symbol}");
        }

        public void Close()
        {
            foreach (var subscription in _subscriptions.Values)
            {
                subscription?.Unsubscribe();
            }

            _connection?.Close();
        }

        public void Dispose()
        {
            foreach (var subscription in _subscriptions.Values)
            {
                subscription?.Dispose();
            }

            _connection?.Dispose();
        }

        private void subscribe(
            String topic,
            EventHandler<MsgHandlerEventArgs> handler)
        {
            var subscription = _connection?.SubscribeAsync(topic, handler);

            if (subscription != null)
            {
                _subscriptions[topic] = subscription;
            }
        }

        private void unsubscribe(
            String topic)
        {
            if (_subscriptions.TryGetValue(
                topic, out var subscription))
            {
                subscription?.Unsubscribe();
                subscription?.Dispose();
            }
        }

        private void handleTradeMessage(
            Object sender,
            MsgHandlerEventArgs eventArgs)
        {
            var message = deserializeBytes<JsonStreamTrade>(
                eventArgs.Message.Data);
            TradeReceived?.Invoke(message);
        }

        private void handleQuoteMessage(
            Object sender,
            MsgHandlerEventArgs eventArgs)
        {
            var message = deserializeBytes<JsonStreamQuote>(
                eventArgs.Message.Data);
            QuoteReceived?.Invoke(message);
        }

        private void handleBarMessage(
            Object sender,
            MsgHandlerEventArgs eventArgs)
        {
            var message = deserializeBytes<JsonStreamBar>(
                eventArgs.Message.Data);
            BarReceived?.Invoke(message);
        }

        private static T deserializeBytes<T>(
            Byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            using (var textReader = new StreamReader(stream))
            using (var jsonreader = new JsonTextReader(textReader))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize<T>(jsonreader);
            }
        }
    }
}
