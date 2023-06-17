using System;

namespace Utils.Extensions
{
    public static class DataTimeExtensions
    {
        private static readonly DateTime Timestamp = new DateTime(1970, 1, 1);
        
        /// <summary>
        /// Returns the TimeSpan that has elapsed since January 1, 1970 to day
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeStamp(this DateTime date)
        {
            return DateTime.Now.Subtract(Timestamp);
        }
    }
}