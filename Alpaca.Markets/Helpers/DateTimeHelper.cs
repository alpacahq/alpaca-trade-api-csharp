namespace Alpaca.Markets;

internal static class DateTimeHelper
{
    public static String DateFormat => "yyyy-MM-dd";

    public static DateTime AsUtcDateTime(
        this in DateTime value) =>
        DateTime.SpecifyKind(value, DateTimeKind.Utc);

    public static DateTime FromUnixTimeSeconds(
        this in Int64 linuxTimeStamp) =>
        DateTimeOffset.FromUnixTimeSeconds(linuxTimeStamp).UtcDateTime;

    public static Int64 IntoUnixTimeSeconds(
        this in DateTime dateTime) =>
        new DateTimeOffset(dateTime).ToUnixTimeSeconds();
}
