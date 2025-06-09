using System;
using System.Linq;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Treat empty strings as zero instead of failing to parse.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TryParse(this string str, out decimal val) 
        {
            if (decimal.TryParse(str, out val))
            {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(str))
            {
                val = 0.0m;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Treat empty strings as zero instead of failing to parse.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TryParse(this string str, out int val)
        {
            if (int.TryParse(str, out val))
            {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(str))
            {
                val = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Treat empty strings as zero instead of failing to parse.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TryParse(this string str, out long val)
        {
            if (long.TryParse(str, out val))
            {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(str))
            {
                val = 0;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Treat empty strings as min value instead of failing to parse.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool TryParse(this string str, out DateTime val)
        {
            if (DateTime.TryParse(str, out val))
            {
                return true;
            }
            else if (string.IsNullOrWhiteSpace(str))
            {
                val = DateTime.MinValue;
                return true;
            }
            return false;
        }

        public static decimal ToDecimal(this string str)
        {
            decimal val;
            return decimal.TryParse(str, out val) ? val : 0.0m;
        }

        public static int ToInt(this string str)
        {
            int val;
            return int.TryParse(str, out val) ? val : 0;
        }

        public static short ToShort(this string str)
        {
            short val;

            // No idea why, but the compiler complains about casting to int if the trinary operator is used...
            if (short.TryParse(str, out val))
            {
                return val;
            }
            else
            {
                return 0;
            }
        }


        public static float ToFloat(this string str)
        {
            float val;

            if (float.TryParse(str, out val))
            {
                return val;
            }
            else
            {
                return 0;
            }
        }

        public static long ToLong(this string str)
        {
            long val;
            return long.TryParse(str, out val) ? val : 0;
        }

        public static Guid ToGuid(this string str)
        {
            Guid val;
            return Guid.TryParse(str, out val) ? val : Guid.Empty;
        }

        public static DateTime? ToDateTimeNullable(this string str)
        {
            DateTime tempdate;
            if (DateTime.TryParse(str, out tempdate))
                return tempdate;
            else
                return null;
        }

        public static DateTime ToDateTime(this string str)
        {
            DateTime val;
            return DateTime.TryParse(str, out val) ? val : DateTime.MinValue;
        }

        public static DateTime ToDateTimeWithDefault(this string str, DateTime defaultTime)
        {
            DateTime val;
            return DateTime.TryParse(str, out val) ? val : defaultTime;
        }

        public static bool ToBool(this string str)
        {
            bool val;
            return bool.TryParse(str, out val) ? val : false;
        }

        public static string ToCSVFormat(this string str)
        {
            if (str.Contains('"') || str.Contains(',') || str.Contains(Environment.NewLine))
            {
                string csvString = str.Replace("\"", "\"\""); // " -> ""
                return string.Format("\"{0}\"", csvString);
            }
            else
            {
                return str;
            }
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        //public static decimal? ToNullableDecimal(this string str)
        //{
        //    decimal val;
        //    return decimal.TryParse(str, out val) ? (decimal?)val : null;
        //}

        //public static int? ToNullableInteger(this string str)
        //{
        //    int val;
        //    return int.TryParse(str, out val) ? (int?)val : null;
        //}

        //public static long? ToNullableLong(this string str)
        //{
        //    long val;
        //    return long.TryParse(str, out val) ? (long?)val : null;
        //}

        //public static Guid? ToNullableGuid(this string str)
        //{
        //    Guid val;
        //    return Guid.TryParse(str, out val) ? (Guid?)val : null;
        //}

        //public static DateTime? ToNullableDateTime(this string str)
        //{
        //    DateTime val;
        //    return DateTime.TryParse(str, out val) ? (DateTime?)val : null;
        //}
    }
}
