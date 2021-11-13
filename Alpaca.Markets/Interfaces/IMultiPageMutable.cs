namespace Alpaca.Markets;

internal interface IMultiPageMutable<TItems> : IMultiPage<TItems>
{
    public new String? NextPageToken { [UsedImplicitly] get; set; }

    public new IReadOnlyDictionary<String, IReadOnlyList<TItems>> Items { [UsedImplicitly] get; set; }
}
