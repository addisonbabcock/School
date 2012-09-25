using Microsoft.Xna.Framework;

namespace Rejeweled
{
    class PlayAreaCoords
    {
        private int mX;
        private int mY;

		/// <summary>
		/// X distance from the top left.
		/// </summary>
        public int X
        {
            get { return mX; }
            set { mX = value; }
        }

		/// <summary>
		/// Y distance from the top left.
		/// </summary>
        public int Y
        {
            get { return mY; }
            set { mY = value; }
        }

		/// <summary>
		/// Constructs a default PlayAreaCoords at {-1, -1}.
		/// </summary>
        public PlayAreaCoords()
        {
            X = -1;
            Y = -1;
        }

		/// <summary>
		/// Constructs a PlayAreaCoords at (_x, _y).
		/// </summary>
		/// <param name="_x">X distance from the top left.</param>
		/// <param name="_y">Y distance from the top left.</param>
        public PlayAreaCoords(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

		/// <summary>
		/// Compares 2 PlayAreaCoords. X and Y values must be the same.
		/// </summary>
		/// <param name="lhs"></param>
		/// <param name="rhs"></param>
		/// <returns>True if the arguments are the same.</returns>
        public static bool operator == (PlayAreaCoords lhs, PlayAreaCoords rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(PlayAreaCoords lhs, PlayAreaCoords rhs)
        {
            return !(lhs == rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

		/// <summary>
		/// Converts a PlayAreaCoords to the on screen location as a Vector2.
		/// </summary>
		/// <param name="coords">The coordinates to convert.</param>
		/// <returns>The on screen coordinates that are equivalent to the coords argument.</returns>
		public static implicit operator Vector2 (PlayAreaCoords coords)
		{
			return new Vector2 (
				coords.X * GlobalVars.GemSizeX + 0,
				coords.Y * GlobalVars.GemSizeY + 0);
		}

		public override string ToString()
		{
			return X.ToString() + ", " + Y.ToString();
		}

        public int Compare(PlayAreaCoords playAreaCoords)
        {
            if (X != playAreaCoords.X)
                return X - playAreaCoords.X;
            return Y - playAreaCoords.Y;
        }
    }
}
