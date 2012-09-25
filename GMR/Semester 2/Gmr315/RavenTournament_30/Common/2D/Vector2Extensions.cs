#region Using

using Common.Misc;
using Microsoft.Xna.Framework;

#endregion

namespace Common._2D
{
    public static class Vector2Extensions
    {
        public static Vector2 Perp(this Vector2 vector)
        {
            //Utils.TestVector(vector);
            return new Vector2(-vector.Y, vector.X);
        }

        public static int Sign(this Vector2 vector, Vector2 vector2)
        {
//            Utils.TestVector(vector, vector2);
            if (vector.Y*vector2.X > vector.X*vector2.Y)
            {
                return -1;
            }
            return 1;
        }

        //----------------------------- Truncate ---------------------------------
        //
        //  truncates a vector so that its length does not exceed max
        //------------------------------------------------------------------------
        public static Vector2 Truncate(this Vector2 vector, float max)
        {
//            Utils.TestVector(vector);
//            Utils.TestFloat(max);
            if (vector.Length() > max)
            {
//                Utils.TestVector(vector, true);
                Vector2 normalizedVector = Vector2.Normalize(vector);

                float newX = normalizedVector.X*max;
                float newY = normalizedVector.Y*max;
                return new Vector2(newX, newY);
            }
            return vector;
        }
    }
}