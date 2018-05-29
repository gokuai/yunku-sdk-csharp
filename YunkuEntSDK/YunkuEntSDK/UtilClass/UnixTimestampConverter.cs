using System;

namespace YunkuEntSDK.UtilClass
{
    public class UnixTimestampConverter
    {
        public static readonly DateTime UnixTimestampLocalZero = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeZoneInfo.Local);
        public static readonly DateTime UnixTimestampUtcZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long ConvertLocalToTimestamp(DateTime datetime)
        {
            return (long)(datetime - UnixTimestampLocalZero).TotalMilliseconds;
        }

        public static long ConvertUtcToTimestamp(DateTime datetime)
        {
            return (long)(datetime - UnixTimestampUtcZero).TotalMilliseconds;
        }

        public static DateTime ConvertLocalFromTimestamp(long timestamp)
        {
            return UnixTimestampLocalZero.AddMilliseconds(timestamp);
        }

        public static DateTime ConvertUtcFromTimestamp(long timestamp)
        {
            return UnixTimestampUtcZero.AddMilliseconds(timestamp);
        }
    } 
}
