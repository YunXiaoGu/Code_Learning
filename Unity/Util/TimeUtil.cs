using System;

namespace Util
{
    public static class TimeUtil
    {
        public static string FormatTime(DateTime dateTime)
        {
            return $"{dateTime.Year:D4}-{dateTime.Month:D2}-{dateTime.Day:D2} {dateTime.Hour:D2}:{dateTime.Minute:D2}:{dateTime.Second:D2}";
        }
    }
}