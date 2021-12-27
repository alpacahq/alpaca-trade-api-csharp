using System;

namespace Alpaca.Markets
{
    internal sealed class LiveEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new ("https://api.alpaca.markets");

        public Uri AlpacaDataApi { get; } = new ("https://data.alpaca.markets");

        public Uri AlpacaStreamingApi { get; } = new ("wss://api.alpaca.markets/stream");

        public Uri AlpacaDataStreamingApi { get; } = new ("wss://stream.data.alpaca.markets/v2/sip");
    }
}
