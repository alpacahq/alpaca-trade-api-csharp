namespace Alpaca.Markets;

/// <summary>
/// Encapsulates asset information from Alpaca REST API.
/// </summary>
public interface IAsset
{
    /// <summary>
    /// Gets unique asset identifier used by Alpaca.
    /// </summary>
    [UsedImplicitly]
    Guid AssetId { get; }

    /// <summary>
    /// Gets asset class.
    /// </summary>
    [UsedImplicitly]
    [SuppressMessage(
        "Naming", "CA1716:Identifiers should not match keywords",
        Justification = "Already used by clients and creates conflict only in VB.NET")]
    AssetClass Class { get; }

    /// <summary>
    /// Gets asset source exchange.
    /// </summary>
    [UsedImplicitly]
    Exchange Exchange { get; }

    /// <summary>
    /// Gets asset symbol.
    /// </summary>
    [UsedImplicitly]
    String Symbol { get; }

    /// <summary>
    /// Gets asset name.
    /// </summary>
    [UsedImplicitly]
    String Name { get; }

    /// <summary>
    /// Get asset status in API.
    /// </summary>
    [UsedImplicitly]
    AssetStatus Status { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is tradable.
    /// </summary>
    [UsedImplicitly]
    Boolean IsTradable { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is marginable.
    /// </summary>
    [UsedImplicitly]
    Boolean Marginable { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is shortable.
    /// </summary>
    [UsedImplicitly]
    Boolean Shortable { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is easy-to-borrow.
    /// </summary>
    [UsedImplicitly]
    Boolean EasyToBorrow { get; }

    /// <summary>
    /// Returns <c>true</c> if asset is fractionable.
    /// </summary>
    [UsedImplicitly]
    Boolean Fractionable { get; }

    /// <summary>
    /// Gets minimum order size. This property is valid only for crypto assets.
    /// </summary>
    [UsedImplicitly]
    Decimal? MinOrderSize { get; }

    /// <summary>
    /// Gets amount a trade quantity can be incremented by. This property is valid only for crypto assets.
    /// </summary>
    [UsedImplicitly]
    Decimal? MinTradeIncrement { get; }

    /// <summary>
    /// Gets amount the price can be incremented by. This property is valid only for crypto assets.
    /// </summary>
    [UsedImplicitly]
    Decimal? PriceIncrement { get; }

    /// <summary>
    /// Gets the % margin requirement for the asset. This property is valid only for equity assets.
    /// </summary>
    [UsedImplicitly]
    Decimal? MaintenanceMarginRequirement { get; }

    /// <summary>
    /// Gets the list of asset attributes (unique asset characteristics like PTP order acceptance mode).
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<AssetAttributes> Attributes { get; }
}
