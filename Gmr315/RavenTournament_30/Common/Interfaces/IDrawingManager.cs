#region Using

using Common._2D;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Interfaces
{
    public interface IDrawingManager
    {
        PrimitiveBatch GetPrimitiveBatch();
        SpriteBatch GetSpriteBatch();
    }
}