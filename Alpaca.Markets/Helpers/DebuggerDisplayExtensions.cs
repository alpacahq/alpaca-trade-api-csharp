namespace Alpaca.Markets;

[ExcludeFromCodeCoverage]
internal static class DebuggerDisplayExtensions
{
    internal static String ToDebuggerDisplayString<TItem>(
        this IPage<TItem> page) =>
        $"{nameof(IPage<TItem>)}<{typeof(TItem).Name}> {{ Count = {page.Items.Count}, Symbol = \"{page.Symbol}\", NextPageToken = \"{page.NextPageToken}\" }}";

    internal static String ToDebuggerDisplayString<TItem>(
        this IMultiPage<TItem> page) =>
        $"{nameof(IPage<TItem>)}<{typeof(TItem).Name}> {{ Count = {page.Items.Count}, NextPageToken = \"{page.NextPageToken}\" }}";

    internal static String ToDebuggerDisplayString<TItem>(
        this IDictionaryPage<TItem> page) =>
        $"{nameof(IDictionaryPage<TItem>)}<{typeof(TItem).Name}> {{ Count = {page.Items.Count}, NextPageToken = \"{page.NextPageToken}\" }}";

    internal static String ToDebuggerDisplayString(
        this IBar bar) =>
        $"{nameof(IBar)} {{ TimeUtc = {bar.TimeUtc:O}, Symbol = \"{bar.Symbol}\", Open = {bar.Open}, High = {bar.High}, Low = {bar.Low}, Close = {bar.Close} }}";

    internal static String ToDebuggerDisplayString(
        this IQuote quote) =>
        $"{nameof(IQuote)} {{ TimeUtc = {quote.TimestampUtc:O}, Symbol = \"{quote.Symbol}\", AskPrice = {quote.AskPrice}, AskSize = {quote.AskSize}, BidPrice = {quote.BidPrice}, BidSize = {quote.BidSize} }}";

    internal static String ToDebuggerDisplayString(
        this ITrade trade) =>
        $"{nameof(ITrade)} {{ TimeUtc = {trade.TimestampUtc:O}, Symbol = \"{trade.Symbol}\", ID = {trade.TradeId}, Price = {trade.Price}, Size = {trade.Size} }}";

    internal static String ToDebuggerDisplayString(
        this ISnapshot snapshot) =>
        $"{nameof(ISnapshot)} {{ Symbol = \"{snapshot.Symbol}\", {snapshot.Quote?.ToDebuggerDisplayString()}, {snapshot.Trade?.ToDebuggerDisplayString()} }}";

    internal static String ToDebuggerDisplayString(
        this IOptionSnapshot snapshot) =>
        $"{nameof(IOptionSnapshot)} {{ Symbol = \"{snapshot.Symbol}\", {snapshot.Quote?.ToDebuggerDisplayString()}, {snapshot.Trade?.ToDebuggerDisplayString()} }}";
}
