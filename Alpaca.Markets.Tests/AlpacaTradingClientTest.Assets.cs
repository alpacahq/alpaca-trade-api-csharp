namespace Alpaca.Markets.Tests;

public sealed partial class AlpacaTradingClientTest
{
    [Fact]
    public async Task GetAssetAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var assetId = Guid.NewGuid();

        mock.AddGet("/v2/assets/*", createAsset(assetId, Stock));

        var asset = await mock.Client.GetAssetAsync(Stock);

        validateAsset(asset, assetId, Stock);
    }

    [Fact]
    public async Task ListAssetsAsyncWorks()
    {
        using var mock = _mockClientsFactory.GetAlpacaTradingClientMock();

        var assetId = Guid.NewGuid();

        mock.AddGet("/v2/assets", new JArray(createAsset(assetId, Crypto)));

        var assets = await mock.Client.ListAssetsAsync(new AssetsRequest
        {
            AssetStatus = AssetStatus.Active,
            AssetClass = AssetClass.UsEquity,
            Exchange = Exchange.Arca
        });

        validateAsset(assets.Single(), assetId, Crypto);
    }

    private static JObject createAsset(
        Guid assetId,
        String symbol) =>
        new(
            new JProperty("maintenance_margin_requirement", 100),
            new JProperty("status", AssetStatus.Active),
            new JProperty("class", AssetClass.UsEquity),
            new JProperty("exchange", Exchange.Amex),
            new JProperty("price_increment", 0.0001),
            new JProperty("min_trade_increment", 1),
            new JProperty("min_order_size", 0.0001),
            new JProperty("easy_to_borrow", true),
            new JProperty("fractionable", false),
            new JProperty("marginable", false),
            new JProperty("shortable", true),
            new JProperty("tradable", true),
            new JProperty("symbol", symbol),
            new JProperty("name", symbol),
            new JProperty("id", assetId));

    private static void validateAsset(
        IAsset asset,
        Guid assetId,
        String symbol)
    {
        Assert.True(asset.Shortable);
        Assert.True(asset.IsTradable);
        Assert.False(asset.Marginable);
        Assert.True(asset.EasyToBorrow);
        Assert.False(asset.Fractionable);

        Assert.Equal(symbol, asset.Name);
        Assert.Equal(symbol, asset.Symbol);
        Assert.Equal(assetId, asset.AssetId);

        Assert.Equal(AssetClass.UsEquity, asset.Class);

        Assert.NotNull(asset.MinOrderSize);
        Assert.NotNull(asset.PriceIncrement);
        Assert.NotNull(asset.MinTradeIncrement);
        Assert.NotNull(asset.MaintenanceMarginRequirement);
    }
}
