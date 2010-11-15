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
	class PlayArea
	{
        public const int GridDimensionX = 10;
        public const int GridDimensionY = 10;

        private List<Gem> mGems;
		private Dictionary<GemType, int> mGemTypeToID;
		private List<List<Texture2D>> mGemTextures;
		private Random mRNG;

		public PlayArea(List <List<Texture2D>> gemTextures)
		{
			mGemTextures = gemTextures;
			mRNG = new Random ();
            mGems = new List<Gem>(GridDimensionX * GridDimensionY);
			mGemTypeToID = new Dictionary<GemType, int> ();

			mGemTypeToID [GemType.Yellow] = 0;
			mGemTypeToID [GemType.White] = 1;
			mGemTypeToID [GemType.Blue] = 2;
			mGemTypeToID [GemType.Red] = 3;
			mGemTypeToID [GemType.Purple] = 4;
			mGemTypeToID [GemType.Orange] = 5;
			mGemTypeToID [GemType.Green] = 6;

			for (int x = 0; x < GridDimensionX; ++x)
			{
				for (int y = 0; y < GridDimensionY; ++y)
				{
					int gemID = mRNG.Next (0, mGemTypeToID.Count);
					Gem gem = new Gem (mGemTypeToID.First (i => i.Value == gemID).Key, mGemTextures [gemID]);
					gem.MoveTo (new PlayAreaCoords (x, y));
					mGems.Add (gem);
				}
			}
		}

		public PlayAreaCoords Size
		{
			get { return new PlayAreaCoords (GridDimensionX, GridDimensionY); }
		}

		public void Update(GameTime gameTime)
		{
			foreach (Gem gem in mGems)
			{
				gem.Update (gameTime);
			}
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
			}
			else
			{
				Debug.WriteLine("Could not find two gems to drag...");
			}
		}
	}
}
