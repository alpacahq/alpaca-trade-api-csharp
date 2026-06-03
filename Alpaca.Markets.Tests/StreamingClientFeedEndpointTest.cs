using System.Net.WebSockets;
using Alpaca.Markets.Extensions;

namespace Alpaca.Markets.Tests;

public sealed class StreamingClientFeedEndpointTest
{
    private static readonly SecurityKey _key =
        new SecretKey(Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N"));

    [Theory]
    [InlineData(MarketDataFeed.Iex, "wss://stream.data.alpaca.markets/v2/iex")]
    [InlineData(MarketDataFeed.Sip, "wss://stream.data.alpaca.markets/v2/sip")]
    [InlineData(MarketDataFeed.Otc, "wss://stream.data.alpaca.markets/v2/otc")]
    [InlineData(MarketDataFeed.DelayedSip, "wss://stream.data.alpaca.markets/v2/delayed_sip")]
    [InlineData(MarketDataFeed.Boats, "wss://stream.data.alpaca.markets/v2/boats")]
    [InlineData(MarketDataFeed.Overnight, "wss://stream.data.alpaca.markets/v2/overnight")]
    public async Task DataStreamingClientConnectsToSelectedFeed(
        MarketDataFeed feed,
        String expectedUrl)
    {
        // The selected feed must override the environment-provided endpoint regardless of environment.
        var connectedUrl = await captureConnectUrlAsync(
            Environments.Paper.GetAlpacaDataStreamingClientConfiguration(_key, feed),
            configuration => configuration.GetClient());

        Assert.Equal(expectedUrl, connectedUrl);
    }

    [Theory]
    [ClassData(typeof(EnvironmentTestData))]
    public async Task DataStreamingClientWithFeedOverridesEnvironment(
        IEnvironment environment)
    {
        var connectedUrl = await captureConnectUrlAsync(
            environment.GetAlpacaDataStreamingClientConfiguration(_key).WithFeed(MarketDataFeed.Sip),
            configuration => configuration.GetClient());

        Assert.Equal("wss://stream.data.alpaca.markets/v2/sip", connectedUrl);
    }

    [Fact]
    public async Task DataStreamingClientWithoutFeedKeepsLiveEndpoint()
    {
        var connectedUrl = await captureConnectUrlAsync(
            Environments.Live.GetAlpacaDataStreamingClientConfiguration(_key),
            configuration => configuration.GetClient());

        Assert.Equal("wss://stream.data.alpaca.markets/v2/sip", connectedUrl);
    }

    [Fact]
    public async Task DataStreamingClientWithoutFeedKeepsPaperEndpoint()
    {
        var connectedUrl = await captureConnectUrlAsync(
            Environments.Paper.GetAlpacaDataStreamingClientConfiguration(_key),
            configuration => configuration.GetClient());

        Assert.Equal("wss://stream.data.alpaca.markets/v2/iex", connectedUrl);
    }

    [Theory]
    [InlineData(OptionsFeed.Opra, "wss://stream.data.alpaca.markets/v1beta1/opra")]
    [InlineData(OptionsFeed.Indicative, "wss://stream.data.alpaca.markets/v1beta1/indicative")]
    public async Task OptionsStreamingClientConnectsToSelectedFeed(
        OptionsFeed feed,
        String expectedUrl)
    {
        // Regression guard: the v1beta1/{feed} path segment must not be duplicated.
        var connectedUrl = await captureConnectUrlAsync(
            Environments.Live.GetAlpacaOptionsStreamingClientConfiguration(_key, feed),
            configuration => configuration.GetClient());

        Assert.Equal(expectedUrl, connectedUrl);
    }

    private static async Task<String> captureConnectUrlAsync<TConfiguration>(
        TConfiguration configuration,
        Func<TConfiguration, IStreamingClient> factory)
        where TConfiguration : StreamingClientConfiguration
    {
        Uri? connectedUrl = null;

        var webSocket = new Mock<IWebSocket>();
        webSocket
            .Setup(socket => socket.ConnectAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>()))
            .Callback<Uri, CancellationToken>((uri, _) => connectedUrl = uri)
            .Returns(Task.CompletedTask);
        // Immediately report a clean close so the background receive loop terminates deterministically.
        webSocket
            .SetupGet(socket => socket.CloseStatus)
            .Returns(WebSocketCloseStatus.NormalClosure);
        webSocket
            .Setup(socket => socket.ReceiveAsync(It.IsAny<Memory<Byte>>()))
            .ReturnsAsync(new ReceiveResult(WebSocketMessageType.Close, true, 0));

        configuration.WebSocketFactory = () => webSocket.Object;

        using var client = factory(configuration);
        await client.ConnectAsync();

        Assert.NotNull(connectedUrl);
        return connectedUrl!.AbsoluteUri;
    }
}
