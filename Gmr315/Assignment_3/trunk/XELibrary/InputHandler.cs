#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace XELibrary
{
    public class InputHandler : GameComponent, IInputHandler
    {
        private readonly KeyboardHandler keyboardState;
        private MouseState mouseState;
        private MouseState prevMouseState;

        public InputHandler(Game game) : base(game)
        {
            game.Services.AddService(typeof (IInputHandler), this);
            game.IsMouseVisible = true;
            prevMouseState = Mouse.GetState();
            keyboardState = new KeyboardHandler();
        }

        #region IInputHandler Members

        public KeyboardHandler KeyboardState
        {
            get { return keyboardState; }
        }

        public MouseState MouseState
        {
            get { return mouseState; }
        }

        public MouseState PreviousMouseState
        {
            get { return prevMouseState; }
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            keyboardState.Update();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                base.Game.Exit();
            }
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
            base.Update(gameTime);
        }
    }
}