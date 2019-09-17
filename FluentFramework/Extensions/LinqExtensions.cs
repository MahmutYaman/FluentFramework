using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FluentFramework.Extensions
{
    public static class LinqExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            using (var provider = new RNGCryptoServiceProvider())
            {
                var n = list.Count;
                while (n > 1)
                {
                    var box = new byte[1];
                    do provider.GetBytes(box);
                    while (!(box[0] < n * (byte.MaxValue / n)));
                    var k = (box[0] % n);
                    n--;
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }
    }
}
