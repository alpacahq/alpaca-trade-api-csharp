﻿using System;

namespace Alpaca.Markets
{
    internal static class DateTimeHelper
    {
        private static readonly DateTime _epoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTimeMilliseconds(
            Int64 linuxTimeStamp)
        {
            return _epoch.Add(TimeSpan.FromMilliseconds(linuxTimeStamp));
        }

        public static DateTime FromUnixTimeSeconds(
            Int64 linuxTimeStamp)
        {
            return _epoch.Add(TimeSpan.FromSeconds(linuxTimeStamp));
        }
    }
}
