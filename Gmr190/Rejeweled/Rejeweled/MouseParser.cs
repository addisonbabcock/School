using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Rejeweled
{
	//parses mouse input into a format that's easier to use.
	class MouseParser
	{
		private MouseState mPreviousState;
		private MouseState mDragOrigin;

		public void Update (MouseState currentMouseState)
		{
			if (mPreviousState != null)
			{
				//look for clicks
				if (mPreviousState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
				{
					//deal with a left mouse click here
				}

				//was clicked and is now released
				if (mPreviousState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
				{
					mDragOrigin = null;
				}

				//look for dragging
				if (mPreviousState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed &&
					(mPreviousState.X != currentMouseState.X || mPreviousState.Y != currentMouseState.Y))
				{
					if (mDragOrigin == null)
						mDragOrigin = mPreviousState;
				}
			}

			//MUST BE LAST!
			mPreviousState = currentMouseState;
		}
	}
}
