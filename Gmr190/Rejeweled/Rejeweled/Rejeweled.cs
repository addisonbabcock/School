using System;
using System.Diagnostics;
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

namespace Rejeweled
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Rejeweled : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<List<Texture2D>> mGemTextures;
		PlayArea mPlayArea;
		MouseParser mMouseParser;
		RuleChecker mRuleChecker;

		public Rejeweled ()
		{
			Content.RootDirectory = "Content";

			graphics = new GraphicsDeviceManager (this);
			graphics.PreferredBackBufferWidth = 1024;
			graphics.PreferredBackBufferHeight = 768;
			//graphics.IsFullScreen = true;
			Window.AllowUserResizing = true;
			Window.ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
			graphics.ApplyChanges();

			GlobalVars.UpdateViewport(graphics.GraphicsDevice.Viewport);

			IsMouseVisible = true;

			mRuleChecker = new RuleChecker();
		}

		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			GlobalVars.UpdateViewport(graphics.GraphicsDevice.Viewport);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here

			base.Initialize ();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
			mGemTextures = new List<List<Texture2D>> ();

			for (int i = 0; i < 7; ++i)
			{
				mGemTextures.Add (new List<Texture2D> ());
				for (int j = 0; j < 20; ++j)
				{
					string gemName =
						"Gems\\Alpha\\Gem" +
						i +
						"\\gem" +
						i +
						"_" +
						(j + 1).ToString ("0#");
					mGemTextures [i].Add (Content.Load<Texture2D> (gemName));
				}
			}

			mPlayArea = new PlayArea (mGemTextures);
			mMouseParser = new MouseParser ();
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
			KeyboardState kbState = Keyboard.GetState ();

			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit ();
			if (kbState.IsKeyDown (Keys.Escape))
				Exit ();
			if (kbState.IsKeyDown(Keys.F11))
			{
				graphics.IsFullScreen = !graphics.IsFullScreen;
				graphics.ApplyChanges();
			}
			mMouseParser.Update (Mouse.GetState ());

			MouseEvent mouseEvent = null;
			do
			{
				mouseEvent = mMouseParser.GetNextEvent ();
				HandleMouseEvent (mouseEvent);
				
				//temporary, move me somewhere more appropriate.
				//mRuleChecker.FindMatches(mPlayArea);
			} while (mouseEvent != null);

			mPlayArea.Update (gameTime);
			
			base.Update (gameTime);
		}

		private void HandleMouseEvent (MouseEvent mouseEvent)
		{
			if (mouseEvent == null)
				return;

			switch (mouseEvent.MouseEventType)
			{
				case MouseEvent.EventType.MouseClick:
					HandleMouseClick (mouseEvent);
					break;

				case MouseEvent.EventType.MouseDrag:
					HandleMouseDrag (mouseEvent);
					break;

				default:
					Debug.WriteLine ("Managed to get here somehow?");
					break;
			}
		}

		private void HandleMouseClick (MouseEvent mouseEvent)
		{
			Debug.WriteLine ("Mouse click at coords: " + (int)mouseEvent.MouseLocation.X + ", " + (int)mouseEvent.MouseLocation.Y);
			mPlayArea.MouseClicked (mouseEvent);
		}

		private void HandleMouseDrag (MouseEvent mouseEvent)
		{
			Debug.WriteLine ("Mouse drag from " +
				(int)mouseEvent.DragStart.X + ", " + (int)mouseEvent.DragStart.Y + " to " +
				(int)mouseEvent.DragEnd.X + ", " + (int)mouseEvent.DragEnd.Y);
			mPlayArea.MouseDragged(mouseEvent);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			GraphicsDevice.Clear (Color.CornflowerBlue);

			spriteBatch.Begin (SpriteBlendMode.AlphaBlend);
			mPlayArea.Draw (spriteBatch);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
	}
}
