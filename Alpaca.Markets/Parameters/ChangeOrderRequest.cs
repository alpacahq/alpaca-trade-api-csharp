namespace Alpaca.Markets;

/// <summary>
/// Encapsulates request parameters for <see cref="IAlpacaTradingClient.PatchOrderAsync(ChangeOrderRequest,CancellationToken)"/> call.
/// </summary>
public sealed class ChangeOrderRequest : Validation.IRequest
{
    /// <summary>
    /// Creates new instance of <see cref="ChangeOrderRequest"/> object.
    /// </summary>
    /// <param name="orderId">Server side order identifier.</param>
    public ChangeOrderRequest(
        Guid orderId) =>
        OrderId = orderId;

    /// <summary>
    /// Gets server side order identifier.
    /// </summary>
    [JsonIgnore]
    [UsedImplicitly]
    public Guid OrderId { get; }

    /// <summary>
    /// Gets or sets updated order quantity or <c>null</c> if quantity is not changed.
    /// </summary>
    [JsonProperty(PropertyName = "qty", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Int64? Quantity { get; set; }

    /// <summary>
    /// Gets or sets updated order duration or <c>null</c> if duration is not changed.
    /// </summary>
    [JsonProperty(PropertyName = "time_in_force", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public TimeInForce? Duration { get; set; }

    /// <summary>
    /// Gets or sets updated order limit price or <c>null</c> if limit price is not changed.
    /// </summary>
    [JsonProperty(PropertyName = "limit_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? LimitPrice { get; set; }

    /// <summary>
    /// Gets or sets updated order stop price or <c>null</c> if stop price is not changed.
    /// </summary>
    [JsonProperty(PropertyName = "stop_price", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public Decimal? StopPrice { get; set; }

    /// <summary>
    /// Gets or sets updated client order ID or <c>null</c> if client order ID is not changed.
    /// </summary>
    [JsonProperty(PropertyName = "client_order_id", Required = Required.Default, NullValueHandling = NullValueHandling.Ignore)]
    public String? ClientOrderId { get; set; }

    internal String GetEndpointUri() => $"v2/orders/{OrderId:D}";

    IEnumerable<RequestValidationException?> Validation.IRequest.GetExceptions()
    {
        ClientOrderId = ClientOrderId?.TrimClientOrderId();
        yield return Quantity.TryValidateQuantity();
    }
}
