namespace Rejeweled
{
    class PlayAreaCoords
    {
        private int mX;
        private int mY;

        public int X
        {
            get { return mX; }
            set { mX = value; }
        }

        public int Y
        {
            get { return mY; }
            set { mY = value; }
        }

        public PlayAreaCoords()
        {
            X = 0;
            Y = 0;
        }

        public PlayAreaCoords(int _x, int _y)
        {
            X = _x;
            Y = _y;
        }

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
    }
}
