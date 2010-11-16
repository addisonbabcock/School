using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	class RuleChecker
	{
		Gem[,] mGemMatrix;

		public RuleChecker()
		{
			mGemMatrix = new Gem[GlobalVars.GridDimensionX, GlobalVars.GridDimensionY];
		}

		private void BuildGemMatrix(PlayArea playArea)
		{
			List<Gem> gemList = playArea.Gems;

			foreach (Gem gem in gemList)
			{
				mGemMatrix[gem.BoardLocation.X, gem.BoardLocation.Y] = gem;
			}
		}

		public void FindMatches (PlayArea playArea)
		{
			BuildGemMatrix(playArea);

			for (int x = 0; x < GlobalVars.GridDimensionX; ++x)
			{
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					//because we are checking starting at the top left,
					//we only need to check to the right and down.
					int matchesRight = CheckRight(x, y, 1);
					int matchesBelow = CheckBelow(x, y, 1);

					MarkGemsAsDisappearingBelow(x, y, matchesBelow);
					MarkGemsAsDisappearingRight(x, y, matchesRight);
				}
			}
		}

		private void MarkGemsAsDisappearingRight(int x, int y, int matchesRight)
		{
			if (matchesRight >= GlobalVars.MinMatch)
			{
				for (int i = 0; i < matchesRight; ++i)
				{
					mGemMatrix[x + i, y].Matched();
				}
			}
		}

		private void MarkGemsAsDisappearingBelow(int x, int y, int matchesBelow)
		{
			if (matchesBelow >= GlobalVars.MinMatch)
			{
				System.Diagnostics.Debug.WriteLine("Matching " + matchesBelow + " going down from " + x + ", " + y);
				for (int i = 0; i < matchesBelow; ++i)
				{
					mGemMatrix[x, y + i].Matched();
				}
			}
		}

		private int CheckBelow(int x, int y, int matchCount)
		{
			if (y < GlobalVars.GridDimensionY - 1)
			{
				if (mGemMatrix[x, y].Type == mGemMatrix[x, y + 1].Type)
					return CheckBelow(x, y + 1, matchCount + 1);
			}
			return matchCount;
		}

		private int CheckRight(int x, int y, int matchCount)
		{
			if (x < GlobalVars.GridDimensionX - 1)
			{
				if (mGemMatrix[x, y].Type == mGemMatrix[x + 1, y].Type)
					return CheckRight(x + 1, y, matchCount + 1);
			}
			return matchCount;
		}
	}
}
