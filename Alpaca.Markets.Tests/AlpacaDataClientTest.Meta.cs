using System.Globalization;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaDataClientTest
{
    [Fact]
    public async Task ListExchangesAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/meta/exchanges", createDictionary());

        verifyDictionary(await mock.Client.ListExchangesAsync());
    }

    [Fact]
    public async Task ListTradeConditionsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/meta/conditions/trade", createDictionary());

        verifyDictionary(await mock.Client.ListTradeConditionsAsync(Tape.A));
    }

    [Fact]
    public async Task ListQuoteConditionsAsyncWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaDataClientMock();

        mock.AddGet("/v2/stocks/meta/conditions/quote", createDictionary());

        verifyDictionary(await mock.Client.ListQuoteConditionsAsync(Tape.B));
    }

    private static JObject createDictionary() =>
        new(Enumerable.Range(1, 10)
            .Select(index => new JProperty(
                index.ToString("D", CultureInfo.InvariantCulture),
                Guid.NewGuid().ToString("D"))));

    private static void verifyDictionary(
        IReadOnlyDictionary<String, String> dictionary)
    {
        Assert.NotEmpty(dictionary);
        foreach (var (code, name) in dictionary)
        {
            Assert.False(String.IsNullOrEmpty(code));
            Assert.False(String.IsNullOrWhiteSpace(name));
        }
    }
}
