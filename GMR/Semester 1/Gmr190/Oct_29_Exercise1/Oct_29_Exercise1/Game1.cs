using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Oct_29_Exercise1
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Sprite> mSprites;
		List<ParticleEffect> mParticles;

		Texture2D mCat;
		Texture2D mDog;
		Texture2D mSquare;
		SpriteFont mFont;

		TimeSpan mSpawnNewExplosion;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			base.Initialize();

			mSprites = new List<Sprite>();
			mParticles = new List<ParticleEffect>();

			Vector2 catPos = new Vector2(
				graphics.GraphicsDevice.Viewport.Width / 2,
				graphics.GraphicsDevice.Viewport.Height / 2);
			Vector2 catSpeed = new Vector2(0.5f, 0.0f);
			double catRotSpeed = 0.005;
			mSprites.Add(new Sprite(mCat, catPos, catSpeed, catRotSpeed, true, graphics));

			for (int i = 0; i < 10; ++i)
			{
				Vector2 dogPos = new Vector2(
					i * -1000,
					graphics.GraphicsDevice.Viewport.Height / 2);
				Vector2 dogSpeed = new Vector2(1.0f, 0.0f);
				double dogRotSpeed = -0.0025;
				mSprites.Add(new Sprite(mDog, dogPos, dogSpeed, dogRotSpeed, false, graphics));
			}

			ParticleEffect effect = new ParticleEffect(new Vector2(0.0f, 8.91f), mSquare);
			effect.AddParticles(50);
			mParticles.Add(effect);
			mSpawnNewExplosion = new TimeSpan(0, 0, 2);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			mCat = Content.Load<Texture2D>("cats_012");
			mDog = Content.Load<Texture2D>("worlds-strongest-dog");
			mSquare = Content.Load<Texture2D>("square");
			mFont = Content.Load<SpriteFont>("MyFont");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
				Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			base.Update(gameTime);

			foreach (Sprite sprite in mSprites)
				sprite.Update(gameTime);

			foreach (ParticleEffect effect in mParticles)
			{
				effect.Update(gameTime);
			}

			mSpawnNewExplosion -= gameTime.ElapsedGameTime;
			if (mSpawnNewExplosion.TotalSeconds <= 0.0)
			{
				ParticleEffect effect = new ParticleEffect(
					new Vector2(0.0f, 8.91f),
					mSquare);
				effect.AddParticles(50);
				mParticles.Add(effect);
				mSpawnNewExplosion = new TimeSpan(0, 0, 2);
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Aqua);

			spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState);
			//foreach (Sprite sprite in mSprites)
			//    sprite.Draw(spriteBatch);
			foreach (ParticleEffect effect in mParticles)
				effect.Draw(spriteBatch);
			spriteBatch.DrawString(mFont,
				((int)(1.0 / gameTime.ElapsedGameTime.TotalMilliseconds * 1000.0)).ToString(),
				new Vector2(0.0f, 0.0f),
				Color.Black);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
