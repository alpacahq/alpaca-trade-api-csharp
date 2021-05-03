using System;

namespace Alpaca.Markets
{
    internal sealed class LiveEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi => new ("https://api.alpaca.markets");

        public Uri AlpacaDataApi => new ("https://data.alpaca.markets");

        public Uri AlpacaStreamingApi => new ("wss://api.alpaca.markets/stream");

        public Uri AlpacaDataStreamingApi => new ("wss://stream.data.alpaca.markets/v2/sip");
    }
}
