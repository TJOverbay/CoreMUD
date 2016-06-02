using System;

namespace App.Common
{
    public static class StringHelper
    {
        public const string NullTextToken = "[null]";

        public static string ToStringOrDefault(this object value, string defaultText = NullTextToken)
        {
            if (ReferenceEquals(null, value))
                return defaultText;
            else
                return value.ToString();
        }

        public static string ToStringOrDefault(this string value, string defaultText = NullTextToken)
        {
            if (string.IsNullOrWhiteSpace(value))
                return defaultText;
            else
                return value;
        }

        public static string ToStringOrDefault<T>(this T? value, string defaultText = NullTextToken, string format = null, IFormatProvider formatProvider = null)
            where T : struct
        {
            if (value.HasValue)
            {
                if (!string.IsNullOrWhiteSpace(format))
                {
                    var f = value.Value as IFormattable;
                    if (f != null)
                        return f.ToString(format, formatProvider ?? System.Globalization.CultureInfo.CurrentCulture);
                }

                return value.Value.ToString();
            }
            else
            {
                return defaultText;
            }
        }

    }
}
