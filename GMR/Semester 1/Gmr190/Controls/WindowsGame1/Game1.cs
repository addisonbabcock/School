using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Controls
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D mSquiggle;
        Vector2 mSquiggleLoc;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            mSquiggleLoc = new Vector2();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mSquiggle = Content.Load<Texture2D>("SquiggleThing");
            //mSquiggle.GenerateMipMaps(TextureFilter.Anisotropic);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown (Keys.Escape))
            {
                Exit();
            }

            double speed = 0.25;
            if (kb.IsKeyDown(Keys.W) || kb.IsKeyDown(Keys.Up))
            {
                mSquiggleLoc.Y -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            }
            if (kb.IsKeyDown(Keys.S) || kb.IsKeyDown(Keys.Down))
            {
                mSquiggleLoc.Y += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            }
            if (kb.IsKeyDown(Keys.A) || kb.IsKeyDown(Keys.Left))
            {
                mSquiggleLoc.X -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            }
            if (kb.IsKeyDown(Keys.D) || kb.IsKeyDown(Keys.Right))
            {
                mSquiggleLoc.X += (float)(gameTime.ElapsedGameTime.TotalMilliseconds * speed);
            }

            MouseState mouse = Mouse.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                mSquiggleLoc = new Vector2((float)mouse.X, (float)mouse.Y);
            }

            BoundSquiggle();

            base.Update(gameTime);
        }

        void BoundSquiggle()
        {
            if (mSquiggleLoc.X < 0)
            {
                mSquiggleLoc.X = 0;
            }
            if (mSquiggleLoc.X + mSquiggle.Width > graphics.GraphicsDevice.Viewport.Width)
            {
                mSquiggleLoc.X = graphics.GraphicsDevice.Viewport.Width - mSquiggle.Width;
            }
            if (mSquiggleLoc.Y < 0)
            {
                mSquiggleLoc.Y = 0;
            }
            if (mSquiggleLoc.Y + mSquiggle.Height > graphics.GraphicsDevice.Viewport.Height)
            {
                mSquiggleLoc.Y = graphics.GraphicsDevice.Viewport.Height - mSquiggle.Height;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(mSquiggle, mSquiggleLoc, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
