using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentFramework.Extensions
{
    public static class LinqExtensions
    {
        public static T Random<T>(this IEnumerable<T> source)
        {
            return source.ElementAt(new Random().Next(0, source.Count()));
        }

        public static T RandomOrDefault<T>(this IEnumerable<T> source)
        {
            var count = source.Count();
            return count == 0 ? default : source.ElementAt(new Random().Next(0, count));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            var random = new Random();
            return source.OrderBy(x => random.Next());
        }
    }
}
