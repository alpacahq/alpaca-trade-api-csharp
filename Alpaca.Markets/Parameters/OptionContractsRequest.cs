namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListOptionContractsAsync(OptionContractsRequest,CancellationToken)"/> call.
/// </summary>
public sealed class OptionContractsRequest : Validation.IRequest
{
    private readonly HashSet<String> _underlyingSymbols = new(StringComparer.Ordinal);

    /// <summary>
    /// Creates new instance of <see cref="OptionContractsRequest"/> object.
    /// </summary>
    public OptionContractsRequest()
    {
    }

    /// <summary>
    /// Creates new instance of <see cref="OptionContractsRequest"/> object.
    /// </summary>
    /// <param name="underlyingSymbol">The symbol of the underlying asset for filtering.</param>
    public OptionContractsRequest(
        String underlyingSymbol) =>
        _underlyingSymbols.Add(underlyingSymbol.EnsureNotNull());

    /// <summary>
    /// Creates new instance of <see cref="OptionContractsRequest"/> object.
    /// </summary>
    /// <param name="underlyingSymbols">The symbols list of the underlying asset for filtering.</param>
    public OptionContractsRequest(
        IEnumerable<String> underlyingSymbols) =>
        _underlyingSymbols.UnionWith(underlyingSymbols.EnsureNotNull());

    /// <summary>
    /// Gets the symbols list of the underlying asset for filtering.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyCollection<String> UnderlyingSymbols => _underlyingSymbols;

    /// <summary>
    /// Gets or sets filter by the asset status. By default, only active contracts are returned.
    /// </summary>
    [UsedImplicitly]
    public AssetStatus? AssetStatus { get; set; }

    /// <summary>
    /// Gets or sets filter by the exact option contract expiration date.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? ExpirationDateEqualTo { get; set; }

    /// <summary>
    /// Gets or sets filter by the expiration date greater than or equal to the specified value.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? ExpirationDateGreaterThanOrEqualTo { get; set; }

    /// <summary>
    /// Gets or sets filter by the expiration date less than or equal to the specified value.
    /// </summary>
    [UsedImplicitly]
    public DateOnly? ExpirationDateLessThanOrEqualTo { get; set; }

    /// <summary>
    /// Gets or sets filter ty the root symbol.
    /// </summary>
    [UsedImplicitly]
    public String? RootSymbol { get; set; }

    /// <summary>
    /// Gets or sets filter the option contract type.
    /// </summary>
    [UsedImplicitly]
    public OptionType? OptionType { get; set; }

    /// <summary>
    /// Gets or sets filter the option contract execution style.
    /// </summary>
    [UsedImplicitly]
    public OptionStyle? OptionStyle { get; set; }

    /// <summary>
    /// Gets or sets filter by the strike price greater than or equal to the specified value.
    /// </summary>
    [UsedImplicitly]
    public Decimal? StrikePriceGreaterThanOrEqualTo { get; set; }

    /// <summary>
    /// Gets or sets filter by the strike price less than or equal to the specified value.
    /// </summary>
    [UsedImplicitly]
    public Decimal? StrikePriceLessThanOrEqualTo { get; set; }

    /// <summary>
    /// Gets the pagination parameters for the request (page size and token).
    /// </summary>
    [UsedImplicitly]
    public Pagination Pagination { get; } = new();

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/options/contracts",
            Query = await Pagination.QueryBuilder
                .AddParameter("expiration_date_gte", ExpirationDateGreaterThanOrEqualTo)
                .AddParameter("expiration_date_lte", ExpirationDateLessThanOrEqualTo)
                .AddParameter("strike_price_gte", StrikePriceGreaterThanOrEqualTo)
                .AddParameter("strike_price_lte", StrikePriceLessThanOrEqualTo)
                .AddParameter("expiration_date", ExpirationDateEqualTo)
                .AddParameter("underlying_symbols", UnderlyingSymbols)
                .AddParameter("root_symbol", RootSymbol)
                .AddParameter("status", AssetStatus)
                .AddParameter("style", OptionStyle)
                .AddParameter("type", OptionType)
                .AsStringAsync().ConfigureAwait(false)
        };

    /// <inheritdoc />
    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Pagination.TryValidatePageSize(Pagination.MaxPageSize);
        yield return UnderlyingSymbols.TryValidateSymbolName();

        if (ExpirationDateEqualTo.HasValue && (
                ExpirationDateGreaterThanOrEqualTo.HasValue ||
                ExpirationDateLessThanOrEqualTo.HasValue))
        {
            yield return new RequestValidationException(nameof(ExpirationDateEqualTo),
                "Inconsistent expiration date filters combination.");
        }
    }
}
