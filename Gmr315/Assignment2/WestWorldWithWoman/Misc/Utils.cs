#region Using

using System;

#endregion

namespace WestWorldWithWoman.Misc
{
    public static class Utils
    {
        private static readonly Random random = new Random((int) DateTime.Now.Ticks);

        public static double RandFloat()
        {
            return random.NextDouble();
        }

        public static int RandInt(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}