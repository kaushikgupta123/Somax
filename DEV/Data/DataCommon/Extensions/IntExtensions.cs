using System;

namespace Common.Extensions
{
    public static class IntExtensions
    {
        public static string ToFileSize(this int val) 
        {
            int kilobyte = 1024;
            double bytes = Convert.ToDouble(val);

            if (bytes >= Math.Pow(kilobyte, 2)) 
            {
                return string.Concat(Math.Round(bytes / Math.Pow(kilobyte, 2), 1), " MB");
            }
            else if (bytes >= kilobyte) 
            {
                return string.Concat(Math.Round(bytes / kilobyte, 1), " KB");
            }
            else
            {
                return string.Concat(bytes, " Bytes");
            }
        }

        public static long ConvertMinuteToMilliSecond(this int val)
        {
            return val * 60 * 1000;
        }
 
    }
}
