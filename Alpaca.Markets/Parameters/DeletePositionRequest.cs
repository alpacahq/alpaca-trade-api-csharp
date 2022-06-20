namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.DeletePositionAsync(DeletePositionRequest,CancellationToken)"/> call.
/// </summary>
[UsedImplicitly]
public sealed class DeletePositionRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="DeletePositionRequest"/> object.
    /// </summary>
    /// <param name="symbol">Symbol for liquidation.</param>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="symbol"/> argument is <c>null</c>.
    /// </exception>
    public DeletePositionRequest(
        String symbol) =>
        Symbol = symbol.EnsureNotNull();

    /// <summary>
    /// Gets or sets the custom position liquidation size (if <c>null</c> the position will be liquidated completely).
    /// </summary>
    [UsedImplicitly]
    public PositionQuantity? PositionQuantity { get; set; }

    /// <summary>
    /// Gets the symbol for liquidation.
    /// </summary>
    [UsedImplicitly]
    public String Symbol { get; }

    internal async ValueTask<UriBuilder> GetUriBuilderAsync(
        HttpClient httpClient) =>
        new(httpClient.BaseAddress!)
        {
            Path = $"v2/positions/{Symbol}",
            Query = await new QueryBuilder()
                .AddParameter("percentage", PositionQuantity?.AsPercentage())
                .AddParameter("qty", PositionQuantity?.AsFractional())
                .AsStringAsync().ConfigureAwait(false)
        };

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        yield return Symbol.TryValidateSymbolName();
    }
}
