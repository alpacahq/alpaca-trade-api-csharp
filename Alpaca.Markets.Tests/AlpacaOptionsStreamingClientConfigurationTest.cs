using System.Reflection;

namespace Alpaca.Markets.Tests;

public sealed class AlpacaOptionsStreamingClientConfigurationTest
{
    [Fact]
    public void GetApiEndpoint_UsesIndicativePath_ForDefaultConfiguration()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration();

        var endpoint = InvokeGetApiEndpoint(configuration);

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/indicative", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/indicative", endpoint.AbsolutePath);
        Assert.DoesNotContain("/v1beta1/v1beta1/", endpoint.AbsolutePath);
    }

    [Fact]
    public void GetApiEndpoint_UsesOpraPath_ForOpraFeed()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration(OptionsFeed.Opra);

        var endpoint = InvokeGetApiEndpoint(configuration);

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/opra", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/opra", endpoint.AbsolutePath);
    }

    [Fact]
    public void WithFeed_UpdatesEndpointPath()
    {
        var configuration = new AlpacaOptionsStreamingClientConfiguration(OptionsFeed.Indicative)
            .WithFeed(OptionsFeed.Opra);

        var endpoint = InvokeGetApiEndpoint(configuration);

        Assert.Equal("wss://stream.data.alpaca.markets/v1beta1/opra", endpoint.AbsoluteUri);
        Assert.Equal("/v1beta1/opra", endpoint.AbsolutePath);
    }

    private static Uri InvokeGetApiEndpoint(AlpacaOptionsStreamingClientConfiguration configuration)
    {
        var method = typeof(AlpacaOptionsStreamingClientConfiguration)
            .GetMethod("GetApiEndpoint", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(method);

        var endpoint = method!.Invoke(configuration, null);
        Assert.NotNull(endpoint);

        return Assert.IsType<Uri>(endpoint);
    }
}
