#region Using

using Microsoft.Xna.Framework;

#endregion

namespace Common.Game
{
    public interface IEntity
    {
        Vector2 Pos();
        float BRadius();
        void Tag();
        void UnTag();
        void SetPos(Vector2 position);
    }
}