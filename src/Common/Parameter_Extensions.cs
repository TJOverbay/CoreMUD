using System;
using System.Reflection;

namespace App.Common
{
    public static class Parameter_Extensions
    {
        public static void ThrowIfNull<T>(this T argValue, string argName)
            where T : class
        {
            if (ReferenceEquals(null, argValue))
            {
                throw new ArgumentNullException("Value cannot be null",
                    string.IsNullOrEmpty(argName)
                        ? "[unnamed argument]"
                        : argName);
            }
        }

        public static void ThrowIfNullOrWhitespace(this string argValue, string argName)
        {
            ThrowIfNullOrWhitespace(argValue, argName, "Value cannot be null");
        }

        public static void ThrowIfNullOrWhitespace(this string argValue, string argName, string message)
        {
            if (string.IsNullOrWhiteSpace(argValue))
            {
                throw new ArgumentNullException(message,
                    string.IsNullOrEmpty(argName)
                        ? "[unnamed argument]"
                        : argName);
            }
        }

        public static void ThrowIfNotAssignableTo<T>(this Type argType, string argName, string messageFormat = null, params object[] messageArgs)
        {
            argType.ThrowIfNull(argName);
            var targetType = typeof(T);

            if (!argType.GetTypeInfo().IsAssignableFrom(targetType))
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "Type '{0}' is not assignable to type '{1}'",
                            argType.FullName, targetType.FullName));

                throw new ArgumentOutOfRangeException(argName, targetType, msg);
            }
        }

        public static void ThrowIfNotInRangeInclusive<T>(this T argValue, T minValue, T maxValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : IComparable<T>
        {
            if ((argValue.CompareTo(minValue) < 0) || (argValue.CompareTo(maxValue) > 0))
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "{0} is outside the range of {1} - {2}, inclusive",
                            argValue, minValue, maxValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        public static void ThrowIfNotInRangeExclusive<T>(this T argValue, T minValue, T maxValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : struct, IComparable<T>
        {
            if ((argValue.CompareTo(minValue) <= 0) || (argValue.CompareTo(maxValue) >= 0))
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "{0} is outside the range of {1} - {2}, exclusive",
                            argValue, minValue, maxValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        public static void ThrowIfGreaterThan<T>(this T argValue, T maxValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : struct, IComparable<T>
        {
            if (argValue.CompareTo(maxValue) > 0)
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "{0} cannot be greater than {1}",
                            argValue, maxValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        public static void ThrowIfGreaterThanOrEqualTo<T>(this T argValue, T maxValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : struct, IComparable<T>
        {
            if (argValue.CompareTo(maxValue) >= 0)
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "{0} cannot be greater than or equal to {1}",
                            argValue, maxValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        public static void ThrowIfLessThan<T>(this T argValue, T minValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : struct, IComparable<T>
        {
            if (argValue.CompareTo(minValue) < 0)
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                            "{0} cannot be less than {1}",
                            argValue, minValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        public static void ThrowIfLessThanOrEqualTo<T>(this T argValue, T minValue, string argName, string messageFormat = null, params object[] messageArgs)
            where T : struct, IComparable<T>
        {
            if (argValue.CompareTo(minValue) <= 0)
            {
                string msg = messageFormat
                    .ApplyParams(messageArgs)
                    .ToStringOrDefault(
                        string.Format(
                        "{0} cannot be less than or equal to {1}",
                        argValue, minValue));

                throw new ArgumentOutOfRangeException(argName, msg);
            }
        }

        private static string ApplyParams(this string messageFormat, params object[] messageArgs)
        {
            if (string.IsNullOrWhiteSpace(messageFormat))
            {
                return null;
            }
            else
            {
                return string.Format(messageFormat, messageArgs);
            }
        }
    }
}
