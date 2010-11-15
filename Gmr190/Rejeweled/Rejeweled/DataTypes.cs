using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	enum GemType
	{
		Blue,
		Green,
		Red,
		Purple,
		Yellow,
		White,
		Orange,
	}

	public static class GlobalVars
	{
		private static int mGemSizeX = 60;
		private static int mGemSizeY = 60;
		private static int mGridDimensionX = 10;
		private static int mGridDimensionY = 10;

		private static Viewport mViewport;

		public static void UpdateViewport(Viewport viewport)
		{
			mViewport = viewport;
			int maxRes = System.Math.Min(viewport.Width, viewport.Height);
			mGemSizeX = maxRes / mGridDimensionX;
			mGemSizeY = maxRes / mGridDimensionY;
		}

		public static int GemSizeX
		{
			get { return mGemSizeX; }
		}

		public static int GemSizeY
		{
			get { return mGemSizeY; }
		}

		public static int GridDimensionX
		{
			get { return mGridDimensionX; }
		}

		public static int GridDimensionY
		{
			get { return mGridDimensionY; }
		}
	}
}
