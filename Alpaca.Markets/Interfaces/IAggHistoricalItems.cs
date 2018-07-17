namespace Alpaca.Markets
{
    public interface IAggHistoricalItems<out TItem> : IHistoricalItems<TItem>
    {
        AggregationType AggregationType { get; }
    }
}