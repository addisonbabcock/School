#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace XELibrary
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FPS : DrawableGameComponent
    {
        private const float updateInterval = 1.0f;
        private float fps;
        private float frameCount;
        private float timeSinceLastUpdate;

#if DEBUG
        public FPS(Game game)
            : this(game, false, false, game.TargetElapsedTime)
        {
        }
#else
        public FPS(Game game)
            : this(game, true, false, game.TargetElapsedTime)
        {
        }
#endif

        public FPS(Game game, bool synchWithVerticalRetrace, bool isFixedTimeStep)
            : this(game, synchWithVerticalRetrace, isFixedTimeStep, game.TargetElapsedTime)
        {
        }

        public FPS(Game game, bool synchWithVerticalRetrace, bool isFixedTimeStep, TimeSpan targetElapsedTime)
            : base(game)
        {
            GraphicsDeviceManager graphics =
                (GraphicsDeviceManager) Game.Services.GetService(typeof (IGraphicsDeviceManager));
            graphics.SynchronizeWithVerticalRetrace = synchWithVerticalRetrace;
            Game.IsFixedTimeStep = isFixedTimeStep;
            Game.TargetElapsedTime = targetElapsedTime;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override sealed void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override sealed void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override sealed void Draw(GameTime gameTime)
        {
            float elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            frameCount++;
            timeSinceLastUpdate += elapsed;
            if (timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount/timeSinceLastUpdate;
                Game.Window.Title = "FPS: " + fps;
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
            base.Draw(gameTime);
        }
    }
}