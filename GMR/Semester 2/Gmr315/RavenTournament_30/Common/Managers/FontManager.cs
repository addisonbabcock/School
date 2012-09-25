#region Using

using System;
using System.Collections.Generic;
using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Managers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FontManager : GameComponent, IFontManager
    {
        private readonly Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public FontManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            game.Services.AddService(typeof (IFontManager), this);
        }

        #region IFontManager Members

        public SpriteFont GetFont(string name)
        {
            if (fonts.ContainsKey(name))
            {
                return fonts[name];
            }
            throw new ArgumentException("The given font name does not exist", "name");
        }

        #endregion

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            fonts.Add("Default", Game.Content.Load<SpriteFont>(@"Fonts\DefaultGameFont"));
            base.Initialize();
        }
    }
}