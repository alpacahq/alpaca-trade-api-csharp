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
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Trade,
                    (_, subscription) => subscription | SubscriptionTypes.Trade);
                Client.SubscribeTrade(symbol);
            }

            public void SubscribeQuote(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Quote,
                    (_, subscription) => subscription | SubscriptionTypes.Quote);
                Client.SubscribeQuote(symbol);
            }

            public void SubscribeSecondAgg(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Second,
                    (_, subscription) => subscription | SubscriptionTypes.Second);
                Client.SubscribeSecondAgg(symbol);
            }

            public void SubscribeMinuteAgg(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.Minute,
                    (_, subscription) => subscription | SubscriptionTypes.Minute);
                Client.SubscribeMinuteAgg(symbol);
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
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Trade);
                Client.UnsubscribeTrade(symbol);
            }

            public void UnsubscribeQuote(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Quote);
                Client.UnsubscribeQuote(symbol);
            }

            public void UnsubscribeSecondAgg(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Second);
                Client.UnsubscribeSecondAgg(symbol);
            }

            public void UnsubscribeMinuteAgg(String symbol)
            {
                Subscriptions.AddOrUpdate(symbol, SubscriptionTypes.None,
                    (_, subscription) => subscription & ~SubscriptionTypes.Minute);
                Client.UnsubscribeMinuteAgg(symbol);
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
                add => Client.TradeReceived += value;
                remove => Client.TradeReceived -= value;
            }

            public event Action<IStreamQuote>? QuoteReceived
            {
                add => Client.QuoteReceived += value;
                remove => Client.QuoteReceived -= value;
            }

            public event Action<IStreamAgg>? MinuteAggReceived
            {
                add => Client.MinuteAggReceived += value;
                remove => Client.MinuteAggReceived -= value;
            }

            public event Action<IStreamAgg>? SecondAggReceived
            {
                add => Client.SecondAggReceived += value;
                remove => Client.SecondAggReceived -= value;
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
        /// with automatic reconnection support with the default reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IPolygonStreamingClient WithReconnect(
            this IPolygonStreamingClient client) =>
            WithReconnect(client, ReconnectionParameters.Default);

        /// <summary>
        /// Wraps instance of <see cref="IPolygonStreamingClient"/> into the helper class
        /// with automatic reconnection support and provide optional reconnection parameters.
        /// </summary>
        /// <param name="client">Original streaming client for wrapping.</param>
        /// <param name="parameters">Reconnection parameters (or default if missing).</param>
        /// <returns>Wrapped version of the <paramref name="client"/> object with reconnect.</returns>
        public static IPolygonStreamingClient WithReconnect(
            this IPolygonStreamingClient client,
            ReconnectionParameters parameters) =>
            new ClientWithReconnection(client, parameters);
    }
}
