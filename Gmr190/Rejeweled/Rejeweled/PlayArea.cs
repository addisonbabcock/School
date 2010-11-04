using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		PlayAreaCoords()
		{
			X = 0;
			Y = 0;
		}

		PlayAreaCoords(int _x, int _y)
		{
			X = _x;
			Y = _y;
		}
	}

	class PlayArea
	{
		private GemGrid mMap;

		public PlayArea()
		{
			mMap = new GemGrid();

		}
	}
}
