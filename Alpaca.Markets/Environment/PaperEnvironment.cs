using System;

namespace Alpaca.Markets
{
    internal sealed class PaperEnvironment : IEnvironment
    {
        public Uri AlpacaTradingApi { get; } = new ("https://paper-api.alpaca.markets");

        public Uri AlpacaDataApi => Environments.Live.AlpacaDataApi;

        public Uri AlpacaStreamingApi { get; } = new ("wss://paper-api.alpaca.markets/stream");

        public Uri AlpacaDataStreamingApi => new Uri("wss://stream.data.alpaca.markets/v2/iex");
    }
}
