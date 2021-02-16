using System;

namespace Alpaca.Markets
{
    internal sealed class LiveEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new ("https://api.alpaca.markets");

        public Uri AlpacaDataApi { get; } = new ("https://data.alpaca.markets");

        public Uri PolygonDataApi { get; } = new ("https://api.polygon.io");

        public Uri AlpacaStreamingApi { get; } = new ("wss://api.alpaca.markets/stream");

        public Uri PolygonStreamingApi { get; } = new ("wss://socket.polygon.io/stocks");

        public Uri AlpacaDataStreamingApi { get; } = new ("wss://data.alpaca.markets/stream");
    }
}
