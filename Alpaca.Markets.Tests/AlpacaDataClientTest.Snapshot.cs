using Xunit;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task GetSnapshotAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/**/snapshot", new JsonSnapshot
        {
            JsonPreviousDailyBar = new JsonHistoricalBar(),
            JsonCurrentDailyBar = new JsonHistoricalBar(),
            JsonMinuteBar = new JsonHistoricalBar(),
            JsonQuote = new JsonHistoricalQuote(),
            JsonTrade = new JsonHistoricalTrade(),
            Symbol = Stock
        });

        var snapshot = await mock.Client.GetSnapshotAsync(Stock);

        Assert.NotNull(snapshot.Quote);
        Assert.NotNull(snapshot.Trade);
        Assert.NotNull(snapshot.MinuteBar);
        Assert.NotNull(snapshot.CurrentDailyBar);
        Assert.NotNull(snapshot.PreviousDailyBar);

        Assert.Equal(Stock, snapshot.Symbol);
        Assert.Equal(Stock, snapshot.Quote!.Symbol);
        Assert.Equal(Stock, snapshot.Trade!.Symbol);
        Assert.Equal(Stock, snapshot.MinuteBar!.Symbol);
        Assert.Equal(Stock, snapshot.CurrentDailyBar!.Symbol);
        Assert.Equal(Stock, snapshot.PreviousDailyBar!.Symbol);
    }

    [Fact]
    public async Task GetSnapshotsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/snapshots", new Dictionary<String, JsonSnapshot>
        {
            { Stock, new JsonSnapshot { Symbol = Stock } }
        });

        var snapshots = await mock.Client
            .GetSnapshotsAsync(new [] { Stock });

        Assert.NotEmpty(snapshots);
        Assert.Contains(Stock, snapshots);

        var snapshot = snapshots[Stock];

        Assert.Null(snapshot.Quote);
        Assert.Null(snapshot.Trade);
        Assert.Null(snapshot.MinuteBar);
        Assert.Null(snapshot.CurrentDailyBar);
        Assert.Null(snapshot.PreviousDailyBar);

        Assert.Equal(Stock, snapshot.Symbol);
    }
}
