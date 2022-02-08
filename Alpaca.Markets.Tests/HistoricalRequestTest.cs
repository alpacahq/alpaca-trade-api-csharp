using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Alpaca.Markets.Tests;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
public sealed class HistoricalRequestTest
{
    private static readonly String[] _crypto = { "ETHUSD", "BTCUSD" };

    private static readonly String[] _stocks = { "AAPL", "MSFT" };

    private static readonly CryptoExchange[] _cryptoExchanges =
        Enum.GetValues<CryptoExchange>();

    private static readonly Interval<DateTime> _timeInterval =
        new(DateTime.Today.AddDays(-1), DateTime.Today);

    [Fact]
    public void HistoricalBarsRequestWorks()
    {
        var original =
            new HistoricalBarsRequest(_stocks, _timeInterval, BarTimeFrame.Hour)
                {
                    Adjustment = Adjustment.DividendsOnly
                }
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        Assert.Equal(original.Adjustment, validated.Adjustment);
        Assert.Equal(original.TimeFrame, validated.TimeFrame);
        validateCopiedProperties(original, validated);
    }

    [Fact]
    public void HistoricalTradesRequestWorks()
    {
        var original =
            new HistoricalTradesRequest(_stocks, _timeInterval)
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        validateCopiedProperties(original, validated);
    }

    [Fact]
    public void HistoricalQuotesRequestWorks()
    {
        var original =
            new HistoricalQuotesRequest(_stocks, _timeInterval)
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        validateCopiedProperties(original, validated);
    }

    [Fact]
    public void HistoricalCryptoBarsRequestWorks()
    {
        var original =
            new HistoricalCryptoBarsRequest(_crypto, _timeInterval, BarTimeFrame.Hour)
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithExchanges(_cryptoExchanges)
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        Assert.Equal(original.TimeFrame, validated.TimeFrame);
        validateCopiedProperties(original, validated);
    }

    [Fact]
    public void HistoricalCryptoTradesRequestWorks()
    {
        var original =
            new HistoricalCryptoTradesRequest(_stocks, _timeInterval)
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithExchanges(_cryptoExchanges)
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        validateCopiedProperties(original, validated);
    }

    [Fact]
    public void HistoricalCryptoQuotesRequestWorks()
    {
        var original =
            new HistoricalCryptoQuotesRequest(_stocks, _timeInterval)
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithExchanges(_cryptoExchanges)
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        validateCopiedProperties(original, validated);
    }
    
    [Fact]
    public void HistoricalNewsArticlesRequestWorks()
    {
        var original =
            new NewsArticlesRequest(_stocks)
                {
                    TimeInterval = new Interval<DateTime>()
                        .WithFrom(DateTime.UnixEpoch).WithInto(DateTime.UtcNow),
                    SortDirection = SortDirection.Ascending,
                    ExcludeItemsWithoutContent = true,
                    SendFullContentForItems = true
                }
                .WithPageToken(Guid.NewGuid().ToString("D"))
                .WithPageSize(42);
        var validated = getValidatedRequest(original);

        Assert.Equal(original.ExcludeItemsWithoutContent, validated.ExcludeItemsWithoutContent);
        Assert.Equal(original.SendFullContentForItems, validated.SendFullContentForItems);
        Assert.Equal(original.SortDirection, validated.SortDirection);
        Assert.Equal(original.TimeInterval, validated.TimeInterval);

        Assert.Equal(original.Pagination.Size, validated.Pagination.Size);
        Assert.Equal(original.Symbols, validated.Symbols);

        Assert.NotEqual(original.Pagination.Token, validated.Pagination.Token);
        Assert.Null(validated.Pagination.Token);
    }

    private static TRequest getValidatedRequest<TRequest, TItem>(
        IHistoricalRequest<TRequest, TItem> original)
        where TRequest : IHistoricalRequest<TRequest, TItem> =>
        original.GetValidatedRequestWithoutPageToken();

    private static void validateCopiedProperties(
        HistoricalRequestBase original,
        HistoricalRequestBase validated)
    {
        Assert.Equal(original.Pagination.Size, validated.Pagination.Size);
        Assert.Equal(original.Symbols, validated.Symbols);

        Assert.NotEqual(original.Pagination.Token, validated.Pagination.Token);
        Assert.Null(validated.Pagination.Token);
    }

    private static void validateCopiedProperties(
        HistoricalCryptoRequestBase original,
        HistoricalCryptoRequestBase validated)
    {
        Assert.Equal(original.Exchanges, validated.Exchanges);
        validateCopiedProperties((HistoricalRequestBase)original, validated);
    }
}
