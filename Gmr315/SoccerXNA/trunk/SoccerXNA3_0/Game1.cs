#region Using

using System;
using Common._2D;
using Common.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace SoccerXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public PrimitiveBatch primitiveBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
           // this.TargetElapsedTime = TimeSpan.FromSeconds(1 / (float) ParamLoader.Instance.FrameRate);
        }

        private SoccerPitch SoccerPitch { get; set; }
        private FontManager FontManager { get; set; }
        private DrawingManager DrawingManager { get; set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.DisplayMode.Width / 2;
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.DisplayMode.Height / 2;
            graphics.ApplyChanges();
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
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);
            FontManager = new FontManager(this);
            FontManager.Initialize();
            Components.Add(FontManager);
            DrawingManager = new DrawingManager(this, spriteBatch, primitiveBatch);
            Components.Add(DrawingManager);
            SoccerPitch = new SoccerPitch(this, ParamLoader.Instance.RedTeamId, ParamLoader.Instance.BlueTeamId);
            Components.Add(SoccerPitch);
            
            
            // TODO: use this.Content to load your game content here
        }

        double adjustedScore(long winTime, long loseTime, int loseScore)
        {
            long diff = winTime - loseTime;
            double fraction = diff / (double)winTime;
            return (fraction * 5) + loseScore;
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            //SoccerPitch.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);
            spriteBatch.Begin();
            primitiveBatch.Begin(PrimitiveType.LineList);
            //SoccerPitch.Draw(gameTime);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            primitiveBatch.End();
            spriteBatch.End();
        }
    }
}