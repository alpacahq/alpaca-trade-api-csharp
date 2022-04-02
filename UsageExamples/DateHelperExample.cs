using Alpaca.Markets;
using System.Diagnostics;

namespace UsageExamples;

public enum MarketStatus
{
    PreMarket,
    Open,
    Closed,
    PostMarket
}

internal sealed class DateHelper
{
    private const string API_KEY = "REPLACEME";
    private const string API_SECRET = "REPLACEME";

    private static readonly TimeSpan earlyOpenTS = new(4, 0, 0);
    private static readonly TimeSpan normalOpenTS = new(9, 30, 0);
    private static readonly TimeSpan normalCloseTS = new(16, 0, 0);
    private static readonly TimeSpan lateCloseTS = new(20, 0, 0);

    public static async Task Run()
    {
        // api method of finding market status which is time consuming and takes up an api call
        // my tests took an average of 4000 milliseconds for this task
        var task1watch = Stopwatch.StartNew();
        var apiStatus = await GetAlpacaMarketStatus(DateTime.UtcNow).ConfigureAwait(false);
        task1watch.Stop();
        Console.WriteLine($"Api method took {task1watch.ElapsedMilliseconds} milliseconds to run.");

        // local method of finding market status which takes far less time and no api call necessary
        // my tests took an average of 2 milliseconds for this task
        var task2watch = Stopwatch.StartNew();
        var localStatus = GetLocalMarketStatus(DateTime.UtcNow);
        task2watch.Stop();
        Console.WriteLine($"Local method took {task2watch.ElapsedMilliseconds} milliseconds to run.");
    }

    /// <summary>
    /// Api version taking in a date in local UTC time and returning current market status
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static async Task<MarketStatus> GetAlpacaMarketStatus(DateTime date)
    {
        // initialize secret key and alpaca clients
        var key = new SecretKey(API_KEY, API_SECRET);
        using var tradingClient = Environments.Paper.GetAlpacaTradingClient(key);

        // get calendar for current day
        var calendars = await tradingClient.ListIntervalCalendarAsync(CalendarRequest.GetForSingleDay(DateOnly.FromDateTime(date))).ConfigureAwait(false);
        var calendar = calendars.Single();

        // get market hours in eastern time
        var earlyOpen = calendar.Session.OpenEst;
        var lateClose = calendar.Session.CloseEst;
        var normalOpen = calendar.Trading.OpenEst;
        var normalClose = calendar.Trading.CloseEst;

        return GetMarketStatus(new MarketHours(earlyOpen.DateTime, normalOpen.DateTime, normalClose.DateTime, lateClose.DateTime), date);
    }

    /// <summary>
    /// Local version taking in a date in local UTC time and returning current market status
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static MarketStatus GetLocalMarketStatus(DateTime date) => GetMarketStatus(GetMarketHours(date), date);

    /// <summary>
    /// Takes a date in eastern time zone and returns the status of the current market hours
    /// </summary>
    /// <param name="date"></param>
    /// <param name="useExtendedHours"></param>
    /// <returns></returns>
    public static MarketStatus GetMarketStatus(MarketHours marketHours, DateTime date)
    {
        if (IsTodayAWeekDay(date))
        {
            if (date >= marketHours.EarlyOpen && date < marketHours.NormalOpen)
            {
                return MarketStatus.PreMarket;
            }
            else if (date > marketHours.NormalClose && date <= marketHours.LateClose)
            {
                return MarketStatus.PostMarket;
            }
            else if (date > marketHours.LateClose || date < marketHours.EarlyOpen)
            {
                return MarketStatus.Closed;
            }
            else
            {
                return MarketStatus.Open;
            }
        }
        else
        {
            return MarketStatus.Closed;
        }
    }

    public readonly record struct MarketHours(DateTime EarlyOpenDate, DateTime NormalOpenDate, DateTime NormalCloseDate, DateTime LateCloseDate)
    {
        public DateTime EarlyOpen => EarlyOpenDate;
        public DateTime NormalOpen => NormalOpenDate;
        public DateTime NormalClose => NormalCloseDate;
        public DateTime LateClose => LateCloseDate;
    }

    /// <summary>
    /// Takes current date and returns market hours for that date
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static MarketHours GetMarketHours(DateTime date)
    {
        var defaultDate = date.Date;

        if (IsTodayAWeekDay(date))
        {
            var holidays = GetHolidays(date.Year);
            var result = holidays.Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).FirstOrDefault();

            // changed early and late hours to match new alpaca times
            var earlyOpen = date.Add(earlyOpenTS);
            var normalOpen = date.Add(normalOpenTS);
            var normalClose = date.Add(normalCloseTS);
            var lateClose = date.Add(lateCloseTS);

            if (result == default)
            {
                return new MarketHours(earlyOpen, normalOpen, normalClose, lateClose);
            }
            else
            {
                // only checking for early hours for christmas eve and black friday and sometimes independence day
                if (result.Hour != 0 && date.Hour < result.Hour)
                {
                    return new MarketHours(earlyOpen, normalOpen, result, result);
                }
            }
        }

        return new MarketHours(defaultDate, defaultDate, defaultDate, defaultDate);
    }

    private static bool IsTodayAWeekDay(DateTime date = default) => date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;

    private static DateTime GoodFriday(int year)
    {
        int g = year % 19;
        int c = year / 100;
        int h = (c - c / 4 - ((8 * c + 13) / 25) + 19 * g + 15) % 30;
        int i = h - h / 28 * (1 - h / 28 * (29 / (h + 1)) * ((21 - g) / 11));
        int day = i - (year + year / 4 + i + 2 - c + c / 4) % 7 + 28;
        int month = 3;

        if (day > 31)
        {
            month++;
            day -= 31;
        }

        return new DateTime(year, month, day, 0, 0, 0).AddDays(-2);
    }

    private static DateTime MartinLutherKingDay(int year)
    {
        var mlk = (from day in Enumerable.Range(1, 31)
                   where new DateTime(year, 1, day).DayOfWeek == DayOfWeek.Monday
                   select day).ElementAt(2);

        return new(year, 1, mlk, 0, 0, 0);
    }

    private static DateTime WashingtonDay(int year)
    {
        var leapYear = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0 && year % 100 == 0);
        var washington = (from day in Enumerable.Range(1, leapYear ? 29 : 28)
                          where new DateTime(year, 2, day).DayOfWeek == DayOfWeek.Monday
                          select day).ElementAt(2);

        return new(year, 2, washington, 0, 0, 0);
    }

    private static DateTime MemorialDay(int year)
    {
        var memorialDay = new DateTime(year, 5, 31, 0, 0, 0);
        var dayOfWeek = memorialDay.DayOfWeek;

        while (dayOfWeek != DayOfWeek.Monday)
        {
            memorialDay = memorialDay.AddDays(-1);
            dayOfWeek = memorialDay.DayOfWeek;
        }

        return memorialDay;
    }

    private static DateTime LaborDay(int year)
    {
        var laborDay = new DateTime(year, 9, 1, 0, 0, 0);
        var dayOfWeek = laborDay.DayOfWeek;
        
        while (dayOfWeek != DayOfWeek.Monday)
        {
            laborDay = laborDay.AddDays(1);
            dayOfWeek = laborDay.DayOfWeek;
        }

        return laborDay;
    }

    private static DateTime ThanksgivingDay(int year)
    {
        var thanksgiving = (from day in Enumerable.Range(1, 30)
                            where new DateTime(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                            select day).ElementAt(3);

        return new(year, 11, thanksgiving, 0, 0, 0);
    }

    /// <summary>
    /// Returns a list of all market holiday dates with special closed hours or shortened market hours for selected year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private static IEnumerable<DateTime> GetHolidays(int year)
    {
        // new years
        yield return AdjustForWeekendHoliday(new DateTime(year, 1, 1, 0, 0, 0));

        yield return MartinLutherKingDay(year);

        yield return WashingtonDay(year);

        yield return GoodFriday(year);

        yield return MemorialDay(year);

        var independenceDay = AdjustForWeekendHoliday(new DateTime(year, 7, 4, 0, 0, 0));
        yield return independenceDay;

        if (independenceDay.Day == 4 && independenceDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if independence day falls on a weekday (and not monday) then we close early at 1pm for the day before
            yield return new DateTime(year, 7, 3, 13, 0, 0);
        }

        yield return LaborDay(year);

        var thanksgiving = ThanksgivingDay(year);
        yield return thanksgiving;

        // we close early at 1pm on black friday
        yield return new DateTime(year, 11, thanksgiving.Day + 1, 13, 0, 0);

        var christmasDay = AdjustForWeekendHoliday(new DateTime(year, 12, 25, 0, 0, 0));
        yield return christmasDay;

        if (christmasDay.Day == 25 && christmasDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if christmas falls on a week day (and not on monday) then we close early at 1pm on the day before
            yield return new DateTime(year, 12, 24, 13, 0, 0);
        }

        var nextYearNewYearsDate = AdjustForWeekendHoliday(new DateTime(year + 1, 1, 1, 0, 0, 0));
        if (nextYearNewYearsDate.Year == year)
        {
            yield return nextYearNewYearsDate;
        }
    }

    private static DateTime AdjustForWeekendHoliday(DateTime holiday)
    {
        return holiday.DayOfWeek switch
        {
            DayOfWeek.Saturday => holiday.AddDays(-1),
            DayOfWeek.Sunday => holiday.AddDays(1),
            _ => holiday,
        };
    }
}
