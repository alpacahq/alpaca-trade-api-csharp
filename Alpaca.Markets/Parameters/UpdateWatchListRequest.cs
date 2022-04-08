namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.UpdateWatchListByIdAsync(UpdateWatchListRequest,CancellationToken)"/> call.
/// </summary>
public sealed class UpdateWatchListRequest : Validation.IRequest
{
    private readonly List<String> _assets = new();

    /// <summary>
    /// Creates new instance of <see cref="UpdateWatchListRequest"/> object.
    /// </summary>
    /// <param name="watchListId">Unique watch list identifier.</param>
    /// <param name="name">User defined watch list name.</param>
    /// <param name="assets">List of asset symbols for new watch list.</param>
    public UpdateWatchListRequest(
        Guid watchListId,
        String name,
        IEnumerable<String> assets)
    {
        WatchListId = watchListId;
        Name = name;
        _assets.AddRange(
            // ReSharper disable once ConstantNullCoalescingCondition
            (assets ?? Enumerable.Empty<String>())
            .Distinct(StringComparer.Ordinal));
    }

    /// <summary>
    /// Gets the target watch list unique identifier.
    /// </summary>
    [JsonIgnore]
    public Guid WatchListId { get; }

    /// <summary>
    /// Gets the target watch list name.
    /// </summary>
    [JsonProperty(PropertyName = "name", Required = Required.Always)]
    public String Name { get; }

    /// <summary>
    /// Gets list of asset symbols for new watch list.
    /// </summary>
    [JsonProperty(PropertyName = "symbols", Required = Required.Always)]
    public IReadOnlyList<String> Assets => _assets;

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Name.TryValidateWatchListName();
        yield return Assets.TryValidateSymbolName();
    }
}
