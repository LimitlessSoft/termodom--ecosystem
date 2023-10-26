using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public static class Random
    {
        private static System.Random _rnd = new System.Random();

        public static int Next(int maxValue)
        {
            return _rnd.Next(maxValue);
        }
        public static int Next(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }

    }
}
