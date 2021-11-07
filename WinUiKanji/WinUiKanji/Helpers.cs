using System;
using System.Collections.Generic;
using System.Linq;

namespace WinUiKanji
{
    public static class Helpers
    {
        public static List<T> Shuffle<T>(this IEnumerable<T> array, Random rnd)
        {
            var result = array.ToList();
            var n = result.Count;
            while (n > 1)
            {
                n--;
                var i = rnd.Next(n + 1);
                (result[i], result[n]) = (result[n], result[i]);
            }
            return result;
        }
    }
}