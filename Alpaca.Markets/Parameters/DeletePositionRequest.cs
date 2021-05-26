using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.DeletePositionAsync(DeletePositionRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class DeletePositionRequest : Validation.IRequest
    {
        /// <summary>
        /// Creates new instance of <see cref="DeletePositionRequest"/> object.
        /// </summary>
        /// <param name="symbol">Symbol for liquidation.</param>
        public DeletePositionRequest(
            String symbol)
        {
            Symbol = symbol ?? throw new ArgumentException(
                "Symbol name cannot be null.", nameof(symbol));
        }

        /// <summary>
        /// Gets or sets the custom position liquidation size (if missed the position will be liquidated completely).
        /// </summary>
        public PositionQuantity? PositionQuantity { get; set; }

        /// <summary>
        /// Gets the symbol for liquidation.
        /// </summary>
        public String Symbol { get; }

        internal UriBuilder GetUriBuilder(
            HttpClient httpClient) =>
            new (httpClient.BaseAddress)
            {
                Path = $"v2/positions/{Symbol}",
                Query = new QueryBuilder()
                    .AddParameter("percentage", PositionQuantity?.AsPercentage())
                    .AddParameter("qty", PositionQuantity?.AsFractional())
            };

        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions()
        {
            if (String.IsNullOrEmpty(Symbol))
            {
                yield return new RequestValidationException(
                    "Symbols shouldn't be empty.", nameof(Symbol));
            }
        }
    }
}
