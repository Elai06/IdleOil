using System;
//using Firebase.Firestore;

namespace Utils.Extensions
{
    public static class TimestampExtensions
    {
        
        private const double SERVER_HOURS_DIFFERENCE = 3d;
        
        //TODO: добавить позже как появится Firestore
        /*
        public static int GetCurrentDay(this Timestamp timestamp)
        {
            return timestamp.ConvertToServerTime().ToUniversalTime().Day;
        }

        public static DateTime ConvertToServerTime(this Timestamp timestamp)
        {
            var dateTime = timestamp.ToDateTime();
            dateTime = dateTime.AddHours(SERVER_HOURS_DIFFERENCE);
            return dateTime;
        }
        */
    }
}