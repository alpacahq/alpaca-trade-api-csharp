namespace Alpaca.Markets.Extensions;

/// <summary>
/// Extends the <see cref="IAlpacaDataSubscription{TItem}"/> interface with disposing support
/// so caller can use instance of this interface in <c>using</c> and <c>await using</c> statements.
/// </summary>
/// <typeparam name="TItem"></typeparam>
public interface IDisposableAlpacaDataSubscription<out TItem>
    : IAlpacaDataSubscription<TItem>, IAsyncDisposable, IDisposable;
