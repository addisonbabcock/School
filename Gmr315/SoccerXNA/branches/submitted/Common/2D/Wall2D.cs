#region Using

using System.IO;
using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common._2D
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Wall2D : DrawableGameComponent
    {
        private static PrimitiveBatch primitiveBatch;

        protected
            Vector2 m_vA,
                    m_vB,
                    m_vN;

        public
            Wall2D(Microsoft.Xna.Framework.Game game)
            : this(game, new Vector2(), new Vector2())
        {
        }

        public Wall2D(Microsoft.Xna.Framework.Game game, Vector2 A, Vector2 B) : base(game)
        {
            m_vA = A;
            m_vB = B;
            CalculateNormal();
        }

        public Wall2D(Microsoft.Xna.Framework.Game game, Vector2 A, Vector2 B, Vector2 N) : base(game)
        {
            m_vA = A;
            m_vB = B;
            m_vN = N;
        }

        public Wall2D(Microsoft.Xna.Framework.Game game, BinaryReader inputStream) : base(game)
        {
            Read(inputStream);
        }

        private PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        protected void CalculateNormal()
        {
            Vector2 temp = Vector2.Normalize(m_vB - m_vA);

            m_vN = new Vector2(-temp.Y, temp.X);
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, false, Color.White);
        }

        public void Draw(GameTime gameTime, Color color)
        {
            Draw(gameTime, false, color);
        }

        public void Draw(GameTime gameTime, bool RenderNormals, Color color)
        {
            Drawing.DrawLine(PrimitiveBatch, m_vA, m_vB, color);

            //render the normals if rqd
            if (RenderNormals)
            {
                int MidX = (int) ((m_vA.X + m_vB.X)/2);
                int MidY = (int) ((m_vA.Y + m_vB.Y)/2);

                Drawing.DrawLine(primitiveBatch, new Vector2(MidX, MidY),
                                 new Vector2(MidX + (m_vN.X*5f), MidY + (m_vN.Y*5f)), color);
            }
            base.Draw(gameTime);
        }

        public Vector2 From()
        {
            return m_vA;
        }

        public void SetFrom(Vector2 v)
        {
            m_vA = v;
            CalculateNormal();
        }

        public Vector2 To()
        {
            return m_vB;
        }

        public void SetTo(Vector2 v)
        {
            m_vB = v;
            CalculateNormal();
        }

        public Vector2 Normal()
        {
            return m_vN;
        }

        public void SetNormal(Vector2 n)
        {
            m_vN = n;
        }

        public Vector2 Center()
        {
            return (m_vA + m_vB)/2.0f;
        }

        public void Write(BinaryWriter outputStream)
        {
        }

        private void Read(BinaryReader inputStream)
        {
        }
    }
}