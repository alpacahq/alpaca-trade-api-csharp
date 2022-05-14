namespace Alpaca.Markets.Tests;

public sealed class DefaultValuesTest
{
    [Fact]
    public void BarTimeFrameWorks()
    {
        Assert.Equal(BarTimeFrame.Week, new BarTimeFrame(1, BarTimeFrameUnit.Week));
        Assert.Equal(BarTimeFrame.Month, new BarTimeFrame(1, BarTimeFrameUnit.Month));
        Assert.Equal(BarTimeFrame.Year, new BarTimeFrame(12, BarTimeFrameUnit.Month));
        Assert.Equal(BarTimeFrame.Quarter, new BarTimeFrame(3, BarTimeFrameUnit.Month));
        Assert.Equal(BarTimeFrame.HalfYear, new BarTimeFrame(6, BarTimeFrameUnit.Month));
    }

    [Fact]
    public void PositionQuantityWorks()
    {
        var inPercent = new PositionQuantity();
        var inShares = PositionQuantity.InShares(0M);

        Assert.Equal(0M, inPercent.Value);
        Assert.Equal(0M, inShares.Value);

        Assert.True(inPercent.IsInPercents);
        Assert.False(inPercent.IsInShares);

        Assert.NotEqual(inPercent, inShares);

        Assert.Throws<ArgumentException>(
            () => PositionQuantity.InPercents(-1M));
    }

    [Fact]
    public void OrderQuantityWorks()
    {
        var defaultValue = new OrderQuantity();
        var fromLong = OrderQuantity.FromInt64(0L);

        Assert.Equal(0M, defaultValue.Value);
        Assert.Equal(0M, fromLong.Value);

        Assert.False(fromLong.IsInDollars);
        Assert.True(fromLong.IsInShares);

        Assert.Equal(fromLong, defaultValue);
    }

    [Fact]
    public void TrailOffsetWorks()
    {
        var defaultValue = new TrailOffset();

        Assert.Equal(0M, defaultValue.Value);

        Assert.False(defaultValue.IsInDollars);
        Assert.True(defaultValue.IsInPercent);
    }

    [Fact]
    public void OpenCloseWorks()
    {
        var defaultValue = new OpenClose();

        Assert.Equal(new DateTimeOffset(), defaultValue.OpenEst);
        Assert.Equal(new DateTimeOffset(), defaultValue.CloseEst);

        var interval = defaultValue.ToInterval();

        Assert.False(interval.IsOpen());
        Assert.False(interval.IsEmpty());
    }

    [Fact]
    public void HistoryPeriodWorks()
    {
        var defaultValue = new HistoryPeriod();

        Assert.Equal(0, defaultValue.Value);
        Assert.Equal(HistoryPeriodUnit.Day, defaultValue.Unit);
    }

    [Fact]
    public void RestClientErrorExceptionWorks()
    {
        var defaultValue = new RestClientErrorException();

        Assert.NotNull(defaultValue.Message);
        Assert.Null(defaultValue.InnerException);

        var message = Guid.NewGuid().ToString("N");
        var withMessage = new RestClientErrorException(message);

        Assert.Equal(message, withMessage.Message);
        Assert.Null(withMessage.InnerException);

        var withInnerException = new RestClientErrorException(message, defaultValue);

        Assert.Equal(message, withInnerException.Message);
        Assert.Equal(defaultValue, withInnerException.InnerException);
    }
}
