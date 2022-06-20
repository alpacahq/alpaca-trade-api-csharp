namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for
/// <see cref="IAlpacaTradingClient.AddAssetIntoWatchListByIdAsync(ChangeWatchListRequest{Guid},CancellationToken)"/>,
/// <see cref="IAlpacaTradingClient.AddAssetIntoWatchListByNameAsync(ChangeWatchListRequest{String},CancellationToken)"/>,
/// <see cref="IAlpacaTradingClient.DeleteAssetFromWatchListByIdAsync(ChangeWatchListRequest{Guid},CancellationToken)"/>, and
/// <see cref="IAlpacaTradingClient.DeleteAssetFromWatchListByNameAsync(ChangeWatchListRequest{String},CancellationToken)"/>
/// calls.
/// </summary>
/// <typeparam name="TKey"></typeparam>
public sealed class ChangeWatchListRequest<TKey> : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="ChangeWatchListRequest{TKey}"/> object.
    /// </summary>
    /// <param name="key">Unique watch list identifier or name.</param>
    /// <param name="asset">Asset symbol for adding into watch list.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="key"/> or <paramref name="asset"/> argument is <c>null</c>.
    /// </exception>
    public ChangeWatchListRequest(
        TKey key,
        String asset)
    {
        Key = key;
        Asset = asset.EnsureNotNull();
    }

    /// <summary>
    /// Gets unique watch list identifier or name.
    /// </summary>
    [JsonIgnore]
    public TKey Key { get; }

    /// <summary>
    /// Gets asset symbol for adding/deleting into watch list.
    /// </summary>
    [JsonProperty(PropertyName = "symbol", Required = Required.Always)]
    public String Asset { get; }

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Key.TryValidateWatchListName();
        yield return Asset.TryValidateSymbolName();
    }
}
