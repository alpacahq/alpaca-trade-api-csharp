namespace Alpaca.Markets;

/// <summary>
/// Encapsulates account activity information from Alpaca REST API.
/// </summary>
public interface IAccountActivity
{
    /// <summary>
    /// Activity type.
    /// </summary>
    [UsedImplicitly]
    AccountActivityType ActivityType { get; }

    /// <summary>
    /// An ID for the activity, always in "{date}::{uuid}" format. Can be sent as page_token in requests to facilitate the paging of results.
    /// </summary>
    [UsedImplicitly]
    String ActivityId { get; }

    /// <summary>
    /// An activity timestamp (date and time) from <see cref="ActivityId"/> in the UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime ActivityDateTimeUtc { get; }

    /// <summary>
    /// An activity unique identifier from <see cref="ActivityId"/>.
    /// </summary>
    [UsedImplicitly]
    Guid ActivityGuid { get; }

    /// <summary>
    /// The symbol of the security involved with the activity. Not present for all activity types.
    /// </summary>
    [UsedImplicitly]
    String? Symbol { get; }

    /// <summary>
    /// The date on which the activity occurred or on which the transaction associated with the activity settled in the UTC.
    /// </summary>
    [UsedImplicitly]
    DateOnly? ActivityDate { get; }

    /// <summary>
    /// The net amount of money (positive or negative) associated with the activity.
    /// </summary>
    [UsedImplicitly]
    Decimal? NetAmount { get; }

    /// <summary>
    /// For dividend activities, the average amount paid per share. Not present for other activity types.
    /// </summary>
    [UsedImplicitly]
    Decimal? PerShareAmount { get; }

    /// <summary>
    /// The number of shares that contributed to the transaction (with the fractional part). Not present for all activity types.
    /// </summary>
    [UsedImplicitly]
    Decimal? Quantity { get; }

    /// <summary>
    /// The cumulative quantity of shares involved in the execution (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal? CumulativeQuantity { get; }

    /// <summary>
    /// For partially_filled orders, the quantity of shares that are left to be filled (with the fractional part).
    /// </summary>
    [UsedImplicitly]
    Decimal? LeavesQuantity { get; }

    /// <summary>
    /// The number of shares that contributed to the transaction (rounded to the nearest integer). Not present for all activity types.
    /// </summary>
    [UsedImplicitly]
    Int64? IntegerQuantity { get; }

    /// <summary>
    /// The cumulative quantity of shares involved in the execution (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64? IntegerCumulativeQuantity { get; }

    /// <summary>
    /// For partially_filled orders, the quantity of shares that are left to be filled (rounded to the nearest integer).
    /// </summary>
    [UsedImplicitly]
    Int64? IntegerLeavesQuantity { get; }

    /// <summary>
    /// The per-share price that a trade was executed at.
    /// </summary>
    [UsedImplicitly]
    Decimal? Price { get; }

    /// <summary>
    /// The order side of a trade execution.
    /// </summary>
    [UsedImplicitly]
    OrderSide? Side { get; }

    /// <summary>
    /// The time at which an execution occurred in the UTC.
    /// </summary>
    [UsedImplicitly]
    DateTime? TransactionTimeUtc { get; }

    /// <summary>
    /// The type of trade event associated with an execution.
    /// </summary>
    [UsedImplicitly]
    TradeEvent? Type { get; }

    /// <summary>
    /// Gets the id for the order that filled (for the trade activity).
    /// </summary>
    [UsedImplicitly]
    public Guid? OrderId { get; }
}
