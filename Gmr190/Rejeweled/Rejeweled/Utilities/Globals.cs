using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	/// <summary>
	/// The global variables required by Rejeweled.
	/// </summary>
	public static class GlobalVars
	{
		private static int mGemSizeX = 60;
		private static int mGemSizeY = 60;
		private static int mGridDimensionX = 10;
		private static int mGridDimensionY = 10;
		private static int mMinMatch = 3;
		private static bool mEnforceMoveMustResultInMatch = true;
		private static bool mEnforceMoveMustBeAdjacent = true;
		private static Color mClearColor = Color.White;

		private static Viewport mViewport;

		/// <summary>
		/// Should be called when the viewport is updated changed (fullscreen, resized window, etc).
		/// </summary>
		/// <param name="viewport">The updated viewport.</param>
		public static void UpdateViewport(Viewport viewport)
		{
			mViewport = viewport;
			int maxRes = System.Math.Min(viewport.Width, viewport.Height);
			mGemSizeX = maxRes / mGridDimensionX;
			mGemSizeY = maxRes / mGridDimensionY;
		}

		/// <summary>
		/// Gets the X dimension of an individual gem.
		/// </summary>
		public static int GemSizeX
		{
			get { return mGemSizeX; }
		}

		/// <summary>
		/// Gets the Y dimension of an individual gem.
		/// </summary>
		public static int GemSizeY
		{
			get { return mGemSizeY; }
		}

		/// <summary>
		/// Gets the X size of the play grid.
		/// </summary>
		public static int GridDimensionX
		{
			get { return mGridDimensionX; }
		}

		/// <summary>
		/// Gets the Y size of the play grid.
		/// </summary>
		public static int GridDimensionY
		{
			get { return mGridDimensionY; }
		}

		/// <summary>
		/// The minimum number of gems that must be lined up
		/// in order to trigger the disappear animation and new gems.
		/// </summary>
		public static int MinMatch
		{
			get { return mMinMatch; }
		}

		/// <summary>
		/// Should we enforce the rule that all gem swaps must result
		/// in a line of MinMatch gems?
		/// </summary>
		public static bool EnforceMoveMustResultInMatch
		{
			get { return mEnforceMoveMustResultInMatch; }
		}

		/// <summary>
		/// Should we enforce that all move must be adjacent horizontally
		/// or vertically (no diagonals)?
		/// </summary>
		public static bool EnforceMoveMustBeAdjacent
		{
			get { return mEnforceMoveMustBeAdjacent; }
		}

		/// <summary>
		/// The background color of the game.
		/// </summary>
		public static Color ClearColor
		{
			get { return mClearColor; }
		}

        public static Microsoft.Xna.Framework.Vector2 ScreenSize
        {
            get { return new Microsoft.Xna.Framework.Vector2(mViewport.Width, mViewport.Height); }
        }
	}
}
