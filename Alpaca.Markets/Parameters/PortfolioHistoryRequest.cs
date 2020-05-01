using System;
using System.Collections.Generic;
using System.Linq;

namespace Alpaca.Markets
{
    /// <summary>
    /// Encapsulates request parameters for <see cref="AlpacaTradingClient.GetPortfolioHistoryAsync(PortfolioHistoryRequest,System.Threading.CancellationToken)"/> call.
    /// </summary>
    public sealed class PortfolioHistoryRequest : Validation.IRequest
    {
        /// <summary>
        /// Gets or sets start date for desired history.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets  the end date for desired history. Default value (if <c>null</c>) is today.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the time frame value for desired history. Default value (if <c>null</c>) is 1 minute
        /// for a period shorter than 7 days, 15 minutes for a period less than 30 days, or 1 day for a longer period.
        /// </summary>
        public TimeFrame? TimeFrame { get; set; }

        /// <summary>
        /// Gets or sets period value for desired history. Default value (if <c>null</c>) is 1 month.
        /// </summary>
        public HistoryPeriod? Period { get; set; }

        /// <summary>
        /// Gets or sets flags, indicating that include extended hours included in the result.
        /// This is effective only for time frame less than 1 day.
        /// </summary>
        public Boolean? ExtendedHours { get; set; }
 
        IEnumerable<RequestValidationException> Validation.IRequest.GetExceptions() => 
            Enumerable.Empty<RequestValidationException>();
    }
}
