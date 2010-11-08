using System;
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
        public const int GridDimensionX = 1;
        public const int GridDimensionY = 1;

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
					int gemID = 3;// mRNG.Next (0, mGemTypeToID.Count);
					mGems.Add (new Gem (mGemTypeToID.First (i => i.Value == gemID).Key, mGemTextures [gemID]));
				}
			}
		}

		public void Update (GameTime gameTime)
		{
			foreach (Gem gem in mGems)
			{
				gem.Update (gameTime);
			}
		}

		public void Draw (SpriteBatch spriteBatch)
		{
			foreach (Gem gem in mGems)
			{
				gem.Draw (spriteBatch);
			}
		}
	}
}
