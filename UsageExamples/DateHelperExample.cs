using Alpaca.Markets;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace UsageExamples;

public enum MarketStatus
{
    PreMarket,
    Open,
    Closed,
    PostMarket
}

internal sealed class DateHelperExample
{
    public async Task Run()
    {
        // old way of finding market status which is time consuming and takes up an api call
        // my tests took an average of 4000 milliseconds for this task
        var task1watch = new Stopwatch();
        task1watch.Start();
        var oldStatus = await GetAlpacaMarketStatus(DateTime.UtcNow).ConfigureAwait(false);
        task1watch.Stop();
        Console.WriteLine($"Old method took {task1watch.ElapsedMilliseconds} milliseconds to run.");

        // new way of finding market status which takes far less time and no api call necessary
        // my tests took an average of 2 milliseconds for this task
        var task2watch = new Stopwatch();
        task2watch.Start();
        var newStatus = GetMarketStatus(DateTime.UtcNow);
        task2watch.Stop();
        Console.WriteLine($"New method took {task2watch.ElapsedMilliseconds} milliseconds to run.");
    }

    /// <summary>
    /// Old version taking in a date in local UTC time and returning cuurrent market status
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static async Task<MarketStatus> GetAlpacaMarketStatus(DateTime date)
    {
        var key = new SecretKey("...", "...");

        using var tradingClient = Environments.Paper.GetAlpacaTradingClient(key);

        var calendars = await tradingClient.ListIntervalCalendarAsync(
            CalendarRequest.GetForSingleDay(new DateOnly(date.Year, date.Month, date.Day))).ConfigureAwait(false);

        var calendar = calendars.Single();

        var easternTime = ConvertToEasternTime(date);

        var earlyOpen = calendar.Session.OpenEst;
        var lateClose = calendar.Session.CloseEst;
        var normalOpen = calendar.Trading.OpenEst;
        var normalClose = calendar.Trading.CloseEst;

        return IsTodayAWeekDay(date) ? date >= earlyOpen && date < normalOpen ? MarketStatus.PreMarket : date > normalClose && date <= lateClose ?
            MarketStatus.PostMarket : MarketStatus.Open : MarketStatus.Closed;
    }

    /// <summary>
    /// Takes a date in eastern time zone and returns the status of the current market hours
    /// </summary>
    /// <param name="date"></param>
    /// <param name="useExtendedHours"></param>
    /// <returns></returns>
    public static MarketStatus GetMarketStatus(DateTime date)
    {
        var (earlyOpen, normalOpen, normalClose, lateClose) = GetMarketHours(date);

        return earlyOpen.Hour != 0 && lateClose.Hour != 0 ? date >= earlyOpen && date < normalOpen ? MarketStatus.PreMarket : date > normalClose && date <= lateClose ?
            MarketStatus.PostMarket : MarketStatus.Open : MarketStatus.Closed;
    }

    /// <summary>
    /// Converts local UTC time into Eastern time
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static DateTime ConvertToEasternTime(DateTime utcDate)
    {
        TimeZoneInfo easternTimeZone;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            easternTimeZone = TimeZoneInfo
                .FindSystemTimeZoneById("Eastern Standard Time");
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                    || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            easternTimeZone = TimeZoneInfo
                .FindSystemTimeZoneById("America/New_York");
        }
        else
        {
            throw new ArgumentException("Not supported OS");
        }

        return TimeZoneInfo.ConvertTimeFromUtc(utcDate, easternTimeZone);
    }

    /// <summary>
    /// Takes current date and returns market hours for that date
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static (DateTime earlyOpen, DateTime normalOpen, DateTime normalClose, DateTime lateClose) GetMarketHours(DateTime date)
    {
        var defaultDate = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);

        if (IsTodayAWeekDay(date))
        {
            var holidays = GetHolidays(date.Year);
            var dateSearch = holidays.Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).ToList();
            var result = dateSearch.FirstOrDefault();

            // changed early and late hours to match new alpaca times
            var earlyOpen = new DateTime(date.Year, date.Month, date.Day, 4, 0, 0);
            var normalOpen = new DateTime(date.Year, date.Month, date.Day, 9, 30, 0);
            var normalClose = new DateTime(date.Year, date.Month, date.Day, 16, 0, 0);
            var lateClose = new DateTime(date.Year, date.Month, date.Day, 20, 0, 0);

            if (dateSearch.Count == 0)
            {
                return (earlyOpen, normalOpen, normalClose, lateClose);
            }
            else
            {
                // only checking for early hours for christmas eve and black friday and sometimes independence day
                if (result.Hour != 0 && date.Hour < result.Hour)
                {
                    var holidayClose = new DateTime(result.Year, result.Month, result.Day, result.Hour, result.Minute, result.Second);
                    return (earlyOpen, normalOpen, holidayClose, holidayClose);
                }
            }
        }

        return (defaultDate, defaultDate, defaultDate, defaultDate);
    }

    /// <summary>
    /// Finds out if today is a week day
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private static bool IsTodayAWeekDay(DateTime date = default)
    {
        return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
    }

    /// <summary>
    /// Calculates the Easter Sunday date for the selected year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private static DateTime EasterSunday(int year)
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

        return new DateTime(year, month, day, 0, 0, 0);
    }

    /// <summary>
    /// Returns a list of all market holiday dates with special closed hours or shortened market hours for selected year
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    private static HashSet<DateTime> GetHolidays(int year)
    {
        HashSet<DateTime> holidays = new();

        // New Years
        DateTime newYearsDate = AdjustForWeekendHoliday(new DateTime(year, 1, 1, 0, 0, 0));
        holidays.Add(newYearsDate);

        // Martin Luther King Jr Day
        var mlk = (from day in Enumerable.Range(1, 31)
                   where new DateTime(year, 1, day).DayOfWeek == DayOfWeek.Monday
                   select day).ElementAt(2);
        DateTime mlkDay = new(year, 1, mlk, 0, 0, 0);
        holidays.Add(mlkDay);

        // Washington's Birthday
        var leapYear = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0 && year % 100 == 0);
        var washington = (from day in Enumerable.Range(1, leapYear ? 29 : 28)
                          where new DateTime(year, 2, day).DayOfWeek == DayOfWeek.Monday
                          select day).ElementAt(2);
        DateTime washingtonDay = new(year, 2, washington, 0, 0, 0);
        holidays.Add(washingtonDay);

        // Good Friday
        var goodFriday = EasterSunday(year).AddDays(-2);
        holidays.Add(goodFriday);

        // Memorial Day -- last monday in May 
        DateTime memorialDay = new(year, 5, 31, 0, 0, 0);
        DayOfWeek dayOfWeek = memorialDay.DayOfWeek;
        while (dayOfWeek != DayOfWeek.Monday)
        {
            memorialDay = memorialDay.AddDays(-1);
            dayOfWeek = memorialDay.DayOfWeek;
        }
        holidays.Add(memorialDay);

        // Independence Day
        DateTime independenceDay = AdjustForWeekendHoliday(new DateTime(year, 7, 4, 0, 0, 0));
        holidays.Add(independenceDay);

        if (independenceDay.Day == 4 && independenceDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if independence day falls on a weekday (and not monday) then we close early at 1pm for the day before
            var xtraIndependenceDay = new DateTime(year, 7, 3, 13, 0, 0);
            holidays.Add(xtraIndependenceDay);
        }

        // Labor Day -- 1st Monday in September 
        DateTime laborDay = new(year, 9, 1, 0, 0, 0);
        dayOfWeek = laborDay.DayOfWeek;
        while (dayOfWeek != DayOfWeek.Monday)
        {
            laborDay = laborDay.AddDays(1);
            dayOfWeek = laborDay.DayOfWeek;
        }
        holidays.Add(laborDay);

        // Thanksgiving Day -- 4th Thursday in November 
        var thanksgiving = (from day in Enumerable.Range(1, 30)
                            where new DateTime(year, 11, day).DayOfWeek == DayOfWeek.Thursday
                            select day).ElementAt(3);
        DateTime thanksgivingDay = new(year, 11, thanksgiving, 0, 0, 0);
        // we close early at 1pm on black friday
        var blackFriday = new DateTime(year, 11, thanksgiving + 1, 13, 0, 0);
        holidays.Add(thanksgivingDay);

        // Christmas Day 
        DateTime christmasDay = AdjustForWeekendHoliday(new DateTime(year, 12, 25, 0, 0, 0));
        holidays.Add(christmasDay);

        // christmas eve
        if (christmasDay.Day == 25 && christmasDay.DayOfWeek != DayOfWeek.Monday)
        {
            // if christmas falls on a week day (and not on monday) then we close early at 1pm on the day before
            var christmasEve = new DateTime(year, 12, 24, 13, 0, 0);
            holidays.Add(christmasEve);
        }

        // Next year's new years check
        DateTime nextYearNewYearsDate = AdjustForWeekendHoliday(new DateTime(year + 1, 1, 1, 0, 0, 0));
        if (nextYearNewYearsDate.Year == year)
        {
            holidays.Add(nextYearNewYearsDate);
        }

        return holidays;
    }

    /// <summary>
    /// Adjusts date when market is closed because holiday falls on the weekend
    /// </summary>
    /// <param name="holiday"></param>
    /// <returns></returns>
    private static DateTime AdjustForWeekendHoliday(DateTime holiday)
    {
        if (holiday.DayOfWeek == DayOfWeek.Saturday)
        {
            return holiday.AddDays(-1);
        }
        else if (holiday.DayOfWeek == DayOfWeek.Sunday)
        {
            return holiday.AddDays(1);
        }
        else
        {
            return holiday;
        }
    }
}
