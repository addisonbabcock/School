using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Raven
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch SpriteBatch;
        public static PrimitiveBatch PrimitiveBatch;
        public static SpriteFont GameFont;
        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;
        private Raven_Game raven;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            raven = new Raven_Game(this);
            Raven_UserOptions.Instance().m_bSmoothPathsQuick = true;
            this.IsMouseVisible = true;
            Components.Add(raven);
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
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousKeyboardState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            base.Initialize();
        }

        public static bool WasKeyPressed(Keys key)
        {
            return previousKeyboardState.IsKeyDown(key) && currentKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public static Vector2 GetClientCursorPosition()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PrimitiveBatch = new PrimitiveBatch(GraphicsDevice);
            GameFont = Content.Load<SpriteFont>(@"Arial");            
            // TODO: use this.Content to load your game content here
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
            Raven_UserOptions userOptions = Raven_UserOptions.Instance();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            previousKeyboardState = currentKeyboardState;
            previousMouseState = currentMouseState;
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if(WasKeyPressed(Keys.P))
            {
                raven.TogglePause();
            }
            if(WasKeyPressed(Keys.D1))
            {
                raven.ChangeWeaponOfPossessedBot((int) Raven_Objects.type_blaster);
            }
            if (WasKeyPressed(Keys.D2))
            {
                raven.ChangeWeaponOfPossessedBot((int)Raven_Objects.type_shotgun);
            }
            if (WasKeyPressed(Keys.D3))
            {
                raven.ChangeWeaponOfPossessedBot((int)Raven_Objects.type_rocket_launcher);
            }
            if(WasKeyPressed(Keys.D4))
            {
                raven.ChangeWeaponOfPossessedBot((int) Raven_Objects.type_rail_gun);
            }
            if(WasKeyPressed(Keys.X))
            {
                raven.ExorciseAnyPossessedBot();
            }
            if(WasKeyPressed(Keys.Up))
            {
                raven.AddBot("Raven_Bot");
            }
            if(WasKeyPressed(Keys.Down))
            {
                raven.RemoveBot();
            }
            if(WasKeyPressed(Keys.G))
            {
                userOptions.m_bShowGraph = !userOptions.m_bShowGraph;
            }
            if(WasKeyPressed(Keys.N))
            {
                userOptions.m_bShowNodeIndices = !userOptions.m_bShowNodeIndices;
            }
            if (WasKeyPressed(Keys.E))
            {
                userOptions.m_bSmoothPathsQuick = !userOptions.m_bSmoothPathsQuick;
                userOptions.m_bSmoothPathsPrecise = false;
            }
            if (WasKeyPressed(Keys.W))
            {
                userOptions.m_bSmoothPathsPrecise = !userOptions.m_bSmoothPathsPrecise;
                userOptions.m_bSmoothPathsQuick = false;
            }
            if (WasKeyPressed(Keys.I))
            {
                userOptions.m_bShowBotIDs = !userOptions.m_bShowBotIDs;
            }
            if (WasKeyPressed(Keys.H))
            {
                userOptions.m_bShowBotHealth = !userOptions.m_bShowBotHealth;
            }
            if (WasKeyPressed(Keys.T))
            {
                userOptions.m_bShowTargetOfSelectedBot = !userOptions.m_bShowTargetOfSelectedBot;
            }
            if (WasKeyPressed(Keys.S))
            {
                userOptions.m_bShowOpponentsSensedBySelectedBot = !userOptions.m_bShowOpponentsSensedBySelectedBot;
            }
            if (WasKeyPressed(Keys.F))
            {
                userOptions.m_bOnlyShowBotsInTargetsFOV = !userOptions.m_bOnlyShowBotsInTargetsFOV;
            }
            if (WasKeyPressed(Keys.O))
            {
                userOptions.m_bShowScore = !userOptions.m_bShowScore;
            }
            if (WasKeyPressed(Keys.U))
            {
                userOptions.m_bShowGoalsOfSelectedBot = !userOptions.m_bShowGoalsOfSelectedBot;
            }
            base.Update(gameTime);
        }

        protected void ReportScores()
        {
            SpriteBatch.Begin();
            int row = 0;
            string infoString = "Game Over";
            List<AbstractBot> bots = raven.GetAllBots();

            foreach(AbstractBot bot in bots)
            {
                string botIdString = "Bot: " + bot.GetName() + "-" + bot.ID();
                string botScoreString = "    Kills: " + bot.Score();
                string botDeathString = "    Deaths: " + bot.Deaths();
                string scoreString = "    Score: " + (bot.Score() - bot.Deaths());

                SpriteBatch.DrawString(GameFont, string.Format("{0}{1}{2}{3}", botIdString, botScoreString, botDeathString, scoreString), new Vector2(0, GameFont.LineSpacing*(22+row)), Color.Red );
                row++;
            }
            SpriteBatch.End();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            //raven.Draw(gameTime);
            base.Draw(gameTime);

            if(raven.IsTimeUp())
            {
                ReportScores();
            }
        }

        public static bool GetRightMouseButtonClicked()
        {
            return previousMouseState.RightButton == ButtonState.Pressed &&
                   currentMouseState.RightButton == ButtonState.Released;
        }

        public static bool GetLeftMouseButtonClicked()
        {
            return previousMouseState.LeftButton == ButtonState.Pressed &&
                   currentMouseState.LeftButton == ButtonState.Released;
        }
    }
}
