using System;

namespace Alpaca.Markets
{
    internal sealed class LiveEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new Uri("https://api.alpaca.markets");

        public Uri AlpacaDataApi { get; } = new Uri("https://data.alpaca.markets");

        public Uri AlpacaStreamingApi { get; } = new Uri("wss://api.alpaca.markets/stream");

        public Uri AlpacaDataStreamingApi { get; } = new Uri("wss://stream.data.alpaca.markets/v2/sip");
    }
}
