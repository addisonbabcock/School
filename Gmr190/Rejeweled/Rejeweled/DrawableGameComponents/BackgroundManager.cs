using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rejeweled
{
	class BackgroundManager : DrawableGameComponent
	{
		SpriteBatch mSpriteBatch;
		List<Texture2D> mBackgrounds;
		Random mRNG;

		Timer mTimeUntilFade;
		Timer mFadeTimer;
		int mNextBackground;
		int mCurrentBackground;

		protected Rectangle ScreenArea
		{
			get
			{
				Viewport viewport = GraphicsDevice.Viewport;
				return new Rectangle(0, 0, viewport.Width, viewport.Height);
			}
		}

		protected TimeSpan FadeTime
		{
			get { return new TimeSpan(0, 0, 0, 0, 500); }
		}

		protected TimeSpan PauseTime
		{
			get { return new TimeSpan(0, 0, 15); }
		}

		public BackgroundManager(Game game, Random rng)
			: base (game)
		{
			mRNG = rng;
			mBackgrounds = new List<Texture2D>();
			mTimeUntilFade = new Timer (PauseTime);
		}

		public override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);

			Color currentColor;
			Color nextColor;

			if (mFadeTimer == null)
			{
				currentColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
				nextColor = new Color (0.0f, 0.0f, 0.0f, 0.0f);
			}
			else
			{
				float currentColorVal = (float)(1.0 - mFadeTimer.PercentComplete());
				float nextColorVal = (float)(mFadeTimer.PercentComplete());
				currentColor = new Color(currentColorVal, currentColorVal, currentColorVal, currentColorVal);
				nextColor = new Color(nextColorVal, nextColorVal, nextColorVal, nextColorVal);
			}

			mSpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
			mSpriteBatch.Draw(mBackgrounds[mCurrentBackground], ScreenArea, currentColor);
			mSpriteBatch.Draw(mBackgrounds[mNextBackground], ScreenArea, nextColor);
			mSpriteBatch.End();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (mFadeTimer == null && mTimeUntilFade.Update(gameTime.ElapsedGameTime))
			{
				mFadeTimer = new Timer(FadeTime);
			}

			if (mFadeTimer != null)
			{
				bool timerComplete = mFadeTimer.Update(gameTime.ElapsedGameTime);

				if (timerComplete)
				{
					mFadeTimer = null;
					mCurrentBackground = mNextBackground;
					//mNextBackground++;
					//if (mNextBackground >= mBackgrounds.Count)
					//	mNextBackground = 0;
					while (mCurrentBackground == mNextBackground)
						mNextBackground = mRNG.Next(0, mBackgrounds.Count);
					mTimeUntilFade = new Timer(PauseTime);
				}
			}
		}

		protected override void LoadContent()
		{
			mSpriteBatch = new SpriteBatch(GraphicsDevice);

			//load until we get an error
			try
			{
				int backgroundNumber = 0;
				while (true)
				{
					string textureName = "Backgrounds\\Background" + backgroundNumber;
					mBackgrounds.Add(Game.Content.Load<Texture2D>(textureName));
					++backgroundNumber;
				}
			}
			catch (ContentLoadException)
			{
				//don't need to do anything.
			}

			mCurrentBackground = 0;
			if (mBackgrounds.Count > 1)
				mNextBackground = 1;
		}
	}
}
