using System;

namespace Alpaca.Markets
{
    internal sealed class PaperEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new ("https://paper-api.alpaca.markets");

        public Uri AlpacaDataApi => Environments.Live.AlpacaDataApi;

        public Uri PolygonDataApi => throw new InvalidOperationException(
            "Polygon.io REST API does not available on this environment.");

        public Uri AlpacaStreamingApi { get; } = new ("wss://paper-api.alpaca.markets/stream");

        public Uri PolygonStreamingApi => throw new InvalidOperationException(
            "Polygon.io streaming API does not available on this environment.");

        public Uri AlpacaDataStreamingApi => Environments.Live.AlpacaDataStreamingApi;
    }
}
