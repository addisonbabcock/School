using System.IO;

namespace Common.Graph
{
    public class GraphEdge
    {
        //An edge connects two nodes. Valid node indices are always positive.
        protected float m_dCost;
        protected int m_iFrom;
        protected int m_iTo;

        //the cost of traversing the edge


        //ctors
        public GraphEdge(int from, int to, float cost)
        {
           SetCost(cost);
            SetFrom(from);
            SetTo(to);
        }

        public GraphEdge(int from, int to) : this(from, to, 1.0f)
        {
        }

        public GraphEdge() : this((int) SparseGraph.invalid_node_index, (int) SparseGraph.invalid_node_index, 1.0f)
        {
        }

        //stream constructor
        public GraphEdge(StreamReader stream)
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
        }

        public int From()
        {
            return m_iFrom;
        }

        public void SetFrom(int NewIndex)
        {
            m_iFrom = NewIndex;
        }

        public int To()
        {
            return m_iTo;
        }


        public void SetTo(int NewIndex)
        {
            m_iTo = NewIndex;
        }

        public float Cost()
        {
            return m_dCost;
        }

        public void SetCost(float NewCost)
        {
            m_dCost = NewCost;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var rhs = obj as GraphEdge;
            if (rhs == null)
            {
                return false;
            }
            return rhs.From() == From() && rhs.To() == To() && rhs.Cost() == Cost();
        }

        public override int GetHashCode()
        {
            return From().GetHashCode() ^ To().GetHashCode() ^ Cost().GetHashCode();
        }

        //public static bool operator ==(GraphEdge lhs, GraphEdge rhs)
     //   {
     //       return lhs.Equals(rhs);
     //   }

    //    public static bool operator !=(GraphEdge lhs, GraphEdge rhs)
      //  {
      //      return !(lhs == rhs);
     //   }


        //for reading and writing to streams.
        public void Write(StreamWriter stream)
        {
            stream.WriteLine("m_iFrom: " + From() + " m_iTo: " + To() + " m_dCost: " + Cost());
        }
    }
}