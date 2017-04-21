using System;

namespace BaseApp.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToJsonTime(this DateTime dateTime)
        {
            var dateStart = new DateTime(1970, 1, 1, 0, 0, 0);
            return (dateTime.ToUniversalTime() - dateStart).TotalMilliseconds;
        }

        public static DateTime? SpecifyKindLocal(this DateTime? dateTime)
        {
            if (dateTime == null)
                return null;

            return dateTime.Value.SpecifyKindLocal();
        }

        public static DateTime SpecifyKindLocal(this DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        }

        public static DateTime EndOfDay(this DateTime dt)
        {
            return dt.Date.AddDays(1).AddSeconds(-1);
        }

        public static DateTime? GetDate(this DateTime? dt)
        {
            if (dt == null) return null;
            return dt.Value.Date;
        }
    }
}
