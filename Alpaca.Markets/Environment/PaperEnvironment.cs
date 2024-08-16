namespace Alpaca.Markets;

internal sealed class PaperEnvironment : IEnvironment
{
    public Uri AlpacaTradingApi => new("https://paper-api.alpaca.markets");

    public Uri AlpacaDataApi => Environments.Live.AlpacaDataApi;

    public Uri AlpacaStreamingApi => new("wss://paper-api.alpaca.markets/stream");

    public Uri AlpacaDataStreamingApi => new("wss://stream.data.alpaca.markets/v2/iex");

    public Uri AlpacaCryptoStreamingApi => Environments.Live.AlpacaCryptoStreamingApi;

    public Uri AlpacaOptionsStreamingApi => new("wss://stream.data.alpaca.markets/v1beta1/indicative");

    public Uri AlpacaNewsStreamingApi => Environments.Live.AlpacaNewsStreamingApi;
}
