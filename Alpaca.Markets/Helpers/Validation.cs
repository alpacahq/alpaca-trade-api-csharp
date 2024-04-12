namespace Alpaca.Markets;

internal static class Validation
{
    private const Int32 ClientOrderIdMaxLength = 128;

    private const Int32 WatchListNameMaxLength = 64;

    private const String WatchListNameShouldBe64CharactersLengthMessage =
        "Watch list name should be from 1 to 64 characters length.";

    private const String ListShouldContainsAtLeastOneItemMessage =
        "Symbols list should contains at least one item.";

    private const String RequestPageSizeTooBigOrTooSmallMessage =
        "Request page size too big or too small.";

    private const String OrderQuantityShouldBePositiveMessage =
        "Order quantity should be positive value.";

    private const String CollectionShouldNotBeEmptyMessage =
        "Collection should contains at least one item.";

    private const String IntervalShouldNotBeOpenMessage =
        "You should specify both start and end of the interval.";

    private const String SymbolShouldNotBeEmptyMessage =
        "Symbol shouldn't be empty.";

    internal interface IRequest
    {
        /// <summary>
        /// Gets all validation exceptions (inconsistent request data errors).
        /// </summary>
        /// <returns>Lazy-evaluated list of validation errors.</returns>
        IEnumerable<RequestValidationException?> GetExceptions();
    }

    public static TRequest Validate<TRequest>(
        this TRequest request)
        where TRequest : class, IRequest
    {
        var exception = new AggregateException(
            // ReSharper disable once RedundantEnumerableCastCall
            request.GetExceptions().OfType<RequestValidationException>());
        if (exception.InnerExceptions.Count != 0)
        {
            throw exception.InnerExceptions.Count == 1
                ? exception.InnerExceptions[0]
                : exception;
        }

        return request;
    }

    public static RequestValidationException? TryValidateSymbolName(
        this String symbolName,
        [CallerArgumentExpression(nameof(symbolName))] String propertyName = "") =>
        String.IsNullOrWhiteSpace(symbolName)
            ? new RequestValidationException(SymbolShouldNotBeEmptyMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateSymbolName(
        this IEnumerable<String> symbolNames,
        [CallerArgumentExpression(nameof(symbolNames))] String propertyName = "") =>
        symbolNames.Any(String.IsNullOrWhiteSpace)
            ? new RequestValidationException(SymbolShouldNotBeEmptyMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateQuantity(
        this OrderQuantity quantity,
        [CallerArgumentExpression(nameof(quantity))] String propertyName = "") =>
        quantity.Value <= 0M
            ? new RequestValidationException(OrderQuantityShouldBePositiveMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateQuantity(
        this Int64? quantity,
        [CallerArgumentExpression(nameof(quantity))] String propertyName = "") =>
        quantity <= 0M
            ? new RequestValidationException(OrderQuantityShouldBePositiveMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidatePageSize(
        this Pagination pagination,
        UInt32 maxPageSize,
        [CallerArgumentExpression(nameof(pagination))] String propertyName = "") =>
        pagination.Size < Pagination.MinPageSize || pagination.Size > maxPageSize
            ? new RequestValidationException(RequestPageSizeTooBigOrTooSmallMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateSymbolsList(
        this IReadOnlyCollection<String> symbolNames,
        [CallerArgumentExpression(nameof(symbolNames))] String propertyName = "") =>
        symbolNames.Count == 0
            ? new RequestValidationException(ListShouldContainsAtLeastOneItemMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateWatchListName(
        this String? watchListName,
        [CallerArgumentExpression(nameof(watchListName))] String propertyName = "") =>
        isWatchListNameInvalid(watchListName)
            ? new RequestValidationException(WatchListNameShouldBe64CharactersLengthMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateWatchListName<TKey>(
        this TKey watchListName,
        [CallerArgumentExpression(nameof(watchListName))] String propertyName = "") =>
        watchListName is String stringKey && isWatchListNameInvalid(stringKey)
            ? new RequestValidationException(WatchListNameShouldBe64CharactersLengthMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateCollection<TItem>(
        this IReadOnlyCollection<TItem> collection,
        [CallerArgumentExpression(nameof(collection))] String propertyName = "") =>
        collection.Count == 0
            ? new RequestValidationException(CollectionShouldNotBeEmptyMessage, propertyName)
            : null;

    public static RequestValidationException? TryValidateInterval<TItem>(
        this Interval<TItem> interval,
        [CallerArgumentExpression(nameof(interval))] String propertyName = "")
        where TItem : struct, IComparable<TItem> =>
        interval.IsOpen()
            ? new RequestValidationException(IntervalShouldNotBeOpenMessage, propertyName)
            : null;

    public static String? ValidateWatchListName(
        this String? watchListName,
        [CallerArgumentExpression(nameof(watchListName))] String propertyName = "") =>
        isWatchListNameInvalid(watchListName)
            ? throw new ArgumentException(WatchListNameShouldBe64CharactersLengthMessage, propertyName)
            : watchListName;

    public static String? TrimClientOrderId(
        this String? clientOrderId) =>
        clientOrderId?.Length > ClientOrderIdMaxLength
            ? clientOrderId[..ClientOrderIdMaxLength]
            : clientOrderId;

    private static Boolean isWatchListNameInvalid(
        this String? watchListName) =>
        watchListName is null ||
#pragma warning disable CA1508 // Avoid dead conditional code
            String.IsNullOrEmpty(watchListName) ||
#pragma warning restore CA1508 // Avoid dead conditional code
            watchListName.Length > WatchListNameMaxLength;
}
