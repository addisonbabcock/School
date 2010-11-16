using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	class Gem
	{
		private GemType mType;
		private bool mIsSelected;
		private bool mIsPoweredUp;
 //       private PlayAreaCoords mBoardLocation;

		private List<Texture2D> mNormalTextures;
		private int mCurrentTexture;
		private TimeSpan mTextureSwapTime;

		private Vector2 mScreenOffset = new Vector2 (0.0f, 0.0f);

		private bool mIsMoving;
		private PlayAreaCoords mMoveFrom;
		private PlayAreaCoords mMoveTo;
		private Timer mMoveTimer;
		private Vector2 mActualPosition;

		public Gem(GemType type, List<Texture2D> textures)
		{
			mType = type;

			mNormalTextures = textures;
			mCurrentTexture = 0;
			mTextureSwapTime = new TimeSpan(0, 0, 0, 0, 50);

			mIsPoweredUp = false;
			mIsSelected = false;
           // mBoardLocation = new PlayAreaCoords();

			mIsMoving = false;
			mMoveFrom = new PlayAreaCoords();
			mMoveTo = new PlayAreaCoords();
			mMoveTimer = new Timer(new TimeSpan(1));
		}

        public void Swap(Gem with)
        {
            PlayAreaCoords temp = with.mMoveTo;
			with.MoveTo(mMoveTo);
			MoveTo(temp);
        }

		public void MoveTo(PlayAreaCoords newLoc)
		{
			if (newLoc != mMoveTo)
			{
				mMoveFrom = mMoveTo;
				mMoveTo = newLoc;
				mMoveTimer = new Timer(new TimeSpan(0, 0, 0, 0, 250));
				mIsMoving = true;
				mActualPosition = new Vector2 ((float)OnScreenLocation.X, (float)OnScreenLocation.Y);
			}

			//mBoardLocation = newLoc;
		}

        public bool IsAt(PlayAreaCoords coords)
        {
            return mMoveTo == coords;
        }

		public bool Contains (Vector2 coords)
		{
			//we don't want people to be able to click gems while they are moving
			if (mIsMoving)
			{
				System.Diagnostics.Debug.WriteLine("Blocked a call to contains because the gem is moving.");
				return false;
			}

			return OnScreenLocation.Intersects (new Rectangle ((int)coords.X, (int)coords.Y, 1, 1));
		}

        public bool IsPoweredUp
        {
            get { return mIsPoweredUp; }
            set { mIsPoweredUp = value; }
        }

		public bool IsSelected
		{
			get { return mIsSelected; }
			set { mIsSelected = value; }
		}

		public GemType Type
		{
			get { return mType; }
		}

        public void Update(GameTime gameTime)
        {
			UpdateSpin(gameTime);
			UpdateMovement(gameTime);
        }

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
				}
			}
		}

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

		public PlayAreaCoords BoardLocation
		{
			get { return mMoveTo; }
			set { mMoveFrom = value; }
		}

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

		private void CalculateScreenPosition()
		{
			float percent = (float)mMoveTimer.PercentComplete();
			Vector2 moveFrom = mMoveFrom;
			Vector2 moveTo = mMoveTo;

			mActualPosition.X = moveFrom.X + ((moveTo.X - moveFrom.X) * percent);
			mActualPosition.Y = moveFrom.Y + ((moveTo.Y - moveFrom.Y) * percent);
		}

		public void Draw(SpriteBatch spriteBatch)
        {
			spriteBatch.Draw(
				mNormalTextures [mCurrentTexture],
				OnScreenLocation,
				Color.White);
        }
	}
}
