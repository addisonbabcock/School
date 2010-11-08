using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	class Gem
	{
		private GemType mType;
		private GemAnimationState mAnimationState;
		private bool mIsExplosive;
        private PlayAreaCoords mBoardLocation;

		private List<Texture2D> mNormalTextures;
		private int mCurrentTexture;
		private TimeSpan mTextureSwapTime;

		public Gem(GemType type, List<Texture2D> textures)
		{
			mType = type;

			mNormalTextures = textures;
			mCurrentTexture = 0;
			mTextureSwapTime = new TimeSpan(0, 0, 0, 0, 50);

			mAnimationState = GemAnimationState.Idle;
			mIsExplosive = false;
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

        public bool IsExplosive
        {
            get { return mIsExplosive; }
            set { mIsExplosive = value; }
        }

		public GemType Type
		{
			get { return mType; }
		}

        public void Update(GameTime gameTime)
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

        public void Draw(SpriteBatch spriteBatch)
        {
			Color color = new Color ();
			color.A = 255;
			color.R = 255;
			color.G = 255;
			color.B = 255;

			spriteBatch.Draw(
				mNormalTextures [mCurrentTexture],
				new Rectangle(
					0,
					0,
					100,
					100),
				color);
        }
	}
}
