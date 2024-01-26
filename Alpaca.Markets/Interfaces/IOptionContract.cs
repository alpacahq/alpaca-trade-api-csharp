namespace Alpaca.Markets;

/// <summary>
/// 
/// </summary>
public interface IOptionContract
{
    /// <summary>
    /// Gets unique option contract identifier used by Alpaca.
    /// </summary>
    [UsedImplicitly]
    Guid ContractId { get; }

    /// <summary>
    /// Get option contract symbol.
    /// </summary>
    String Symbol { get; }

    /// <summary>
    /// Gets option contract name.
    /// </summary>
    [UsedImplicitly]
    String Name { get; }

    /// <summary>
    /// Get option contract status in API.
    /// </summary>
    [UsedImplicitly]
    AssetStatus Status { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is tradable.
    /// </summary>
    [UsedImplicitly]
    Boolean IsTradable { get; }

    /// <summary>
    /// Get option contract size.
    /// </summary>
    [UsedImplicitly]
    Decimal Size { get; }

    /// <summary>
    /// Get option contract type.
    /// </summary>
    [UsedImplicitly]
    OptionType OptionType { get; }

    /// <summary>
    /// Get option contract strike price.
    /// </summary>
    [UsedImplicitly]
    Decimal StrikePrice { get; }

    /// <summary>
    /// Get option contract expiration date.
    /// </summary>
    [UsedImplicitly]
    DateOnly ExpirationDate { get; }

    /// <summary>
    /// Get option contract execution style.
    /// </summary>
    [UsedImplicitly]
    OptionStyle OptionStyle { get; }

    /// <summary>
    /// Get option contract root asset <see cref="IAsset.Symbol"/> property.
    /// </summary>
    [UsedImplicitly]
    String RootSymbol { get; }

    /// <summary>
    /// Get option contract underlying asset <see cref="IAsset.Symbol"/> property.
    /// </summary>
    [UsedImplicitly]
    String UnderlyingSymbol { get; }

    /// <summary>
    /// Get option contract underlying asset <see cref="IAsset.AssetId"/> property.
    /// </summary>
    [UsedImplicitly]
    Guid UnderlyingAssetId { get; }

    /// <summary>
    /// Get option contract open interest.
    /// </summary>
    [UsedImplicitly]
    Decimal? OpenInterest { get; }

    /// <summary>
    /// Get option contract open interest date.
    /// </summary>
    [UsedImplicitly]
    DateOnly? OpenInterestDate { get; }

    /// <summary>
    /// Get option contract close price.
    /// </summary>
    [UsedImplicitly]
    Decimal? ClosePrice { get; }

    /// <summary>
    /// Get option contract close price date.
    /// </summary>
    [UsedImplicitly]
    DateOnly? ClosePriceDate { get; }
}
