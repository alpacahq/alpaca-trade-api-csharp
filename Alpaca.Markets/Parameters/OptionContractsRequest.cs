namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.ListOptionContractsAsync(OptionContractsRequest,CancellationToken)"/> call.
/// </summary>
public sealed class OptionContractsRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="OptionContractsRequest"/> object.
    /// </summary>
    /// <param name="underlyingSymbol">The symbol of the underlying asset for filtering.</param>
    public OptionContractsRequest(
        String underlyingSymbol) =>
        UnderlyingSymbol = underlyingSymbol.EnsureNotNull();
    
    /// <summary>
    /// Gets the symbol of the underlying asset for filtering.
    /// </summary>
    [UsedImplicitly]
    public String UnderlyingSymbol { get; }

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
    /// Gets or sets the desired page number. The <c>null</c> treated as 1st page.
    /// </summary>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public UInt32? PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the number of contracts to limit per page (default = 10000).
    /// </summary>
    [UsedImplicitly]
    [CLSCompliant(false)]
    public UInt32? PageSize { get; set; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = "v2/options/contracts",
            Query = await new QueryBuilder()
                .AddParameter("underlying_symbol", UnderlyingSymbol)
                .AddParameter("status", AssetStatus)
                .AddParameter("expiration_date", ExpirationDateEqualTo)
                .AddParameter("expiration_date_gte", ExpirationDateGreaterThanOrEqualTo)
                .AddParameter("expiration_date_lte", ExpirationDateLessThanOrEqualTo)
                .AddParameter("root_symbol", RootSymbol)
                .AddParameter("style", OptionStyle)
                .AddParameter("type", OptionType)
                .AddParameter("strike_price_gte", StrikePriceGreaterThanOrEqualTo)
                .AddParameter("strike_price_lte", StrikePriceLessThanOrEqualTo)
                .AddParameter("page", PageNumber)
                .AddParameter("limit", PageSize)
                .AsStringAsync().ConfigureAwait(false)
        };

    /// <inheritdoc />
    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return UnderlyingSymbol.TryValidateSymbolName();
        yield return RootSymbol?.TryValidateSymbolName();

        if (ExpirationDateEqualTo.HasValue && (
                ExpirationDateGreaterThanOrEqualTo.HasValue ||
                ExpirationDateLessThanOrEqualTo.HasValue))
        {
            yield return new RequestValidationException(nameof(ExpirationDateEqualTo),
                "Inconsistent expiration date filters combination.");
        }

        if (PageNumber is 0)
        {
            yield return new RequestValidationException(nameof(PageNumber),
                "Page number should be greater than or equal to 1.");
        }

        if (PageSize is 0 or > 10_000)
        {
            yield return new RequestValidationException(nameof(PageSize),
                "Page size value should be between 1 and 10 0000.");
        }
    }
}
