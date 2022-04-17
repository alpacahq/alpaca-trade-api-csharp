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
    /// Asset is marginable or not
    /// </summary>
    [UsedImplicitly]
    Boolean Marginable { get; }

    /// <summary>
    /// Asset is shortable or not
    /// </summary>
    [UsedImplicitly]
    Boolean Shortable { get; }

    /// <summary>
    /// Asset is easy-to-borrow or not
    /// </summary>
    [UsedImplicitly]
    Boolean EasyToBorrow { get; }

    /// <summary>
    /// Asset is fractionable or not
    /// </summary>
    [UsedImplicitly]
    Boolean Fractionable { get; }
}
