namespace Alpaca.Markets;

/// <summary>
/// Encapsulates the historical corporate action response page with different corporate action types inside.
/// </summary>
public interface ICorporateActionsResponse
{
    /// <summary>
    /// Gets list of <see cref="IStockAndCashMerger"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IStockAndCashMerger> StockAndCashMergers { get; }

    /// <summary>
    /// Gets list of <see cref="IRightsDistribution"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IRightsDistribution> RightsDistributions { get; }

    /// <summary>
    /// Gets list of <see cref="IWorthlessRemoval"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IWorthlessRemoval> WorthlessRemovals { get; }

    /// <summary>
    /// Gets list of <see cref="IStockDividend"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IStockDividend> StockDividends { get; }

    /// <summary>
    /// Gets list of <see cref="ICashDividend"/> items from the response page.
    /// </summary>
    public IReadOnlyList<ICashDividend> CashDividends { get; }

    /// <summary>
    /// Gets list of <see cref="IReverseSplit"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IReverseSplit> ReverseSplits { get; }

    /// <summary>
    /// Gets list of <see cref="IForwardSplit"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IForwardSplit> ForwardSplits { get; }

    /// <summary>
    /// Gets list of <see cref="IStockMerger"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IStockMerger> StockMergers { get; }

    /// <summary>
    /// Gets list of <see cref="INameChange"/> items from the response page.
    /// </summary>
    public IReadOnlyList<INameChange> NameChanges { get; }

    /// <summary>
    /// Gets list of <see cref="ICashMerger"/> items from the response page.
    /// </summary>
    public IReadOnlyList<ICashMerger> CashMergers { get; }

    /// <summary>
    /// Gets list of <see cref="IRedemption"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IRedemption> Redemptions { get; }

    /// <summary>
    /// Gets list of <see cref="IUnitSplit"/> items from the response page.
    /// </summary>
    public IReadOnlyList<IUnitSplit> UnitSplits { get; }

    /// <summary>
    /// Gets list of <see cref="ISpinOff"/> items from the response page.
    /// </summary>
    public IReadOnlyList<ISpinOff> SpinOffs { get; }

    /// <summary>
    /// Gets the next page token for continuation. If value of this property
    /// equals to <c>null</c> this page is the last one and no more data is available.
    /// </summary>
    [UsedImplicitly]
    public String? NextPageToken { get; }
}
