using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Graph
{
    public class NavGraphEdge : GraphEdge, IGraphEdge
    {
        public enum GraphEdgeFlags
        {
            normal = 0,
            swim = 1 << 0,
            crawl = 1 << 1,
            creep = 1 << 3,
            jump = 1 << 3,
            fly = 1 << 4,
            grapple = 1 << 5,
            goes_through_door = 1 << 6
        }

        public static bool operator ==(NavGraphEdge lhs, NavGraphEdge rhs)
        {
            return (lhs.m_iFrom == rhs.m_iFrom &&
                   lhs.m_iTo == rhs.m_iTo &&
                   lhs.m_dCost == rhs.m_dCost);
        }

        public static bool operator !=(NavGraphEdge lhs, NavGraphEdge rhs)
        {
            return !(lhs == rhs);
        }

        public static bool IsNull(NavGraphEdge o)
        {
            if ((object)o == null) return true;
            return false;
        }


        protected int m_iFlags;

        //if this edge intersects with an object (such as a door or lift), then
        //this is that object's ID. 
        protected int m_iIDofIntersectingEntity;


        public NavGraphEdge() : base()
        {
            SetFlags(0);
            SetIDofIntersectingEntity(-1);
        }
        public NavGraphEdge(int from, int to, float cost)
            : this(from, to, cost, 0, -1)
        {
        }

        public NavGraphEdge(int from,
               int to,
               float cost,
               int flags,
               int id)
            : base(from, to, cost)
        {
            SetFlags(flags);
            SetIDofIntersectingEntity(id);

        }


        //stream constructor
        public NavGraphEdge(StreamReader stream)
        {
            Load(stream);

        }

        public void Load(StreamReader stream)
        {
            string line = stream.ReadLine();
            string[] tokens = line.Split(' ');
            SetFrom(int.Parse(tokens[1]));
            SetTo(int.Parse(tokens[3]));
            SetCost(float.Parse(tokens[5]));
            SetFlags(int.Parse(tokens[7]));
            SetIDofIntersectingEntity(int.Parse(tokens[9]));
        }

        public int Flags() { return m_iFlags; }
        public void SetFlags(int flags) { m_iFlags = flags; }

        public int IDofIntersectingEntity() { return m_iIDofIntersectingEntity; }
        public void SetIDofIntersectingEntity(int id) { m_iIDofIntersectingEntity = id; }

        public void Write(StreamWriter stream)
        {
            stream.WriteLine("m_iFrom: " + From() + " m_iTo: " + To() + " m_dCost: " + Cost()
                + " m_iFlags: " + Flags()
                             + " ID: " + IDofIntersectingEntity());


        }
    }
}
