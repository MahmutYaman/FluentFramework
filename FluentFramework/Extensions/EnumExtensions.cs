using System;

namespace FluentFramework.Extensions
{
    public static class EnumExtensions
    {
        public static T Parse<T>(this Enum @enum, string value)
            => (T)Enum.Parse(@enum.GetType(), value, true);

        public static T ToEnum<T>(this string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Return type must be an enumerated type", nameof(T));
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}