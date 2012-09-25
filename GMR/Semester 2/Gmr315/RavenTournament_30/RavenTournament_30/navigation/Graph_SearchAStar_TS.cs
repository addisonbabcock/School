using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Graph;
using Common.Misc;
using Edge = Common.Graph.NavGraphEdge;
using Node = Common.Graph.NavGraphNode;
namespace Raven.navigation
{
    public class Graph_SearchAStar_TS : Graph_SearchTimeSliced<Edge>, IDisposable
    {

        //Function delegate defining the heuristic policy 
        public delegate float CalculateHeuristic(SparseGraph graph, int nd1, int nd2);

        private CalculateHeuristic funcPointer;


        private SparseGraph m_Graph;

        //indexed into my node. Contains the 'real' accumulative cost to that node
        private List<float> m_GCosts;

        //indexed into by node. Contains the cost from adding m_GCosts[n] to
        //the heuristic cost from n to the target node. This is the vector the
        //iPQ indexes into.
        private List<float> m_FCosts;

        private List<Edge> m_ShortestPathTree;
        private List<Edge> m_SearchFrontier;

        private int m_iSource;
        private int m_iTarget;

        //create an indexed priority queue of nodes. The nodes with the
        //lowest overall F cost (G+H) are positioned at the front.
        IndexedPriorityQLow m_pPQ;




        public Graph_SearchAStar_TS(SparseGraph graph,
                              int source,
                              int target, CalculateHeuristic heuristic)
            : base((int)SearchType.AStar)
        {
            funcPointer = heuristic;
            m_Graph = graph;
            int numNodes = m_Graph.NumNodes();

            m_ShortestPathTree = new List<Edge>(numNodes);
            m_SearchFrontier = new List<Edge>(numNodes);
            m_GCosts = new List<float>(numNodes);
            m_FCosts = new List<float>(numNodes);
            m_iSource = source;
            m_iTarget = target;

            for (int i = 0; i < numNodes; i++)
            {
                m_ShortestPathTree.Add(null);
                m_SearchFrontier.Add(null);
                m_GCosts.Add(0.0f);
                m_FCosts.Add(0.0f);
            }

            //create the PQ   
            m_pPQ = new IndexedPriorityQLow(ref m_FCosts, numNodes);

            //put the source node on the queue
            m_pPQ.insert(m_iSource);
        }

        public void Dispose() { m_pPQ.Dispose(); }


        //When called, this method pops the next node off the PQ and examines all
        //its edges. The method returns an enumerated value (target_found,
        //target_not_found, search_incomplete) indicating the status of the search
        public override int CycleOnce()
        {
            //if the PQ is empty the target has not been found
            if (m_pPQ.empty())
            {
                return (int)TimeSliceResult.target_not_found;
            }

            //get lowest cost node from the queue
            int NextClosestNode = m_pPQ.Pop();

            //put the node on the SPT
            m_ShortestPathTree[NextClosestNode] = m_SearchFrontier[NextClosestNode];

            //if the target has been found exit
            if (NextClosestNode == m_iTarget)
            {
                return (int)TimeSliceResult.target_found;
            }

            SparseGraph.EdgeIterator EdgeItr = new SparseGraph.EdgeIterator(m_Graph, NextClosestNode);

            //now to test all the edges attached to this node
            while(EdgeItr.MoveNext())
            {
                //calculate the heuristic cost from this node to the target (H)                       
                float HCost = funcPointer(m_Graph, m_iTarget, EdgeItr.Current.To());

                //calculate the 'real' cost to this node from the source (G)
                float GCost = m_GCosts[NextClosestNode] + EdgeItr.Current.Cost();

                //if the node has not been added to the frontier, add it and update
                //the G and F costs
                if (NavGraphEdge.IsNull(m_SearchFrontier[EdgeItr.Current.To()]))
                {
                    m_FCosts[EdgeItr.Current.To()] = GCost + HCost;
                    m_GCosts[EdgeItr.Current.To()] = GCost;

                    m_pPQ.insert(EdgeItr.Current.To());

                    m_SearchFrontier[EdgeItr.Current.To()] = EdgeItr.Current;
                }

                //if this node is already on the frontier but the cost to get here
                //is cheaper than has been found previously, update the node
                //costs and frontier accordingly.
                else if ((GCost < m_GCosts[EdgeItr.Current.To()]) && NavGraphEdge.IsNull(m_ShortestPathTree[EdgeItr.Current.To()]))
                {
                    m_FCosts[EdgeItr.Current.To()] = GCost + HCost;
                    m_GCosts[EdgeItr.Current.To()] = GCost;

                    m_pPQ.ChangePriority(EdgeItr.Current.To());

                    m_SearchFrontier[EdgeItr.Current.To()] = EdgeItr.Current;
                }
            }

            //there are still nodes to explore
            return (int)TimeSliceResult.search_incomplete;
        }

        //returns the vector of edges that the algorithm has examined
        public override List<Edge> GetSPT()
        {
            return m_ShortestPathTree;
        }

        //returns a vector of node indexes that comprise the shortest path
        //from the source to the target
        public override List<int> GetPathToTarget()
        {
            List<int> path = new List<int>();

            //just return an empty path if no target or no path found
            if (m_iTarget < 0) return path;

            int nd = m_iTarget;

            path.Add(nd);

            while ((nd != m_iSource) && !(NavGraphEdge.IsNull(m_ShortestPathTree[nd])))
            {
                nd = m_ShortestPathTree[nd].From();

                path.Insert(0, nd);
            }

            return path;
        }

        //returns the path as a list of PathEdges
        public override List<PathEdge> GetPathAsPathEdges()
        {
            List<PathEdge> path = new List<PathEdge>();

            //just return an empty path if no target or no path found
            if (m_iTarget < 0) return path;

            int nd = m_iTarget;

            while ((nd != m_iSource) && !(NavGraphEdge.IsNull(m_ShortestPathTree[nd])))
            {
                path.Insert(0, new PathEdge(m_Graph.GetNode(m_ShortestPathTree[nd].From()).Pos(),
                                         m_Graph.GetNode(m_ShortestPathTree[nd].To()).Pos(),
                                         m_ShortestPathTree[nd].Flags(),
                                         m_ShortestPathTree[nd].IDofIntersectingEntity()));

                nd = m_ShortestPathTree[nd].From();
            }

            return path;
        }

        //returns the total cost to the target
        public override float GetCostToTarget() { return m_GCosts[m_iTarget]; }
    }
}
