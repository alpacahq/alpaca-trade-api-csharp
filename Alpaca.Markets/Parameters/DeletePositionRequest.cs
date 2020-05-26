using System;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.DeletePositionAsync(DeletePositionRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class DeletePositionRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="DeletePositionRequest"/> object.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        public DeletePositionRequest(
            String symbol)
        {
            Symbol = symbol;
        }

        /// <summary>
        /// Gets the symbol for liquidation.
        /// </summary>
        public String Symbol { get; }
    }
}
