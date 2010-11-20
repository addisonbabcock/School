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
	/// This is the main type for Rejeweled. 
	/// </summary>
	public class Rejeweled : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		PlayArea mPlayArea;
		MouseParser mMouseParser;
		RuleChecker mRuleChecker;
		Random mRNG;

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
			mRNG = new Random();

			BackgroundManager bgManager = new BackgroundManager(this, mRNG);
			Components.Add(bgManager);

			mPlayArea = new PlayArea(this, mRNG);
			Components.Add(mPlayArea);

			ScoreManager scManager = new ScoreManager(this);
			Components.Add(scManager);
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
			mMouseParser = new MouseParser ();

			base.LoadContent();

			mPlayArea.CreateNewBoard();
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
			} while (mouseEvent != null);

			base.Update (gameTime);
		}

		/// <summary>
		/// Deals with a mouse click or drag and passes the message along.
		/// </summary>
		/// <param name="mouseEvent">The mouse event to be handled.</param>
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

		/// <summary>
		/// Deals with a mouse click event.
		/// </summary>
		/// <param name="mouseEvent">The mouse click event to handle.</param>
		private void HandleMouseClick (MouseEvent mouseEvent)
		{
			Debug.WriteLine ("Mouse click at coords: " + (int)mouseEvent.MouseLocation.X + ", " + (int)mouseEvent.MouseLocation.Y);
			mPlayArea.MouseClicked (mouseEvent);
		}

		/// <summary>
		/// Deals with a mouse drag event.
		/// </summary>
		/// <param name="mouseEvent">The mouse drag event to handle.</param>
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
			GraphicsDevice.Clear (GlobalVars.ClearColor);
			base.Draw(gameTime);
		}
	}
}
