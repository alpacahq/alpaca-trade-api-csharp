using System;
using System.Collections.Generic;

namespace Alpaca.Markets
{
    /// <summary>
    /// Provides unified type-safe access for Polygon streaming API via websockets.
    /// </summary>
    public interface IPolygonStreamingClient : IStreamingClientBase
    {
        /// <summary>
        /// Occured when new trade received from stream.
        /// </summary>
        event Action<IStreamTrade>? TradeReceived;

        /// <summary>
        /// Occured when new quote received from stream.
        /// </summary>
        event Action<IStreamQuote>? QuoteReceived;

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        event Action<IStreamAgg>? MinuteAggReceived;

        /// <summary>
        /// Occured when new bar received from stream.
        /// </summary>
        event Action<IStreamAgg>? SecondAggReceived;

        /// <summary>
        /// Subscribes for the trade updates via <see cref="PolygonStreamingClient.TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void SubscribeTrade(
            String symbol);

        /// <summary>
        /// Subscribes for the quote updates via <see cref="PolygonStreamingClient.QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void SubscribeQuote(
            String symbol);

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="PolygonStreamingClient.SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void SubscribeSecondAgg(
            String symbol);

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="PolygonStreamingClient.MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void SubscribeMinuteAgg(
            String symbol);

        /// <summary>
        /// Subscribes for the trade updates via <see cref="PolygonStreamingClient.TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void SubscribeTrade(
            IEnumerable<String> symbols);

        /// <summary>
        /// Subscribes for the quote updates via <see cref="PolygonStreamingClient.QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void SubscribeQuote(
            IEnumerable<String> symbols);

        /// <summary>
        /// Subscribes for the second bar updates via <see cref="PolygonStreamingClient.SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void SubscribeSecondAgg(
            IEnumerable<String> symbols);

        /// <summary>
        /// Subscribes for the minute bar updates via <see cref="PolygonStreamingClient.MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void SubscribeMinuteAgg(
            IEnumerable<String> symbols);

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="PolygonStreamingClient.TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void UnsubscribeTrade(
            String symbol);

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="PolygonStreamingClient.QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void UnsubscribeQuote(
            String symbol);

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="PolygonStreamingClient.SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void UnsubscribeSecondAgg(
            String symbol);

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="PolygonStreamingClient.MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbol">Asset name for subscription change.</param>
        void UnsubscribeMinuteAgg(
            String symbol);

        /// <summary>
        /// Unsubscribes from the trade updates via <see cref="PolygonStreamingClient.TradeReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void UnsubscribeTrade(
            IEnumerable<String> symbols);

        /// <summary>
        /// Unsubscribes from the quote updates via <see cref="PolygonStreamingClient.QuoteReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void UnsubscribeQuote(
            IEnumerable<String> symbols);

        /// <summary>
        /// Unsubscribes from the second bar updates via <see cref="PolygonStreamingClient.SecondAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void UnsubscribeSecondAgg(
            IEnumerable<String> symbols);

        /// <summary>
        /// Unsubscribes from the minute bar updates via <see cref="PolygonStreamingClient.MinuteAggReceived"/>
        /// event for specific asset from Polygon streaming API.
        /// </summary>
        /// <param name="symbols">List of asset names for subscription change.</param>
        void UnsubscribeMinuteAgg(
            IEnumerable<String> symbols);
    }
}
