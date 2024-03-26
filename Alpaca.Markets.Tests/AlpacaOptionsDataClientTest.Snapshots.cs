namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddCryptoSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.ListSnapshotsAsync(
            new LatestOptionsDataRequest(_symbols));

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        foreach (var symbol in _symbols)
        {
            var snapshot = snapshots[symbol];
            validate(snapshot, symbol);
        }
    }
    
    [Fact]
    public async Task GetOptionChainAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        mock.AddOptionChainExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.GetOptionChainAsync(
            new OptionChainRequest("AAPL"));

        Assert.NotNull(snapshots);
        Assert.NotEmpty(snapshots);

        foreach (var symbol in _symbols)
        {
            var snapshot = snapshots[symbol];
            validate(snapshot, symbol);
        }
    }

    private static void validate(
        ISnapshot snapshot,
        String symbol)
    {
        Assert.Equal(symbol, snapshot.Symbol);

        Assert.Null(snapshot.PreviousDailyBar);
        Assert.Null(snapshot.CurrentDailyBar);
        Assert.Null(snapshot.MinuteBar);

        Assert.NotNull(snapshot.Trade);
        Assert.NotNull(snapshot.Quote);

        Assert.True(snapshot.Trade.Validate(symbol));
        Assert.True(snapshot.Quote.Validate(symbol));
    }
}
