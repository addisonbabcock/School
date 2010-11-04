using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	class PlayArea
	{
        public const int GridDimensionX = 10;
        public const int GridDimensionY = 10;

        private List<Gem> mGems;

		public PlayArea()
		{
            mGems = new List<Gem>(GridDimensionX * GridDimensionY);
		}
	}
}
