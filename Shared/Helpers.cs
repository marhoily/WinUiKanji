using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
    public static class Helpers
    {
        private static readonly Random Rnd = new();

        public static List<T> Shuffle<T>(this IEnumerable<T> array)
        {
            var result = array.ToList();
            var n = result.Count;
            while (n > 1)
            {
                n--;
                var i = Rnd.Next(n + 1);
                (result[i], result[n]) = (result[n], result[i]);
            }
            return result;
        }
    }
}