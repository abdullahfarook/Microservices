using System;
using System.Runtime.CompilerServices;

namespace Microservices.Core
{
    public static partial class CoreExtensions
    {
        public static string GetDefaultValue<T>(T item) where T : class
        {
            if (item == null)
                return string.Empty;
            var property = typeof(T).GetProperties()[0];
            var value =(string)  property.GetValue(item);
            return !string.IsNullOrEmpty(value) ? value : property.Name;
        }

        public static string GetDefaultValue(this string value, [CallerMemberName] string methodName = null) => 
            string.IsNullOrEmpty(value) ? throw new ArgumentNullException($"A value in {methodName} is not set") : value;
    }
}
