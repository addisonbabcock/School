using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	/// <summary>
	/// Represents a collection of gems.
	/// </summary>
	class PlayArea
	{
        private List<Gem> mGems;
		private Dictionary<GemType, int> mGemTypeToID;
		private List<List<Texture2D>> mGemTextures;
		private Random mRNG;

		private RuleChecker mRules;
		bool mCheckRulesNextUpdate;

		Gem mSwapGem1;
		Gem mSwapGem2;

		List<int> mReplaceGemIndexes;

		public PlayArea(List <List<Texture2D>> gemTextures)
		{
			mGemTextures = gemTextures;
			mRNG = new Random ();
			mGems = new List<Gem>(GlobalVars.GridDimensionX * GlobalVars.GridDimensionY);
			mGemTypeToID = new Dictionary<GemType, int> ();

			mGemTypeToID [GemType.Yellow] = 0;
			mGemTypeToID [GemType.White] = 1;
			mGemTypeToID [GemType.Blue] = 2;
			mGemTypeToID [GemType.Red] = 3;
			mGemTypeToID [GemType.Purple] = 4;
			mGemTypeToID [GemType.Orange] = 5;
			mGemTypeToID [GemType.Green] = 6;

			for (int x = 0; x < GlobalVars.GridDimensionX; ++x)
			{
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					Gem newGem = GetNewGem ();
					newGem.MoveTo(new PlayAreaCoords(x, y));
					newGem.SetStartingLocation(new PlayAreaCoords(x, y - GlobalVars.GridDimensionY));
					mGems.Add(newGem);
				}
			}

			mRules = new RuleChecker();
			mCheckRulesNextUpdate = false;
			mReplaceGemIndexes = new List<int>();

			mSwapGem1 = null;
			mSwapGem2 = null;
		}

		private Gem GetNewGem()
		{
			int gemID = mRNG.Next(0, mGemTypeToID.Count);
			Gem gem = new Gem(mGemTypeToID.First(i => i.Value == gemID).Key, mGemTextures[gemID], this);
			return gem;
		}

		public void GemMoveAnimationCompleted()
		{
			mCheckRulesNextUpdate = true;
		}

		public void GemDisappearAnimationComplete(Gem gem)
		{
			//hmmm this will need to be refactored so we can count the number
			//of missing gems in a column.
			mReplaceGemIndexes.Add(mGems.FindIndex(i => i == gem));
		}

		public List<Gem> Gems
		{
			get { return mGems; }
		}

		public PlayAreaCoords Size
		{
			get { return new PlayAreaCoords(GlobalVars.GridDimensionX, GlobalVars.GridDimensionY); }
		}

		public void Update(GameTime gameTime)
		{
			if (mReplaceGemIndexes.Count > 0)
			{
				ReplaceDisappearingGems();
			}

			if (mCheckRulesNextUpdate)
			{
				if (!mRules.FindMatches(this))
				{
					//these can be null during startup
					if (mSwapGem1 != null && mSwapGem2 != null && GlobalVars.EnforceMoveMustResultInMatch)
						mSwapGem1.Swap(mSwapGem2); //if the move doesnt result in a match, move the gems back.
				}
				mSwapGem1 = null;
				mSwapGem2 = null;
				mCheckRulesNextUpdate = false;
			}

			for (int i = 0; i < mGems.Count; ++i)
			{
				mGems[i].Update (gameTime);
			}
		}

		private void ReplaceDisappearingGems()
		{
			List<int> missingGemsInColumn = CountMissingGemsByColumn();

			foreach (int index in mReplaceGemIndexes)
			{
				PlayAreaCoords gemLoc = new PlayAreaCoords(mGems [index].BoardLocation.X, 0);
				Gem newGem = GetNewGem();
				newGem.MoveTo(gemLoc);
				newGem.SetStartingLocation(new PlayAreaCoords(gemLoc.X, -1));

				for (int y = 0; y < gemLoc.Y; ++y)
				{
					PlayAreaCoords findCoords = new PlayAreaCoords(gemLoc.X, y);
					Gem moveGem = mGems.Find(i => i.BoardLocation == findCoords);
					if (moveGem != null)
					{
						PlayAreaCoords moveCoords = new PlayAreaCoords(findCoords.X, findCoords.Y + 1);
						System.Diagnostics.Debug.WriteLine("Dropping gem from " + findCoords.ToString() + " to " + moveCoords.ToString());
						moveGem.MoveTo(moveCoords);
					}
				}
				mGems[index] = newGem;
			}
			mReplaceGemIndexes.Clear();
		}

		private List<int> CountMissingGemsByColumn()
		{
			List <int> missingGemsByColumn = new List<int>(GlobalVars.GridDimensionX);

			for (int column = 0; column < GlobalVars.GridDimensionX; ++column)
			{
				missingGemsByColumn.Add(mGems.Count(i => i.BoardLocation.X == column && i.IsDisappeared));
			}

			return missingGemsByColumn;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Gem gem in mGems)
			{
				gem.Draw (spriteBatch);
			}
		}

		public void MouseClicked (MouseEvent mouseEvent)
		{
			Gem clickedGem = mGems.Find (i => i.Contains (mouseEvent.MouseLocation));
			Gem selectedGem = mGems.Find (i => i.IsSelected);

			if (clickedGem == null)
			{
				Debug.WriteLine ("Did not find a gem at the mouse click.");
				return;
			}

			if (selectedGem != null)
			{
				if (selectedGem != clickedGem)
				{
					clickedGem.Swap(selectedGem);
					mSwapGem1 = selectedGem;
					mSwapGem2 = clickedGem;
				}
				else
				{
					selectedGem.IsSelected = false;
				}
			}
			else
			{
				clickedGem.IsSelected = true;
			}
		}

		public void MouseDragged(MouseEvent mouseEvent)
		{
			Gem fromGem = mGems.Find(i => i.Contains(mouseEvent.DragStart));
			Gem toGem = mGems.Find(i => i.Contains(mouseEvent.DragEnd));

			if (fromGem != null && toGem != null)
			{
				fromGem.Swap(toGem);
				mSwapGem1 = toGem;
				mSwapGem2 = fromGem;
			}
			else
			{
				Debug.WriteLine("Could not find two gems to drag...");
			}
		}
	}
}
