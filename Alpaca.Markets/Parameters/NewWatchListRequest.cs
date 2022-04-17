namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.CreateWatchListAsync(NewWatchListRequest,CancellationToken)"/> call.
/// </summary>
public sealed class NewWatchListRequest : Validation.IRequest
{
    private readonly List<String> _assets = new();

    /// <summary>
    /// Creates new instance of <see cref="NewWatchListRequest"/> object.
    /// </summary>
    /// <param name="name">User defined watch list name.</param>
    public NewWatchListRequest(String name) => Name = name;

    /// <summary>
    /// Creates new instance of <see cref="NewWatchListRequest"/> object.
    /// </summary>
    /// <param name="name">User defined watch list name.</param>
    /// <param name="assets">List of asset symbols for new watch list.</param>
    public NewWatchListRequest(
        String name,
        IEnumerable<String> assets)
        : this(name) =>
        _assets.AddRange(
            // ReSharper disable once ConstantNullCoalescingCondition
            (assets ?? Enumerable.Empty<String>())
            .Distinct(StringComparer.Ordinal));

    /// <summary>
    /// Gets user defined watch list name.
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
