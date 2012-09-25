using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	/// <summary>
	/// A utility class for enforcing the rules of Rejeweled.
	/// </summary>
	class RuleChecker
	{
		Gem[,] mGemMatrix;
        List<List<Gem>> mMatchedGems;

		public RuleChecker()
		{
			mGemMatrix = new Gem[GlobalVars.GridDimensionX, GlobalVars.GridDimensionY];
            mMatchedGems = new List<List<Gem>>();
		}

		/// <summary>
		/// Helper method to update the gem matrix to the current game state.
		/// </summary>
		/// <param name="playArea">The Rejeweled PlayArea</param>
		private void BuildGemMatrix(PlayArea playArea)
		{
			List<Gem> gemList = playArea.Gems;

			foreach (Gem gem in gemList)
			{
				mGemMatrix[gem.BoardLocation.X, gem.BoardLocation.Y] = gem;
			}
		}

		/// <summary>
		/// Looks for matches of GlobalVars.MinMatch or more on the PlayArea and begins the disappearing animation for the matching gems.
		/// </summary>
		/// <param name="playArea">The PlayArea to check for matches.</param>
		/// <returns>True if matches were found.</returns>
		public List<List<Gem>> FindMatches (PlayArea playArea)
		{
			BuildGemMatrix(playArea);
            mMatchedGems.Clear();

			for (int x = 0; x < GlobalVars.GridDimensionX; ++x)
			{
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					//because we are checking starting at the top left,
					//we only need to check to the right and down.
					int matchesRight = CheckRight(x, y, 1);
					int matchesBelow = CheckBelow(x, y, 1);

                    List<Gem> matchedGems = GetGemsRight(x, y, matchesRight);
                    if (matchedGems.Count >= GlobalVars.MinMatch)
                        mMatchedGems.Add(matchedGems);
                    matchedGems = GetGemsBelow(x, y, matchesBelow);
                    if (matchedGems.Count >= GlobalVars.MinMatch)
                        mMatchedGems.Add(matchedGems);
    			}
			}

			return mMatchedGems;
		}

        private List<Gem> GetGemsBelow(int x, int y, int matchesBelow)
        {
            List<Gem> matches = new List<Gem>();
            if (matchesBelow >= GlobalVars.MinMatch)
            {
                for (int i = 0; i < matchesBelow; ++i)
                {
                    matches.Add(mGemMatrix[x, y + i]);
                }
            }

            return matches;
        }

        private List<Gem> GetGemsRight(int x, int y, int matchesRight)
        {
            List<Gem> matches = new List<Gem>();
            if (matchesRight >= GlobalVars.MinMatch)
            {
                for (int i = 0; i < matchesRight; ++i)
                {
                    matches.Add(mGemMatrix[x + i, y]);
                }
            }

            return matches;
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
				for (int i = 0; i < matchesBelow; ++i)
				{
					mGemMatrix[x, y + i].Matched();
				}
			}
		}

		private int CheckBelow(int x, int y, int matchCount)
		{
			//bound check
			if (y < GlobalVars.GridDimensionY - 1)
			{
				//are the gems the same type? if so check the next one below
				if (mGemMatrix[x, y].Type == mGemMatrix[x, y + 1].Type)
					return CheckBelow(x, y + 1, matchCount + 1);
			}
			//out of bounds or different type, bail out
			return matchCount;
		}

		private int CheckRight(int x, int y, int matchCount)
		{
			//bounds check
			if (x < GlobalVars.GridDimensionX - 1)
			{
				//are the gems the same type? if so check the next one to the right
				if (mGemMatrix[x, y].Type == mGemMatrix[x + 1, y].Type)
					return CheckRight(x + 1, y, matchCount + 1);
			}
			//otu of bounds or different type, bail out
			return matchCount;
		}

		/// <summary>
		/// Checks if the Gems can be swapped without violating the rules.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool IsValidMove(PlayAreaCoords from, PlayAreaCoords to)
		{
			int moveX = Math.Abs(from.X - to.X);
			int moveY = Math.Abs(from.Y - to.Y);

			//gems can only swap with gems that are adjacent (left and right or up and down, no diagonals).
			if ((moveX == 1 && moveY == 0) || (moveX == 0 && moveY == 1) || !GlobalVars.EnforceMoveMustBeAdjacent)
				return true;

			return false;
		}
	}
}
