using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	/// <summary>
	/// The color of a gem.
	/// </summary>
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

	/// <summary>
	/// This is where most of the action is. Represents a individual gem on the screen.
	/// </summary>
	class Gem
	{
		private GemType mType;
		private bool mIsSelected;
		private bool mIsPoweredUp;
        private PlayAreaCoords mBoardLocation;
		private PlayArea mPlayArea;

		private List<Texture2D> mNormalTextures;
		private int mCurrentTexture;
		private TimeSpan mTextureSwapTime;

		private Vector2 mScreenOffset = new Vector2 (0.0f, 0.0f);

		private bool mIsMoving;
		private PlayAreaCoords mMoveFrom;
		private PlayAreaCoords mMoveTo;
		private Timer mMoveTimer;
		private Vector2 mActualPosition;

		private bool mIsMarked;
		private bool mIsDisappeared;
		private Timer mMarkTimer;
		private Color mColor;

		/// <summary>
		/// Constructs a Gem object.
		/// </summary>
		/// <param name="type">The color of the gem.</param>
		/// <param name="textures">The textures the gem should use.</param>
		/// <param name="playArea">The PlayArea this gem belongs to. Required for some callbacks.</param>
		public Gem(GemType type, List<Texture2D> textures, PlayArea playArea)
		{
			mType = type;
			mPlayArea = playArea;

			mNormalTextures = textures;
			mCurrentTexture = 0;
			mTextureSwapTime = new TimeSpan(0, 0, 0, 0, 50);

			mIsPoweredUp = false;
			mIsSelected = false;
            mBoardLocation = new PlayAreaCoords();

			mIsMoving = false;
			mMoveFrom = new PlayAreaCoords();
			mMoveTo = new PlayAreaCoords();
			mMoveTimer = new Timer(new TimeSpan(1));

			mIsDisappeared = false;
			mIsMarked = false;
			mMarkTimer = new Timer(new TimeSpan (0, 0, 0, 0,250));
			mColor = Color.White;
		}

		/// <summary>
		/// Begins the animation to swap a gem with another gem.
		/// </summary>
		/// <param name="with">The gem to swap this with.</param>
        public void Swap(Gem with)
        {
            PlayAreaCoords temp = with.mMoveTo;
			with.MoveTo(mMoveTo);
			MoveTo(temp);
        }

		/// <summary>
		/// Be very careful when calling this because it will affect any animations which are underway.
		/// Sets the starting location for the movement animation.
		/// </summary>
		/// <param name="coords">Where to start the move animation from.</param>
		public void SetStartingLocation(PlayAreaCoords coords)
		{
			mMoveFrom = coords;
		}

		/// <summary>
		/// Sets up the movement animation.
		/// </summary>
		/// <param name="newLoc">Where the gem should move to.</param>
		public void MoveTo(PlayAreaCoords newLoc)
		{
			if (newLoc != mMoveTo)
			{
				mMoveFrom = BoardLocation;
				mMoveTo = newLoc;
				mMoveTimer = new Timer(new TimeSpan(0, 0, 0, 0, 250));
				mIsMoving = true;
				mActualPosition = new Vector2 ((float)OnScreenLocation.X, (float)OnScreenLocation.Y);
			}
		}

		/// <summary>
		/// Checks if the coordinates passed in are inside the Gem. Useful for detecting clicks.
		/// </summary>
		/// <param name="coords">The coordinates to check.</param>
		/// <returns>True if the coordinates are inside the Gem and the Gem is not moving.</returns>
		public bool Contains (Vector2 coords)
		{
			//we don't want people to be able to click gems while they are moving
			//this is kind of hacky, should maybe be done in another way...
			if (mIsMoving)
			{
				System.Diagnostics.Debug.WriteLine("Blocked a call to contains because the gem is moving.");
				return false;
			}

			return OnScreenLocation.Intersects (new Rectangle ((int)coords.X, (int)coords.Y, 1, 1));
		}

		/// <summary>
		/// Gets or sets the Gems power up status.
		/// </summary>
        public bool IsPoweredUp
        {
            get { return mIsPoweredUp; }
            set { mIsPoweredUp = value; }
        }

		/// <summary>
		/// Gets or sets the Gems selected status. Being selected will cause the gem to rotate.
		/// </summary>
		public bool IsSelected
		{
			get { return mIsSelected; }
			set { mIsSelected = value; }
		}

		/// <summary>
		/// Gets the status of the Gems disappear animation. True if the gem has completely disappeared.
		/// </summary>
		public bool IsDisappeared
		{
			get { return mIsDisappeared; }
		}

		/// <summary>
		/// Gets the color of the gem.
		/// </summary>
		public GemType Type
		{
			get { return mType; }
		}

		/// <summary>
		/// Updates the Gems animations. Call this once per frame.
		/// </summary>
		/// <param name="gameTime">How much time has passed since the last call to update.</param>
        public void Update(GameTime gameTime)
        {
			UpdateSpin(gameTime);
			UpdateMovement(gameTime);
			UpdateDisappearing(gameTime);
        }

		/// <summary>
		/// Updates the Gems disappearing animation. Will call PlayArea.GemDisappearAnimationComplete when the animation is done.
		/// </summary>
		/// <param name="gameTime">How much time has passed since the last call.</param>
		private void UpdateDisappearing(GameTime gameTime)
		{
			if (mIsMarked)
			{
				bool disappeared = mMarkTimer.Update(gameTime.ElapsedGameTime);
				mColor = new Color(1.0f, 1.0f, 1.0f, 1.0f - (float)mMarkTimer.PercentComplete());

//				if (disappeared)
				if (mMarkTimer.PercentComplete () > 0.5)
				{
					mIsDisappeared = true;
					mPlayArea.GemDisappearAnimationComplete(this);
				}
			}
		}

		/// <summary>
		/// Updates the Gems movement animation. Must be called once per frame. Will call PlayArea.GemMoveanimationCompleted () when done.
		/// </summary>
		/// <param name="gameTime">How much time has passed since the last call.</param>
		private void UpdateMovement(GameTime gameTime)
		{
			if (mIsMoving)
			{
				bool moveDone = mMoveTimer.Update(gameTime.ElapsedGameTime);
				CalculateScreenPosition();

				if (moveDone)
				{
					System.Diagnostics.Debug.WriteLine("Gem movement animation complete.");
					mIsMoving = false;
					mIsSelected = false; //lettign the gem keep spinning it is moving makes things look a little better
					BoardLocation = mMoveTo;
					mPlayArea.GemMoveAnimationCompleted();
				}
			}
		}

		/// <summary>
		/// Updates the Gems rotate animation if the Gem is selected. Must be called once per frame.
		/// </summary>
		/// <param name="gameTime">How much time has passed since the last call.</param>
		private void UpdateSpin(GameTime gameTime)
		{
			if (mIsSelected)
			{
				mTextureSwapTime -= gameTime.ElapsedGameTime;
				while (mTextureSwapTime.TotalMilliseconds < 0.0)
				{
					mTextureSwapTime += new TimeSpan(0, 0, 0, 0, 50);
					++mCurrentTexture;
					if (mCurrentTexture >= mNormalTextures.Count)
						mCurrentTexture = 0;
				}
			}
			else
			{
				mCurrentTexture = 0;
			}
		}

		/// <summary>
		/// Where this Gem is located on the board. Not on screen coordinates.
		/// </summary>
		public PlayAreaCoords BoardLocation
		{
			get { return mBoardLocation; }
			set { mBoardLocation = value; }
		}

		/// <summary>
		/// Where the gem is being drawn on the screen.
		/// </summary>
		public Rectangle OnScreenLocation
		{
			get
			{
				return new Rectangle (
					(int)mActualPosition.X,
					(int)mActualPosition.Y,
					GlobalVars.GemSizeX,
					GlobalVars.GemSizeY);
			}
		}

		/// <summary>
		/// Updates the gems on screen position based on the movement timer.
		/// </summary>
		private void CalculateScreenPosition()
		{
			float percent = (float)mMoveTimer.PercentComplete();
			Vector2 moveFrom = mMoveFrom;
			Vector2 moveTo = mMoveTo;

			mActualPosition.X = moveFrom.X + ((moveTo.X - moveFrom.X) * percent);
			mActualPosition.Y = moveFrom.Y + ((moveTo.Y - moveFrom.Y) * percent);
		}

		/// <summary>
		/// Draws the Gem on the screen taking into account all the various animations.
		/// </summary>
		/// <param name="spriteBatch">The SpriteBatch that the Gem will be drawn with.</param>
		public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(
				mNormalTextures [mCurrentTexture],
				OnScreenLocation,
				mColor);
        }

		/// <summary>
		/// Mark the gem as being part of a line and begins the disappear animation.
		/// </summary>
		public void Matched()
		{
			if (!mIsMarked)
			{
				mIsMarked = true;
				System.Diagnostics.Debug.WriteLine("Gem at " + BoardLocation.X + ", " + BoardLocation.Y + " is matched!");
			}
		}
	}
}
