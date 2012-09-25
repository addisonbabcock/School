#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using XELibrary;

#endregion

namespace BucklandXNA2
{
    public class Path : IDisposable
    {
        private bool m_bLooped;
        private List<Vector2> m_WayPoints = new List<Vector2>();

        public Path()
        {
            m_bLooped = false;
        }

        public Path(int NumWaypoints,
                    float MinX,
                    float MinY,
                    float MaxX,
                    float MaxY,
                    bool looped)
        {
            m_bLooped = looped;

            CreateRandomPath(NumWaypoints, MinX, MinY, MaxX, MaxY);

            CurrentWaypointIndex = 0;
        }

        private int CurrentWaypointIndex { get; set; }


        public Vector2 CurrentWaypoint()
        {
            if ((m_WayPoints == null) || (m_WayPoints.Count == 0))
            {
                throw new NullReferenceException("Waypoints is either uninitialized or has no items");
            }
            return m_WayPoints[CurrentWaypointIndex];
        }

        public List<Vector2> CreateRandomPath(int numberOfWayPoints, float minX, float minY, float maxX,
                                              float maxY)
        {
            m_WayPoints.Clear();

            float midX = (maxX + minX)/2.0f;
            float midY = (maxY + minY)/2.0f;

            float smaller = Math.Min(midX, midY);

            float spacing = MathHelper.Pi*2/numberOfWayPoints;

            for (int i = 0; i < numberOfWayPoints; ++i)
            {
                Vector2 temp = new Vector2(Utils.RandInRange(minX, maxX),
                                           Utils.RandInRange(minY, maxY));

                m_WayPoints.Add(temp);
            }

            CurrentWaypointIndex = 0;

            return m_WayPoints;
        }

        public void LoopOn()
        {
            m_bLooped = true;
        }

        public void LoopOff()
        {
            m_bLooped = false;
        }

        public void Set(List<Vector2> newPath)
        {
            m_WayPoints = newPath;
            CurrentWaypointIndex = 0;
        }

        public void Set(Path path)
        {
            m_WayPoints = path.GetPath();
            CurrentWaypointIndex = 0;
        }


        private void Clear()
        {
            m_WayPoints.Clear();
        }

        public List<Vector2> GetPath()
        {
            return m_WayPoints;
        }

        public void SetNextWaypoint()
        {
            if (m_WayPoints.Count == 0)
            {
                throw new Exception("Original code assertion: There must be waypoints");
            }

            if (++CurrentWaypointIndex == m_WayPoints.Count)
            {
                if (m_bLooped)
                {
                    CurrentWaypointIndex = 0;
                }
                else
                {
                    CurrentWaypointIndex--;
                }
            }
        }

        public bool Finished()
        {
            return CurrentWaypointIndex == m_WayPoints.Count - 1;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        ~Path()
        {
            Dispose(false);
        }

        #endregion
    }
}