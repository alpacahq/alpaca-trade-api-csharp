using Alpaca.Markets;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace UsageExamples;

[SuppressMessage("ReSharper", "UnusedType.Global")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal sealed class DateHelper
{
    private readonly record struct MarketHours(
        DateTime EarlyOpen,
        DateTime NormalOpen,
        DateTime NormalClose,
        DateTime LateClose)
    {
        private static readonly TimeSpan _earlyOpenTime = new(4, 0, 0);

        private static readonly TimeSpan _normalOpenTime = new(9, 30, 0);

        private static readonly TimeSpan _normalCloseTime = new(16, 0, 0);

        private static readonly TimeSpan _lateCloseTime = new(20, 0, 0);

        public MarketHours(IIntervalCalendar calendar)
            : this(
                calendar.Session.OpenEst.DateTime,
                calendar.Trading.OpenEst.DateTime,
                calendar.Trading.CloseEst.DateTime,
                calendar.Session.CloseEst.DateTime)
        {
        }

        public MarketHours(DateTime date)
            : this(date, date, date, date)
        {
        }

        public static MarketHours CreateNormal(DateTime date) =>
            new(
                date.Add(_earlyOpenTime),
                date.Add(_normalOpenTime),
                date.Add(_normalCloseTime),
                date.Add(_lateCloseTime));

        public static MarketHours CreateLate(DateTime holiday) =>
            new(
                holiday.Date.Add(_earlyOpenTime),
                holiday.Date.Add(_normalOpenTime),
                holiday,
                holiday);

        /// <summary>
        /// Takes a date in eastern time zone and returns the status of the current market hours
        /// </summary>
        /// <returns></returns>
        public MarketStatus GetMarketStatus(DateTime dateTime)
        {
            if (isWeekend(dateTime))
            {
                return MarketStatus.Closed;
            }

            if (dateTime >= EarlyOpen && dateTime < NormalOpen)
            {
                return MarketStatus.PreMarket;
            }

            if (dateTime > NormalClose && dateTime <= LateClose)
            {
                return MarketStatus.PostMarket;
            }
        
            if (dateTime > LateClose || dateTime < EarlyOpen)
            {
                return MarketStatus.Closed;
            }

            return MarketStatus.Open;
        }
    }

    public enum MarketStatus
    {
        PreMarket,
        Open,
        Closed,
        PostMarket
    }

    private const string ApiKey = "REPLACEME";
    
    private const string ApiSecret = "REPLACEME";

    public static async Task Run()
    {
        // api method of finding market status which is time consuming and takes up an api call
        // my tests took an average of 4000 milliseconds for this task
        var apiWatch = Stopwatch.StartNew();
        var apiStatus = await GetAlpacaMarketStatus(DateTime.UtcNow).ConfigureAwait(false);
        apiWatch.Stop();
        Console.WriteLine(
            $"Api method took {apiWatch.ElapsedMilliseconds} milliseconds to run and returns {apiStatus}.");

        // local method of finding market status which takes far less time and no api call necessary
        // my tests took an average of 2 milliseconds for this task
        var localWatch = Stopwatch.StartNew();
        var localStatus = GetLocalMarketStatus(DateTime.UtcNow);
        localWatch.Stop();
        Console.WriteLine(
            $"Local method took {localWatch.ElapsedMilliseconds} milliseconds to run and returns {localStatus}.");
    }

    /// <summary>
    /// Api version taking in a date in local UTC time and returning current market status
    /// </summary>
    /// <returns></returns>
    public static async Task<MarketStatus> GetAlpacaMarketStatus(DateTime dateTime)
    {
        // initialize secret key and alpaca clients
        var key = new SecretKey(ApiKey, ApiSecret);
        using var tradingClient = Environments.Paper.GetAlpacaTradingClient(key);

        // get calendar for current day
        var calendars = await tradingClient.ListIntervalCalendarAsync(
            CalendarRequest.GetForSingleDay(DateOnly.FromDateTime(dateTime))).ConfigureAwait(false);
        var calendar = calendars.SingleOrDefault();

        return calendar is null ? MarketStatus.Closed : new MarketHours(calendar).GetMarketStatus(dateTime);
    }

    /// <summary>
    /// Local version taking in a date in local UTC time and returning current market status
    /// </summary>
    /// <returns></returns>
    public static MarketStatus GetLocalMarketStatus(DateTime dateTime) =>
        getMarketHours(dateTime).GetMarketStatus(dateTime);

    /// <summary>
    /// Takes current date and returns market hours for that date
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    private static MarketHours getMarketHours(DateTime dateTime)
    {
        var date = dateTime.Date;

        if (isWeekend(dateTime))
        {
            return new MarketHours(date);
        }

        var holiday = getHolidays(dateTime.Year).FirstOrDefault(x => x.Date == date);

        if (holiday == default)
        {
            return MarketHours.CreateNormal(date);
        }

        // only checking for early hours for christmas eve and black friday and sometimes independence day
        if (holiday.Hour != 0 && dateTime.Hour < holiday.Hour)
        {
            return MarketHours.CreateLate(holiday);
        }

        return new MarketHours(date);
    }

    /// <summary>
    /// Returns a list of all market holiday dates with special closed hours or shortened market hours for selected year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private static IEnumerable<DateTime> getHolidays(int year)
    {
        // new years
        yield return adjustForWeekendHoliday(new DateTime(year, 1, 1));

        yield return martinLutherKingDay(year);

        yield return washingtonDay(year);

        yield return goodFriday(year);

        yield return memorialDay(year);
        
        yield return juneteenthDay(year);

        var independenceDay = adjustForWeekendHoliday(new DateTime(year, 7, 4));
        yield return independenceDay;

        if (independenceDay.Day == 4 && independenceDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if independence day falls on a weekday (and not monday) then we close early at 1pm for the day before
            yield return new DateTime(year, 7, 3, 13, 0, 0);
        }

        yield return laborDay(year);

        var thanksgiving = thanksgivingDay(year);
        yield return thanksgiving;

        // we close early at 1pm on black friday
        yield return new DateTime(year, 11, thanksgiving.Day + 1, 13, 0, 0);

        var christmasDay = adjustForWeekendHoliday(new DateTime(year, 12, 25));
        yield return christmasDay;

        if (christmasDay.Day == 25 && christmasDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if christmas falls on a week day (and not on monday) then we close early at 1pm on the day before
            yield return new DateTime(year, 12, 24, 13, 0, 0);
        }

        var nextYearNewYearsDate = adjustForWeekendHoliday(new DateTime(year + 1, 1, 1));
        if (nextYearNewYearsDate.Year == year)
        {
            yield return nextYearNewYearsDate;
        }
    }

    private static DateTime goodFriday(int year)
    {
        var g = year % 19;
        var c = year / 100;
        var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
        var i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));
        var day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;
        var month = 3;

        // ReSharper disable once InvertIf
        if (day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateTime(year, month, day).AddDays(-2);
    }

    private static DateTime martinLutherKingDay(int year) =>
        getNext(DayOfWeek.Monday, new DateTime(year, 1, 1)).AddDays(14);

    private static DateTime washingtonDay(int year) =>
        getNext(DayOfWeek.Monday, new DateTime(year, 2, 1)).AddDays(14);

    private static DateTime memorialDay(int year) =>
        getPrev(DayOfWeek.Monday, new DateTime(year, 5, 31));
    
    private static DateTime juneteenthDay(int year) =>
        getNext(DayOfWeek.Monday, new DateTime(year, 6, 19));

    private static DateTime laborDay(int year) => 
        getNext(DayOfWeek.Monday, new DateTime(year, 9, 1));

    private static DateTime thanksgivingDay(int year) =>
        getNext(DayOfWeek.Thursday, new DateTime(year, 11, 1)).AddDays(21);

    private static bool isWeekend(DateTime date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    private static DateTime adjustForWeekendHoliday(DateTime holiday) =>
        holiday.DayOfWeek switch
        {
            DayOfWeek.Saturday => holiday.AddDays(-1),
            DayOfWeek.Sunday => holiday.AddDays(1),
            _ => holiday
        };

    private static DateTime getNext(DayOfWeek dayOfWeek, DateTime date) =>
        date.AddDays((dayOfWeek - date.DayOfWeek + 7) % 7);

    private static DateTime getPrev(DayOfWeek dayOfWeek, DateTime date) =>
        date.AddDays((dayOfWeek - date.DayOfWeek - 7) % 7);
}
