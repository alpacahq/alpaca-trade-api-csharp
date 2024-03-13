using System.Diagnostics.CodeAnalysis;

namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    private static readonly DateOnly _today = DateOnly.FromDateTime(DateTime.Today);

    [Fact]
    public async Task GetOptionContractByIdWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var contractId = Guid.NewGuid();

        mock.AddGet("/v2/options/contracts/*", createOptionContract(contractId, Stock));

        var optionContract = await mock.Client.GetOptionContractByIdAsync(contractId);

        validateOptionContract(optionContract, contractId, Stock);
    }

    [Fact]
    public async Task GetOptionContractBySymbolWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var contractId = Guid.NewGuid();

        mock.AddGet("/v2/options/contracts/*", createOptionContract(contractId, Stock));

        var optionContract = await mock.Client.GetOptionContractBySymbolAsync(Stock);

        validateOptionContract(optionContract, contractId, Stock);
    }

    [Fact]
    public async Task ListOptionContractsWorks()
    {
        using var mock = mockClientsFactory.GetAlpacaTradingClientMock();

        var contractId = Guid.NewGuid();

        mock.AddGet("/v2/options/contracts", createOptionContractsList(contractId, Stock));

        var optionContracts = await mock.Client.ListOptionContractsAsync(
            new OptionContractsRequest(Stock)
            {
                ExpirationDateGreaterThanOrEqualTo = _today,
                ExpirationDateLessThanOrEqualTo = _today,
                StrikePriceGreaterThanOrEqualTo = Price,
                StrikePriceLessThanOrEqualTo = Price,
                OptionStyle = OptionStyle.American,
                AssetStatus = AssetStatus.Active,
                OptionType = OptionType.Call,
                RootSymbol = Stock
            });

        Assert.NotNull(optionContracts.Items);
        Assert.NotEmpty(optionContracts.Items);

        validateOptionContract(optionContracts.Items.Single(), contractId, Stock);
    }

    private static JObject createOptionContractsList(
        Guid contractId,
        String symbol) =>
        new(new JProperty("option_contracts", new JArray(createOptionContract(contractId, symbol))));

    private static JObject createOptionContract(
        Guid contractId,
        String symbol) =>
        new(
            new JProperty("open_interest_date", _today.ToString("O")),
            new JProperty("close_price_date", _today.ToString("O")),
            new JProperty("expiration_date", _today.ToString("O")),
            new JProperty("underlying_asset_id", contractId),
            new JProperty("style", OptionStyle.American),
            new JProperty("status", AssetStatus.Active),
            new JProperty("underlying_symbol", symbol),
            new JProperty("type", OptionType.Call),
            new JProperty("open_interest", Price),
            new JProperty("root_symbol", symbol),
            new JProperty("strike_price", Price),
            new JProperty("close_price", Price),
            new JProperty("tradable", true),
            new JProperty("symbol", symbol),
            new JProperty("id", contractId),
            new JProperty("name", symbol),
            new JProperty("size", 100));

    [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
    private static void validateOptionContract(
        IOptionContract optionContract,
        Guid contractId,
        String symbol)
    {
        Assert.True(optionContract.IsTradable);

        Assert.NotNull(optionContract.ClosePrice);
        Assert.NotNull(optionContract.OpenInterest);
        Assert.NotNull(optionContract.ClosePriceDate);
        Assert.NotNull(optionContract.OpenInterestDate);

        Assert.Equal(100, optionContract.Size);
        Assert.Equal(Price, optionContract.StrikePrice);

        Assert.Equal(symbol, optionContract.Name);
        Assert.Equal(symbol, optionContract.Symbol);
        Assert.Equal(symbol, optionContract.RootSymbol);
        Assert.Equal(symbol, optionContract.UnderlyingSymbol);

        Assert.Equal(contractId, optionContract.ContractId);
        Assert.Equal(contractId, optionContract.UnderlyingAssetId);

        Assert.Equal(OptionType.Call, optionContract.OptionType);
        Assert.Equal(OptionStyle.American, optionContract.OptionStyle);
    }
}
