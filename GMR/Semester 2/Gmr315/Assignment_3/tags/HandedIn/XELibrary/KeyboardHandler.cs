#region Using

using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace XELibrary
{
    public class KeyboardHandler
    {
        private KeyboardState keyboardState;
        private KeyboardState previousKeyboardState = Keyboard.GetState();

        public bool HasReleasedKey(Keys key)
        {
            return (keyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key));
        }

        public bool IsHoldingKey(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyDown(key));
        }

        public bool IsKeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public void Update()
        {
            previousKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
        }

        public bool WasKeyPressed(Keys key)
        {
            return (keyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key));
        }

        public Keys[] GetPressedKeys()
        {
            return keyboardState.GetPressedKeys();
        }
        
        public Keys[] GetKeysThatWerePressed()
        {
            List<Keys> keys = new List<Keys>(previousKeyboardState.GetPressedKeys());
            foreach(Keys key in keyboardState.GetPressedKeys())
            {
                if(keys.Contains(key))
                {
                    keys.Remove(key);
                }
            }
            return keys.ToArray();
        }
    }
}