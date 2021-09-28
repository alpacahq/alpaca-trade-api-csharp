using System;

namespace Alpaca.Markets.Extensions
{
    internal static class HistoricalRequestExtensions
    {
        private const UInt32 MaxPageSize = 10_000;

        public static DateTime GetValidatedFrom(
            this HistoricalRequestBase request) =>
            getValidatedDate(request.TimeInterval.From, nameof(request.TimeInterval.From));

        public static DateTime GetValidatedInto(
            this HistoricalRequestBase request) =>
            getValidatedDate(request.TimeInterval.Into, nameof(request.TimeInterval.Into));

        public static UInt32 GetPageSize(
            this HistoricalRequestBase request) =>
            request.Pagination.Size ?? MaxPageSize;

        private static DateTime getValidatedDate(
            DateTime? date,
            String paramName) =>
            date ?? throw new ArgumentException(
                "Invalid request time interval - empty date", paramName);
    }
}
