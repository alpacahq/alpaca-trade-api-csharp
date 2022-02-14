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

        mock.AddGet("/v2/assets", new[] { createAsset(assetId, Crypto) });

        var assets = await mock.Client.ListAssetsAsync(new AssetsRequest
        {
            AssetStatus = AssetStatus.Active,
            AssetClass = AssetClass.Crypto
        });

        validateAsset(assets.Single(), assetId, Crypto);
    }

    private static JsonAsset createAsset(
        Guid assetId,
        String symbol) =>
        new ()
        {
            AssetId = assetId,
            IsTradable = true,
            Shortable = true,
            Symbol = symbol,
            Name = symbol
        };

    private static void validateAsset(
        IAsset asset,
        Guid assetId,
        String symbol)
    {
        Assert.True(asset.Shortable);
        Assert.True(asset.IsTradable);
        Assert.Equal(symbol, asset.Name);
        Assert.Equal(symbol, asset.Symbol);
        Assert.Equal(assetId, asset.AssetId);
    }
}
