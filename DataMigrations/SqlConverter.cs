using System;
using System.Collections.Generic;
using System.Threading;

namespace DataMigrations
{
    public static class SqlConverter
    {
        private static readonly Dictionary<Type, Func<object, string>> CustomConversions;
        private static bool InCustomConversion;

        private static readonly ThreadLocal<string> Result = new ThreadLocal<string>();

        static SqlConverter()
        {
            CustomConversions = new Dictionary<Type, Func<object, string>>();

            SetDefaultConversions();
        }

        public static void ResetCustomConverters()
        {
            CustomConversions.Clear();
            SetDefaultConversions();
        }

        public static void SetCustom<TFor>(Func<TFor, string> func)
        {
            Func<object, string> objectFunc = o => func((TFor) o);

            CustomConversions[typeof(TFor)] = objectFunc;
        }

        public static void SetDefault<TFor>()
        {
            var type = typeof(TFor);

            if (type == typeof(bool))
            {
                SetCustom<bool?>(ToSql);
                return;
            }
            if (type == typeof(bool?))
            {
                SetCustom<bool?>(ToSql);
                return;
            }
            if (type == typeof(string))
            {
                SetCustom<string>(ToSql);
                return;
            }

            SetCustom<TFor>(ToSql);
        }

        public static void SetDefaultConversions()
        {
            SetDefault<string>();
            SetDefault<bool>();
            SetDefault<bool?>();
        }

        public static string ToSql<T>(this T obj)
        {
            if (TryGetCustom(obj)) return Result.Value;

            if (obj == null) return "NULL";

            return obj.ToString();
        }

        public static string ToSql(this bool obj)
        {
            if (TryGetCustom(obj)) return Result.Value;

            return obj ? "1" : "0";
        }

        public static string ToSql(this bool? obj)
        {
            if (TryGetCustom(obj)) return Result.Value;

            if (obj == null) return "NULL";
            return obj.Value ? "1" : "0";
        }

        public static string ToSql(this string obj)
        {
            if (TryGetCustom(obj)) return Result.Value;

            if (obj == null) return "NULL";
            return string.Format($"'{obj}'");
        }

        private static bool TryGetCustom<T>(T obj)
        {
            if (InCustomConversion)
                return false;

            Func<object, string> conversion;

            if (!CustomConversions.TryGetValue(typeof(T), out conversion))
                return false;

            InCustomConversion = true;
            Result.Value = conversion(obj);
            InCustomConversion = false;

            return true;
        }
    }
}