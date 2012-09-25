#region Using

using Common._2D;
using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Managers
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DrawingManager : GameComponent, IDrawingManager
    {
        private static PrimitiveBatch primitiveBatch;
        private static SpriteBatch spriteBatch;

        public DrawingManager(Microsoft.Xna.Framework.Game game, SpriteBatch batch, PrimitiveBatch primitive)
            : base(game)
        {
            // TODO: Construct any child components here
            game.Services.AddService(typeof (IDrawingManager), this);
            spriteBatch = batch;
            primitiveBatch = primitive;
        }

        #region IDrawingManager Members

        public PrimitiveBatch GetPrimitiveBatch()
        {
            return primitiveBatch;
        }

        public SpriteBatch GetSpriteBatch()
        {
            return spriteBatch;
        }

        #endregion
    }
}