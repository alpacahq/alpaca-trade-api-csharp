namespace Alpaca.Markets;

/// <summary>
/// Encapsulates watch list information from Alpaca REST API.
/// </summary>
public interface IWatchList
{
    /// <summary>
    /// Gets unique watch list identifier.
    /// </summary>
    [UsedImplicitly]
    Guid WatchListId { get; }

    /// <summary>
    /// Gets watch list creation time in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime CreatedUtc { get; }

    /// <summary>
    /// Gets watch list last update time in UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? UpdatedUtc { get; }

    /// <summary>
    /// Gets watch list user-defined name.
    /// </summary>
    [UsedImplicitly]
    String Name { get; }

    /// <summary>
    /// Gets <see cref="IAccount.AccountId"/> for this watch list.
    /// </summary>
    [UsedImplicitly]
    Guid AccountId { get; }

    /// <summary>
    /// Gets the content of this watchlist, in the order as registered by the client.
    /// </summary>
    [UsedImplicitly]
    IReadOnlyList<IAsset> Assets { get; }
}
