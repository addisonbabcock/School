#region Using

using System;
using Common._2D;
using Microsoft.Xna.Framework;

#endregion

namespace SoccerXNA
{
    public class Goal : IDisposable
    {
        private readonly Vector2 m_vCenter;
        private readonly Vector2 m_vFacing;
        private readonly Vector2 m_vLeftPost;
        private readonly Vector2 m_vRightPost;

        //a vector representing the facing direction of the goal

        //each time Scored() detects a goal this is incremented
        private int m_iNumGoalsScored;


        public Goal(Vector2 left, Vector2 right, Vector2 facing)
        {
            m_vLeftPost = left;
            m_vRightPost = right;
            m_vCenter = (left + right)/2.0f;
            m_iNumGoalsScored = 0;
            m_vFacing = facing;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        ~Goal()
        {
            Dispose(false);
        }


        //-----------------------------------------------------accessor methods
        public Vector2 Center()
        {
            return m_vCenter;
        }

        public Vector2 Facing()
        {
            return m_vFacing;
        }

        public Vector2 LeftPost()
        {
            return m_vLeftPost;
        }

        public Vector2 RightPost()
        {
            return m_vRightPost;
        }

        public int NumGoalsScored()
        {
            return m_iNumGoalsScored;
        }

        public void ResetGoalsScored()
        {
            m_iNumGoalsScored = 0;
        }

        //Given the current ball position and the previous ball position,
        //this method returns true if the ball has crossed the goal line 
        //and increments m_iNumGoalsScored
        public bool Scored(SoccerBall ball)
        {
            if (Geometry.LineIntersection2D(ball.Pos(), ball.OldPos(), m_vLeftPost, m_vRightPost))
            {
                ++m_iNumGoalsScored;

                return true;
            }

            return false;
        }
    }
}