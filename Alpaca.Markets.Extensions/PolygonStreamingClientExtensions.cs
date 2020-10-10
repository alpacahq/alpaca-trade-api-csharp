using System;
using System.Collections.Generic;

namespace Alpaca.Markets.Extensions
{
    /// <summary>
    /// Helper extension method for creating special version of the <see cref="IPolygonStreamingClient"/>
    /// implementation with automatic reconnection (with configurable delay and number of attempts) support.
    /// </summary>
    public static class PolygonStreamingClientExtensions
    {
        [Flags]
        private enum SubscriptionTypes
        {
            None = 0x00,
            Trade = 0x01,
            Quote = 0x02,
            Second = 0x04,
            Minute = 0x08
        }

        private sealed class ClientWithReconnection :
            ClientWithReconnectBase<IPolygonStreamingClient, SubscriptionTypes>,
            IPolygonStreamingClient
        {
            public ClientWithReconnection(
                IPolygonStreamingClient client, 
                ReconnectionParameters reconnectionParameters)
                : base(client, reconnectionParameters)
            {
            }

            public void SubscribeTrade(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Trade,
                    (_, subscription) => subscription | SubscriptionTypes.Trade);
                _client.SubscribeTrade(symbol);
            }

            public void SubscribeQuote(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Quote,
                    (_, subscription) => subscription | SubscriptionTypes.Quote);
                _client.SubscribeQuote(symbol);
            }

            public void SubscribeSecondAgg(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Second,
                    (_, subscription) => subscription | SubscriptionTypes.Second);
                _client.SubscribeSecondAgg(symbol);
            }

            public void SubscribeMinuteAgg(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Minute,
                    (_, subscription) => subscription | SubscriptionTypes.Minute);
                _client.SubscribeMinuteAgg(symbol);
            }

            public void SubscribeTrade(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    SubscribeTrade(symbol);
                }
            }

            public void SubscribeQuote(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    SubscribeQuote(symbol);
                }
            }

            public void SubscribeSecondAgg(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    SubscribeSecondAgg(symbol);
                }
            }

            public void SubscribeMinuteAgg(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    SubscribeMinuteAgg(symbol);
                }
            }

            public void UnsubscribeTrade(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Trade);
                _client.UnsubscribeTrade(symbol);
            }

            public void UnsubscribeQuote(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Quote);
                _client.UnsubscribeQuote(symbol);
            }

            public void UnsubscribeSecondAgg(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Second);
                _client.UnsubscribeSecondAgg(symbol);
            }

            public void UnsubscribeMinuteAgg(String symbol)
            {
                _subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Minute);
                _client.UnsubscribeMinuteAgg(symbol);
            }

            public void UnsubscribeTrade(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    UnsubscribeTrade(symbol);
                }
            }

            public void UnsubscribeQuote(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    UnsubscribeQuote(symbol);
                }
            }

            public void UnsubscribeSecondAgg(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    UnsubscribeSecondAgg(symbol);
                }
            }

            public void UnsubscribeMinuteAgg(IEnumerable<String> symbols)
            {
                foreach (var symbol in symbols)
                {
                    UnsubscribeMinuteAgg(symbol);
                }
            }

            public event Action<IStreamTrade>? TradeReceived
            {
                add => _client.TradeReceived += value;
                remove => _client.TradeReceived -= value;
            }

            public event Action<IStreamQuote>? QuoteReceived
            {
                add => _client.QuoteReceived += value;
                remove => _client.QuoteReceived -= value;
            }

            public event Action<IStreamAgg>? MinuteAggReceived
            {
                add => _client.MinuteAggReceived += value;
                remove => _client.MinuteAggReceived -= value;
            }

            public event Action<IStreamAgg>? SecondAggReceived
            {
                add => _client.SecondAggReceived += value;
                remove => _client.SecondAggReceived -= value;
            }

            protected override void Resubscribe(
                String symbol, 
                SubscriptionTypes subscription)
            {
                if ((subscription & SubscriptionTypes.Trade) == SubscriptionTypes.Trade)
                {
                    SubscribeTrade(symbol);
                }
                if ((subscription & SubscriptionTypes.Quote) == SubscriptionTypes.Quote)
                {
                    SubscribeQuote(symbol);
                }
                if ((subscription & SubscriptionTypes.Second) == SubscriptionTypes.Second)
                {
                    SubscribeSecondAgg(symbol);
                }
                if ((subscription & SubscriptionTypes.Minute) == SubscriptionTypes.Minute)
                {
                    SubscribeMinuteAgg(symbol);
                }
            }
        }

        /// <summary>
        /// Wraps instance of <see cref="IPolygonStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IPolygonStreamingClient WithReconnect(
            this IPolygonStreamingClient client,
            ReconnectionParameters? parameters = null) =>
            new ClientWithReconnection(client, parameters ?? ReconnectionParameters.Default);
    }
}
