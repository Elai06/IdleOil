using System;
using System.Globalization;

namespace Utils.Extensions
{
    public static class FormatTime
    {
        public static string HoursStringFormat(int totalHours)
        {
            var hours = totalHours / 3600;
            var minutes = totalHours % 3600 / 60;
            var seсonds = totalHours % 60;
            return $"{hours:0}ч {minutes:00}м";
        }
        
        public static string MinutesStringFormat(int totalMinutes)
        {
            var minutes = totalMinutes / 60;
            var seconds = totalMinutes % 60;

            return $"{minutes:00}m{seconds:00}s";
        }
        
        public static int HoursIntFormat(int totalHours)
        {
            return totalHours * 3600;
        }
        
        public static int MinutesIntFormat(int totalMinutes)
        {
            return totalMinutes * 60;
        }

        public static DateTime StrToDateTime(string strTime)
        {
            var aaa = Convert.ToDateTime(strTime);
            return aaa;
        }
        
        public static string DateTimeToStr(DateTime time)
        {
            return time.ToString(CultureInfo.CurrentCulture);
        }
    }
}