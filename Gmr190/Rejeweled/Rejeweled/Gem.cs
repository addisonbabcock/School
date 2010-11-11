using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	class Gem : IRenderable
	{
		private GemType mType;
		private GemAnimationState mAnimationState;
		private bool mIsPoweredUp;
        private PlayAreaCoords mBoardLocation;

		private List<Texture2D> mNormalTextures;
		private int mCurrentTexture;
		private TimeSpan mTextureSwapTime;

		private const int mGemSizeX = 72;
		private const int mGemSizeY = 72;
		private Vector2 mScreenOffset = new Vector2 (0.0f, 0.0f);

		public Gem(GemType type, List<Texture2D> textures)
		{
			mType = type;

			mNormalTextures = textures;
			mCurrentTexture = 0;
			mTextureSwapTime = new TimeSpan(0, 0, 0, 0, 50);

			mAnimationState = GemAnimationState.Idle;
			mIsPoweredUp = false;
            mBoardLocation = new PlayAreaCoords();
		}

        public void Swap(Gem with)
        {
            PlayAreaCoords temp = with.mBoardLocation;
            with.mBoardLocation = mBoardLocation;
            mBoardLocation = temp;
        }

        public bool IsAt(PlayAreaCoords coords)
        {
            return mBoardLocation == coords;
        }

        public bool IsPoweredUp
        {
            get { return mIsPoweredUp; }
            set { mIsPoweredUp = value; }
        }

		public GemType Type
		{
			get { return mType; }
		}

        public override void Update(GameTime gameTime)
        {
            //just shutting up compiler warnings for now...
            //fill this in later.
            switch (mAnimationState)
            {
                case GemAnimationState.Disappearing:
                    break;
            }

			mTextureSwapTime -= gameTime.ElapsedGameTime;
			if (mTextureSwapTime.TotalMilliseconds < 0.0)
			{
				mTextureSwapTime = new TimeSpan(0, 0, 0, 0, 50);
				++mCurrentTexture;
				if (mCurrentTexture >= mNormalTextures.Count)
					mCurrentTexture = 0;
			}
        }

		public PlayAreaCoords BoardLocation
		{
			get { return mBoardLocation; }
			set { mBoardLocation = value; }
		}

		public Rectangle OnScreenLocation
		{
			get
			{
				return new Rectangle (
					(int)(mBoardLocation.X * mGemSizeX + mScreenOffset.X),
					(int)(mBoardLocation.Y * mGemSizeY + mScreenOffset.Y),
					mGemSizeX,
					mGemSizeY);
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
        {
			Color color = new Color ();
			color.A = 255;
			color.R = 255;
			color.G = 255;
			color.B = 255;

			spriteBatch.Draw(
				mNormalTextures [mCurrentTexture],
				OnScreenLocation,
				color);
        }
	}
}
