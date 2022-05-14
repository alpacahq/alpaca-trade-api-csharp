namespace Alpaca.Markets.Extensions.Tests;

public sealed class EnvironmentTest
{
    [Fact]
    public void WithProxyForAlpacaDataStreamingClientWorks()
    {
        var paper = Environments.Paper;
        var proxy = paper.WithProxyForAlpacaDataStreamingClient();

        Assert.Equal(paper.AlpacaDataApi, proxy.AlpacaDataApi);
        Assert.Equal(paper.AlpacaTradingApi, proxy.AlpacaTradingApi);
        Assert.Equal(paper.AlpacaStreamingApi, proxy.AlpacaStreamingApi);
        Assert.Equal(paper.AlpacaNewsStreamingApi, proxy.AlpacaNewsStreamingApi);
        Assert.Equal(paper.AlpacaCryptoStreamingApi, proxy.AlpacaCryptoStreamingApi);

        Assert.NotEqual(paper.AlpacaDataStreamingApi, proxy.AlpacaDataStreamingApi);
    }
}
