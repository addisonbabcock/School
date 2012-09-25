using System;
using Common._2D;
using Microsoft.Xna.Framework;

namespace Common.Triggers
{
    public abstract class TriggerRegion : IDisposable
    {
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        //returns true if an entity of the given size and position is intersecting
        //the trigger region.
        public abstract bool isTouching(Vector2 EntityPos, float EntityRadius);
    }


    //--------------------------- TriggerRegion_Circle ----------------------------
    //
    //  class to define a circular region of influence
    //-----------------------------------------------------------------------------
    public class TriggerRegion_Circle : TriggerRegion
    {
        //the center of the region

        //the radius of the region
        private readonly float m_dRadius;
        private readonly Vector2 m_vPos;


        public TriggerRegion_Circle(Vector2 pos,
                                    float radius)
        {
            m_dRadius = radius;
            m_vPos = pos;
        }

        public override bool isTouching(Vector2 pos, float EntityRadius)
        {
            return Vector2.DistanceSquared(m_vPos, pos) < (EntityRadius + m_dRadius)*(EntityRadius + m_dRadius);
        }
    }


    //--------------------------- TriggerRegion_Rectangle -------------------------
    //
    //  class to define a circular region of influence
    //-----------------------------------------------------------------------------
    public class TriggerRegion_Rectangle : TriggerRegion
    {
        private InvertedAABBox2D m_pTrigger;


        public TriggerRegion_Rectangle(Vector2 TopLeft,
                                       Vector2 BottomRight)
        {
            m_pTrigger = new InvertedAABBox2D(TopLeft, BottomRight);
        }

        ~TriggerRegion_Rectangle()
        {
            m_pTrigger.Dispose();
        }

        //there's no need to do an accurate (and expensive) circle v
        //rectangle intersection test. Instead we'll just test the bounding box of
        //the given circle with the rectangle.
        public override bool isTouching(Vector2 pos, float EntityRadius)
        {
            var Box = new InvertedAABBox2D(new Vector2(pos.X - EntityRadius, pos.Y - EntityRadius),
                                           new Vector2(pos.X + EntityRadius, pos.Y + EntityRadius));

            return Box.isOverlappedWith(ref m_pTrigger);
        }
    }
}