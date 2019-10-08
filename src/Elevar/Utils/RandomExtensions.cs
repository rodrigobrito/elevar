using System;

namespace Elevar.Utils
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double minValue, double maxValue)
        {
            var next = random.NextDouble();
            return minValue + (next * (maxValue - minValue));
        }
    }
}
