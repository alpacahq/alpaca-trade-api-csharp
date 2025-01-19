namespace Alpaca.Markets;

/// <summary>
/// Encapsulates data for latest options data requests on Alpaca Data API v2.
/// </summary>
public sealed class OptionChainRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="OptionChainRequest"/> object.
    /// </summary>
    /// <param name="underlyingSymbol">Option underlying symbol for data retrieval.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="underlyingSymbol"/> argument is <c>null</c>.
    /// </exception>
    public OptionChainRequest(
        String underlyingSymbol) =>
        UnderlyingSymbol = underlyingSymbol.EnsureNotNull();

    /// <summary>
    /// Gets options symbols list for data retrieval.
    /// </summary>
    [UsedImplicitly]
    public String UnderlyingSymbol { get; }

    /// <summary>
    /// Gets options feed for data retrieval.
    /// </summary>
    [UsedImplicitly]
    [ExcludeFromCodeCoverage]
    public OptionsFeed? OptionsFeed { get; set; }

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
        new UriBuilder(httpClient.BaseAddress!)
        {
            Query = await Pagination.QueryBuilder
                .AddParameter("expiration_date_gte", ExpirationDateGreaterThanOrEqualTo)
                .AddParameter("expiration_date_lte", ExpirationDateLessThanOrEqualTo)
                .AddParameter("strike_price_gte", StrikePriceGreaterThanOrEqualTo)
                .AddParameter("strike_price_lte", StrikePriceLessThanOrEqualTo)
                .AddParameter("expiration_date", ExpirationDateEqualTo)
                .AddParameter("root_symbol", RootSymbol)
                .AddParameter("feed", OptionsFeed)
                .AddParameter("type", OptionType)
                .AsStringAsync().ConfigureAwait(false)
        }.AppendPath($"snapshots/{UnderlyingSymbol}");

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Pagination.TryValidatePageSize(Pagination.MaxPageSize);
        yield return UnderlyingSymbol.TryValidateSymbolName();

        if (ExpirationDateEqualTo.HasValue && (
                ExpirationDateGreaterThanOrEqualTo.HasValue ||
                ExpirationDateLessThanOrEqualTo.HasValue))
        {
            yield return new RequestValidationException(nameof(ExpirationDateEqualTo),
                "Inconsistent expiration date filters combination.");
        }
    }
}
