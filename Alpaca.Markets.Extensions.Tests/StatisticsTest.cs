namespace Alpaca.Markets.Extensions.Tests;

public sealed class StatisticsTest
{
    private sealed record Bar (
        DateTime TimeUtc, String Symbol,
        Decimal Open, Decimal High, Decimal Low, Decimal Close,
        Decimal Volume, Decimal Vwap, UInt64 TradeCount) : IBar
    {
        public Bar(
            Decimal volume)
            : this(DateTime.UtcNow, String.Empty,
                0M, 0M, 0M, 0M, 
                volume, 0M, 1)
        {
        }
    }

    private const Int32 FirstVolume = 1;

    private const Int32 LastVolume = 10;

    [Fact]
    public void GetAverageDailyTradeVolumeWorks()
    {
        var (adtv, count) = Enumerable
            .Range(FirstVolume, LastVolume)
            .Select(volume => new Bar(volume))
            .GetAverageDailyTradeVolume();

        Assert.Equal(LastVolume - FirstVolume + 1, (Int32)count);
        Assert.Equal((FirstVolume + LastVolume) / 2M, adtv);
    }

    [Fact]
    public async Task GetAverageDailyTradeVolumeAsyncWorks()
    {
        var (adtv, count) = await Enumerable
            .Range(FirstVolume, LastVolume)
            .Select(volume => new Bar(volume))
            .ToAsyncEnumerable()
            .GetAverageDailyTradeVolumeAsync();

        Assert.Equal(LastVolume - FirstVolume + 1, (Int32)count);
        Assert.Equal((FirstVolume + LastVolume) / 2M, adtv);
    }
    
    [Fact]
    public void GetSimpleMovingAverageWorks()
    {
        var smaBars = Enumerable
            .Range(FirstVolume, LastVolume)
            .Select(volume => new Bar(volume))
            .GetSimpleMovingAverage(2)
            .ToList();

        Assert.Equal(LastVolume - FirstVolume, smaBars.Count);
    }
}
