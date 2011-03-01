#region Using

using Microsoft.Xna.Framework.Input;

#endregion

namespace XELibrary
{
    public interface IInputHandler
    {
        KeyboardHandler KeyboardState { get; }

        MouseState MouseState { get; }

        MouseState PreviousMouseState { get; }
    }
}