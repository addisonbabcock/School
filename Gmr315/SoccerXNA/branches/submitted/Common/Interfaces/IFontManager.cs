#region Using

using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Interfaces
{
    public interface IFontManager
    {
        SpriteFont GetFont(string name);
    }
}