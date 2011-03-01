#region Using

using System;

#endregion

namespace XELibrary
{
    public static class Utils
    {
        private static readonly Random _random = new Random((int) DateTime.Now.Ticks);

        public static float RandomClamped()
        {
            return (float) (_random.NextDouble() - _random.NextDouble());
        }

        public static int RandInt(int x, int y)
        {
            return _random.Next()%(y - x + 1) + x;
        }

        public static float RandFloat()
        {
            return (float) _random.NextDouble();
        }

        public static float RandInRange(float x, float y)
        {
            return x + (float) _random.NextDouble()*(y - x);
        }
    }
}