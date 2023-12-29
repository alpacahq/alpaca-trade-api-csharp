namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.CreateWatchListAsync(NewWatchListRequest,CancellationToken)"/> call.
/// </summary>
public sealed class NewWatchListRequest : Validation.IRequest
{
    private readonly List<String> _assets = [];

    /// <summary>
    /// Creates new instance of <see cref="NewWatchListRequest"/> object.
    /// </summary>
    /// <param name="name">User defined watch list name.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="name"/> argument is <c>null</c>.
    /// </exception>
    public NewWatchListRequest(String name) => Name = name.EnsureNotNull();

    /// <summary>
    /// Creates new instance of <see cref="NewWatchListRequest"/> object.
    /// </summary>
    /// <param name="name">User defined watch list name.</param>
    /// <param name="assets">List of asset symbols for new watch list.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="name"/> or <paramref name="assets"/> argument is <c>null</c>.
    /// </exception>
    public NewWatchListRequest(
        String name,
        IEnumerable<String> assets)
        : this(name.EnsureNotNull()) =>
        _assets.AddRange(assets.EnsureNotNull().Distinct(StringComparer.Ordinal));

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
