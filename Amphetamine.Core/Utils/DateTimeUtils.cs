using System;
using System.Globalization;

namespace Amphetamine.Core.Utils
{
    public static class DateTimeUtils
    {
        public static long ConvertToUnixTime(DateTime dateTime)
        {
            DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long) (dateTime.ToUniversalTime() - dateTime1).TotalSeconds;
        }

        public static bool IsValidDate(int year, int month, int day) => DateTime.TryParseExact(string.Format("{0}-{1}-{2}", (object) year, (object) month.ToString("D2"), (object) day.ToString("D2")), "yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _);
    }
}