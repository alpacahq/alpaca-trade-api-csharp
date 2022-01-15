namespace Alpaca.Markets;

internal sealed class LiveEnvironment : IEnvironment
{
    public Uri AlpacaTradingApi => new("https://api.alpaca.markets");

    public Uri AlpacaDataApi => new("https://data.alpaca.markets");

    public Uri AlpacaStreamingApi => new("wss://api.alpaca.markets/stream");

    public Uri AlpacaDataStreamingApi => new("wss://stream.data.alpaca.markets/v2/sip");

    public Uri AlpacaCryptoStreamingApi => new("wss://stream.data.alpaca.markets/v1beta1/crypto");

    public Uri AlpacaNewsStreamingApi => new ("wss://stream.data.tradetalk.us/v1beta1/news");
}
