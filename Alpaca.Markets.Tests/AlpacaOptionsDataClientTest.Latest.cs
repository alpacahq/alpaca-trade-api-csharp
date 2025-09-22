namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task ListLatestBarsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddLatestBarsExpectation(PathPrefix, _symbols);

        var bars = await mock.Client.ListLatestBarsAsync(
            new LatestOptionsDataRequest(_symbols));

        Assert.NotNull(bars);
        Assert.NotEmpty(bars);

        foreach (var symbol in _symbols)
        {
            Assert.True(bars[symbol].Validate(symbol));
        }
    }
}