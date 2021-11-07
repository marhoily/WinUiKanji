using System;
using System.Collections.Generic;

namespace WinUiKanji
{
    public static class Helpers
    {
        /// <summary>
        /// Knuth shuffle
        /// </summary>        
        public static void Shuffle<T>(this Random rnd, IList<T> array)
        {
            var n = array.Count;
            while (n > 1)
            {
                n--;
                var i = rnd.Next(n + 1);
                (array[i], array[n]) = (array[n], array[i]);
            }
        }
    }
}