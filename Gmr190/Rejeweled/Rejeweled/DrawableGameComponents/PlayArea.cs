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
	class PlayArea : Microsoft.Xna.Framework.DrawableGameComponent
	{
        private List<Gem> mGems;
		private Dictionary<GemType, int> mGemTypeToID;
		private List<List<Texture2D>> mGemTextures;
		private List<List<Texture2D>> mPowerupTextures;
		private Random mRNG;
		private SpriteBatch mSpriteBatch;

		private RuleChecker mRules;
		bool mCheckRulesNextUpdate;

		Gem mSwapGem1;
		Gem mSwapGem2;

		List<int> mReplaceGemIndexes;

		/// <summary>
		/// Constructs a PlayArea complete with a set of Gems.
		/// </summary>
		/// <param name="gemTextures">The textures that the Gems on this board will use.</param>
		public PlayArea(Game game, Random rng)
			: base (game)
		{
			mGemTextures = new List<List<Texture2D>>();
			mPowerupTextures = new List<List<Texture2D>>();
			mRNG = rng;
			mGems = new List<Gem>(GlobalVars.GridDimensionX * GlobalVars.GridDimensionY);

			mGemTypeToID = new Dictionary<GemType, int> ();
			mGemTypeToID [GemType.Yellow] = 0;
			mGemTypeToID [GemType.White] = 1;
			mGemTypeToID [GemType.Blue] = 2;
			mGemTypeToID [GemType.Red] = 3;
			mGemTypeToID [GemType.Purple] = 4;
			mGemTypeToID [GemType.Orange] = 5;
			mGemTypeToID [GemType.Green] = 6;

			mRules = new RuleChecker();
			mCheckRulesNextUpdate = false;
			mReplaceGemIndexes = new List<int>();

			mSwapGem1 = null;
			mSwapGem2 = null;
		}

		public void CreateNewBoard()
		{
			for (int x = 0; x < GlobalVars.GridDimensionX; ++x)
			{
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					Gem newGem = GetNewGem();
					newGem.MoveTo(new PlayAreaCoords(x, y));
					newGem.SetStartingLocation(new PlayAreaCoords(x, y - GlobalVars.GridDimensionY));
					mGems.Add(newGem);
				}
			}
		}

		protected override void LoadContent ()
		{
			mSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
			base.LoadContent();
			 for (int i = 0; i < 7; ++i)
			 {
				 mGemTextures.Add(new List<Texture2D>());
				 mPowerupTextures.Add(new List<Texture2D>());

				 for (int j = 0; j < 20; ++j)
				 {
					 string gemName =
						 "Gems\\Alpha\\Gem" +
						 i +
						 "\\gem" +
						 i +
						 "_" +
						 (j + 1).ToString("0#");
					 string powerUpName =
						 "Gems\\Alpha_Powerup\\Gem" +
						 i +
						 "\\gem" +
						 i +
						 "_" +
						 (j + 1).ToString("0#");

					 mGemTextures[i].Add(Game.Content.Load<Texture2D>(gemName));
					 mPowerupTextures[i].Add(Game.Content.Load<Texture2D>(powerUpName));
				 }
			 }
		}

		/// <summary>
		/// Constructs a new Gem with a random color.
		/// </summary>
		/// <returns></returns>
		private Gem GetNewGem()
		{
			int gemID = mRNG.Next(0, mGemTypeToID.Count);
			Gem gem = new Gem(mGemTypeToID.First(i => i.Value == gemID).Key, mGemTextures[gemID], this);
			return gem;
		}

		/// <summary>
		/// Callback from Gem. When this is called, we will check the rules next update.
		/// </summary>
		public void GemMoveAnimationCompleted()
		{
			mCheckRulesNextUpdate = true;
		}

		/// <summary>
		/// Callback from Gem. Accumulates all the vanished Gems to be removed next update call.
		/// </summary>
		/// <param name="gem"></param>
		public void GemDisappearAnimationComplete(Gem gem)
		{
			//hmmm this will need to be refactored so we can count the number
			//of missing gems in a column.
			mReplaceGemIndexes.Add(mGems.FindIndex(i => i == gem));
		}

		/// <summary>
		/// Gets all the Gems on this PlayArea.
		/// </summary>
		public List<Gem> Gems
		{
			get { return mGems; }
		}

		/// <summary>
		/// Gets the size of this PlayArea.
		/// </summary>
		public PlayAreaCoords Size
		{
			get { return new PlayAreaCoords(GlobalVars.GridDimensionX, GlobalVars.GridDimensionY); }
		}

		/// <summary>
		/// Updates the PlayArea and all its components.
		/// </summary>
		/// <param name="gameTime">How much time has passed since the last call.</param>
		public override void Update(GameTime gameTime)
		{
			if (mReplaceGemIndexes.Count > 0)
			{
				ReplaceDisappearingGems();
			}

			if (mCheckRulesNextUpdate)
			{
				if (!mRules.FindMatches(this))
				{
					//these can be null during startup because Gems can be lined up before the player has 
					//had a chance to do any swapping.
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

		/// <summary>
		/// Replaces all the vanished Gems with new ones and slides the Gems above them downwards.
		/// </summary>
		private void ReplaceDisappearingGems()
		{
			List<List<Gem>> missingGemsInColumn = GetMissingGemsByColumn();
			int replaceGemIndex = 0;

			//for each column...
			//foreach (List <Gem> missingGems in missingGemsInColumn)
			for (int column = 0; column < GlobalVars.GridDimensionX; ++column)
			{
				//for each gem in the column, starting at the bottom
				for (int y = 0; y < GlobalVars.GridDimensionY; ++y)
				{
					//count the gems above this gem
					PlayAreaCoords currentGemCoords = new PlayAreaCoords (column, y);
					Gem currentGem = mGems.Find(i => i.BoardLocation == currentGemCoords);
					int missingGemsBelow = missingGemsInColumn[column].Count(i => i.BoardLocation.X == column && i.BoardLocation.Y > y);

					if (currentGem != null)
						currentGem.MoveTo(new PlayAreaCoords(column, y + missingGemsBelow));
					else
						//unfortunately this actually does seem to happen... wtf
						System.Diagnostics.Debug.WriteLine("This really shouldn't happen... could not find gem at location " + currentGemCoords.ToString());
				}

				for (int i = 0; i < missingGemsInColumn[column].Count && replaceGemIndex < mReplaceGemIndexes.Count; ++i)
				{
					PlayAreaCoords newGemMoveToLoc = new PlayAreaCoords(column, i);
					PlayAreaCoords newGemMoveFromLoc = new PlayAreaCoords(column, i - missingGemsInColumn[column].Count);
					Gem newGem = GetNewGem();
					newGem.MoveTo(newGemMoveToLoc);
					newGem.SetStartingLocation(newGemMoveFromLoc);
					mGems[mReplaceGemIndexes[replaceGemIndex]] = newGem;
					++replaceGemIndex;
				}
			}

			mReplaceGemIndexes.Clear();
		}

		/// <summary>
		/// Constructs a list of the Gems which have vanished in each column.
		/// </summary>
		/// <returns></returns>
		private List<List<Gem>> GetMissingGemsByColumn()
		{
			List<List<Gem>> missingGems = new List<List<Gem>>(10);

			for (int column = 0; column < GlobalVars.GridDimensionX; ++column)
			{
				missingGems.Add(mGems.FindAll(i => i.BoardLocation.X == column && i.IsDisappeared));
			}

			return missingGems;
		}

		/// <summary>
		/// Draws the PlayArea.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch which will be used for rendering.</param>
		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			mSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			foreach (Gem gem in mGems)
			{
				gem.Draw(mSpriteBatch);
			}
			mSpriteBatch.End();
		}

		/// <summary>
		/// Finds the clicked Gem and swaps it with a selected Gem.
		/// </summary>
		/// <param name="mouseEvent"></param>
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
				if (mRules.IsValidMove (clickedGem.BoardLocation, selectedGem.BoardLocation))
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

		/// <summary>
		/// Finds the Gems where the mouse started and ended then swaps them.
		/// </summary>
		/// <param name="mouseEvent"></param>
		public void MouseDragged(MouseEvent mouseEvent)
		{
			Gem fromGem = mGems.Find(i => i.Contains(mouseEvent.DragStart));
			Gem toGem = mGems.Find(i => i.Contains(mouseEvent.DragEnd));

			if (fromGem != null && toGem != null && mRules.IsValidMove (fromGem.BoardLocation, toGem.BoardLocation))
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
