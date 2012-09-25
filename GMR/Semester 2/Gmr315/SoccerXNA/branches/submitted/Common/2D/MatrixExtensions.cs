#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Common._2D
{
    public static class MatrixExtensions
    {
        public static Matrix MakeRotation(this Matrix matrix, Vector2 vector1, Vector2 vector2)
        {
            matrix = Matrix.Identity;
            matrix.M11 = vector1.X;
            matrix.M12 = vector1.Y;
            matrix.M21 = vector2.X;
            matrix.M22 = vector2.Y;
            return matrix;
        }
    }
}