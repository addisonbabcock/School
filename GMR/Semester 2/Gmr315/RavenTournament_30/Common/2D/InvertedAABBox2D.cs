using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common._2D
{
    public class InvertedAABBox2D : IDisposable
    {
        private Vector2 m_vCenter;
        private Vector2 m_vBottomRight;
        private Vector2 m_vTopLeft;


        public InvertedAABBox2D(Vector2 tl,
                                Vector2 br)
        {
            m_vTopLeft = tl;
            m_vBottomRight = br;
            m_vCenter = (tl + br)/2.0f;
        }


        //returns true if the bbox described by other intersects with this one
        public bool isOverlappedWith(ref InvertedAABBox2D other)
        {
            return !((other.Top() > Bottom()) ||
                     (other.Bottom() < Top()) ||
                     (other.Left() > Right()) ||
                     (other.Right() < Left()));
        }


        public Vector2 TopLeft()
        {
            return m_vTopLeft;
        }

        public Vector2 BottomRight()
        {
            return m_vBottomRight;
        }

        public float Top()
        {
            return m_vTopLeft.Y;
        }

        public float Left()
        {
            return m_vTopLeft.X;
        }

        public float Bottom()
        {
            return m_vBottomRight.Y;
        }

        public float Right()
        {
            return m_vBottomRight.X;
        }

        public Vector2 Center()
        {
            return m_vCenter;
        }

        public void Render(PrimitiveBatch batch, bool renderCenter)
        {
            Render(batch, renderCenter, Color.White);
        }
        public void Render(PrimitiveBatch batch, bool RenderCenter, Color color)
        {
            Drawing.DrawRectangle(batch, color, Left(), Top(), Right() - Left(), Bottom() - Top());

            if (RenderCenter)
            {
                Drawing.DrawCircle(batch, m_vCenter, 5, color);
            }
        }

        public void Dispose()
        {

        }
    }
}