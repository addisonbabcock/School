using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BATMAN
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D myHappyFaceTexture;
		Texture2D myBackgroundTexture;
		Texture2D mySadFace;
		Texture2D BATMAN;
		Rectangle myBackground_Rect;

		Random mRNG;

		List <Vector2> mHappyFaces;


		public Game1 ()
		{
			mRNG = new Random ();
			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 600;
			graphics.SynchronizeWithVerticalRetrace = true;

			Content.RootDirectory = "Content";

			Window.AllowUserResizing = false;
			Window.ClientSizeChanged += new EventHandler (Window_ClientSizeChanged);

			myBackground_Rect = new Rectangle (
				0,
				0,
				graphics.PreferredBackBufferWidth,
				graphics.PreferredBackBufferHeight);

			mHappyFaces = new List<Vector2> (10);
			//foreach (Vector2 vec in mHappyFaces)
			for (int i = 0; i < mHappyFaces.Capacity; ++i)
				mHappyFaces.Add (new Vector2 (
					(float)mRNG.Next (graphics.PreferredBackBufferWidth),
					(float)mRNG.Next (graphics.PreferredBackBufferHeight)));
		}

		void Window_ClientSizeChanged (object sender, EventArgs e)
		{
			myBackground_Rect = new Rectangle (
				0,
				0,
				graphics.GraphicsDevice.Viewport.Width,
				graphics.GraphicsDevice.Viewport.Height);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic 

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (GraphicsDevice);

			myHappyFaceTexture = Content.Load<Texture2D> ("SmallHappyFace");
			myBackgroundTexture = Content.Load<Texture2D> ("happy face");
			mySadFace = Content.Load<Texture2D> ("sad_face");
			BATMAN = Content.Load<Texture2D> ("batmanLogo");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent ()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit ();

			if (Keyboard.GetState ().IsKeyDown (Keys.Escape))
				Exit ();

			// TODO: Add your update logic here

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.Purple);

			// TODO: Add your drawing code here
			//Start Draw
			spriteBatch.Begin ();

			//BATMAN!
			Rectangle batmanLoc = new Rectangle (
				graphics.GraphicsDevice.Viewport.Width / 2,
				graphics.GraphicsDevice.Viewport.Height / 2,
				1000,
				1000);
			Rectangle batmanSource = new Rectangle (0, 0, BATMAN.Width, BATMAN.Height);
			Vector2 batmanCenter = new Vector2 (BATMAN.Width * 0.5f, BATMAN.Height * 0.5f);
			float rotate = (float)gameTime.TotalGameTime.TotalMilliseconds / 250.0f;
			while (rotate > Math.PI * 2)
				rotate -= (float)Math.PI * 2.0f;
			spriteBatch.Draw (
				BATMAN,
				batmanLoc,
				batmanSource,
				Color.White,
				rotate,
				batmanCenter,
				SpriteEffects.None,
				0.0f);

			for (int i = 0; i < mHappyFaces.Count; ++i)
			{
				Vector2 vec = mHappyFaces [i];
				vec.Y += (float)gameTime.ElapsedGameTime.Milliseconds;
				if (vec.Y > graphics.GraphicsDevice.Viewport.Height)
					vec.Y = -100.0f;
				mHappyFaces [i] = vec;

				Rectangle sad_face = new Rectangle (
					(int)vec.X,
					(int)vec.Y,
					100,
					100);
				spriteBatch.Draw (mySadFace, sad_face, Color.White);
			}

			//end draw
			spriteBatch.End ();


			base.Draw (gameTime);
		}
	}
}
