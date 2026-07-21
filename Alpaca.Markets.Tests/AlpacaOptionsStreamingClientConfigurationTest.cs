namespace Alpaca.Markets.Tests;

public sealed class AlpacaOptionsStreamingClientConfigurationTest
{
    [Fact]
    public void GetApiEndpoint_UsesIndicativePath_ForDefaultConfiguration()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration();

        var endpoint = configuration.GetApiEndpoint();

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/indicative", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/indicative", endpoint.AbsolutePath);
    }

    [Fact]
    public void GetApiEndpoint_UsesOpraPath_ForOpraFeed()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration(OptionsFeed.Opra);

        var endpoint = configuration.GetApiEndpoint();

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/opra", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/opra", endpoint.AbsolutePath);
    }

    [Fact]
    public void WithFeed_UpdatesEndpointPath()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration(OptionsFeed.Indicative)
            .WithFeed(OptionsFeed.Opra);

        var endpoint = configuration.GetApiEndpoint();

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/opra", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/opra", endpoint.AbsolutePath);
    }
}
