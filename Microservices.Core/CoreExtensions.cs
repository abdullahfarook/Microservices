using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using AutoMapper;

namespace Microservices.Core
{
    public static partial class CoreExtensions
    {

        private static readonly TextInfo TextInfo;
        static CoreExtensions()
        {
            TextInfo = new CultureInfo("en-US", false).TextInfo;
        }
        public static string ToDashCase(this string arg)
        {
            return arg.Trim().ToLower().Replace(" ", "-");
        }
        public static string ToTitleCase(this string arg)
        {
            return TextInfo.ToTitleCase(arg.Trim().ToLower());
        }
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return Char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
        public static void AddRange<T>(this ICollection<T> destination,IEnumerable<T> source)
        {
            if (destination is List<T> list)
            {
                list.AddRange(source);
            }
            else
            {
                foreach (T item in source)
                {
                    destination.Add(item);
                }
            }
        }

        public static IMappingExpression<TSource, TDestination> IgnoreNullAndDefault<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
            {
                if (srcMember == null) return false;
                var type = srcMember.GetType();
                if (type == typeof(bool)) return true;
                return !srcMember.Equals(type.GetDefaultValue());
            }));

            return expression;
        }

        public static bool IsSimple(this Type type)
        {
            return type.IsPrimitive || type.IsValueType || type == typeof(string) || type == typeof(decimal) ||
                   type.IsEnum;
        }
        public static IMappingExpression<TSource, TDestination> IgnoreNull<TSource, TDestination>
            (this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            return expression;
        }
        private static void IgnoreUnmappedProperties(TypeMap map, IMappingExpression expr)
        {
            foreach (string propName in map.GetUnmappedPropertyNames())
            {
                if (map.SourceType.GetProperty(propName) != null)
                {
                    expr.ForSourceMember(propName, opt => opt.DoNotValidate());
                }
                if (map.DestinationType.GetProperty(propName) != null)
                {
                    expr.ForMember(propName, opt => opt.Ignore());
                }
            }
        }

        public static void IgnoreUnmapped(this IProfileExpression profile)
        {
            profile.ForAllMaps(IgnoreUnmappedProperties);
        }

        public static void IgnoreUnmapped(this IProfileExpression profile, Func<TypeMap, bool> filter)
        {
            profile.ForAllMaps((map, expr) =>
            {
                if (filter(map))
                {
                    IgnoreUnmappedProperties(map, expr);
                }
            });
        }

        public static void IgnoreUnmapped(this IProfileExpression profile, Type src, Type dest)
        {
            profile.IgnoreUnmapped((TypeMap map) => map.SourceType == src && map.DestinationType == dest);
        }

        public static void IgnoreUnmapped<TSrc, TDest>(this IProfileExpression profile)
        {
            profile.IgnoreUnmapped(typeof(TSrc), typeof(TDest));
        }
        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        public static Exception ToUniqueKeyException(this Exception ex, string resourceName)
        {
            //2601 is error number of unique index violation
            if (ex.GetBaseException() is SqlException sqlException && sqlException.Number == 2601)
            {
                return new ApiException(HttpStatusCode.Conflict, $"{resourceName} must be unique");
            }
            return ex;
        }
        //public static TA As<TA>(this Exception ex) where TA : Exception
        //{
        //    var type = typeof(TA);
        //    var instance = Activator.CreateInstance(type);

        //    PropertyInfo[] properties = type.GetProperties();
        //    foreach (var property in properties)
        //    {
        //        property.SetValue(instance, property.GetValue(ex, null), null);
        //    }

        //    return (TA)instance;
        //}

        public static string ReadToEnd(this Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
