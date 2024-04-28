namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaOptionsDataClientTest
{
    [Fact]
    public async Task ListSnapshotsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        // TODO: olegra - create special method for option snapshots
        mock.AddOptionSnapshotsExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.ListSnapshotsAsync(
            new OptionSnapshotRequest(_symbols));

        Assert.NotNull(snapshots);
        Assert.NotNull(snapshots.Items);
        Assert.NotEmpty(snapshots.Items);

        foreach (var symbol in _symbols)
        {
            var snapshot = snapshots.Items[symbol];
            validate(snapshot, symbol);
        }
    }
    
    [Fact]
    public async Task GetOptionChainAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaOptionsDataClientMock();

        // TODO: olegra - create special method for option snapshots
        mock.AddOptionChainExpectation(PathPrefix, _symbols);

        var snapshots = await mock.Client.GetOptionChainAsync(
            new OptionChainRequest("AAPL"));

        Assert.NotNull(snapshots);
        Assert.NotNull(snapshots.Items);
        Assert.NotEmpty(snapshots.Items);

        foreach (var symbol in _symbols)
        {
            var snapshot = snapshots.Items[symbol];
            validate(snapshot, symbol);
        }
    }

    private static void validate(
        IOptionSnapshot snapshot,
        String symbol)
    {
        Assert.Equal(symbol, snapshot.Symbol);

        Assert.NotNull(snapshot.Trade);
        Assert.NotNull(snapshot.Quote);

        Assert.True(snapshot.Trade.Validate(symbol));
        Assert.True(snapshot.Quote.Validate(symbol));

        Assert.NotNull(snapshot.ImpliedVolatility);
        Assert.True(snapshot.ImpliedVolatility > 0.0M);

        Assert.NotNull(snapshot.Greeks);
        Assert.NotNull(snapshot.Greeks.Delta);
        Assert.NotNull(snapshot.Greeks.Gamma);
        Assert.NotNull(snapshot.Greeks.Theta);
        Assert.NotNull(snapshot.Greeks.Vega);
        Assert.NotNull(snapshot.Greeks.Rho);
    }
}
