using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Graph;
using Common.Misc;
using Common.Triggers;

using Edge = Common.Graph.NavGraphEdge;
using Node = Common.Graph.NavGraphNode;
namespace Raven.navigation
{
        //-------------------------- Graph_SearchDijkstras_TS -------------------------
//
//  Dijkstra's algorithm class modified to spread a search over multiple
//  update-steps
//-----------------------------------------------------------------------------

    public class Graph_SearchDijkstras_TS: Graph_SearchTimeSliced<Edge>, IDisposable
    {

        public new void Dispose()
        {
            m_pPQ.Dispose();
        }

        private SparseGraph m_Graph;

        //indexed into my node. Contains the accumulative cost to that node
        private List<float> m_CostToThisNode;

        private List<Edge> m_ShortestPathTree;
        private List<Edge> m_SearchFrontier;

        int m_iSource;
        int m_iTarget;

        //create an indexed priority queue of nodes. The nodes with the
        //lowest overall F cost (G+H) are positioned at the front.
        IndexedPriorityQLow m_pPQ;

        private ITerminatingCondition condition;



        public Graph_SearchDijkstras_TS(SparseGraph graph,
                                  int source,
                                  int target,
                                  ITerminatingCondition condition)
            : base(SearchType.Dijkstra)
        {
            this.condition = condition;

            m_Graph = graph;
            int numNodes = m_Graph.NumNodes();
            m_ShortestPathTree = new List<Edge>(numNodes);
            m_SearchFrontier = new List<Edge>(numNodes);
            m_CostToThisNode = new List<float>(numNodes);
            m_iSource = source;
            m_iTarget = target;

            for (int i = 0; i < numNodes; i++)
            {
                m_ShortestPathTree.Add(null);
                m_SearchFrontier.Add(null);
                m_CostToThisNode.Add(0);
            }

            //create the PQ         ,
            m_pPQ = new IndexedPriorityQLow(ref m_CostToThisNode, m_Graph.NumNodes());

            //put the source node on the queue
            m_pPQ.insert(m_iSource);
        }



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

            //move this node from the frontier to the spanning tree
            m_ShortestPathTree[NextClosestNode] = m_SearchFrontier[NextClosestNode];

            //if the target has been found exit
            if (condition.isSatisfied(m_Graph, m_iTarget, NextClosestNode))
            {
                //make a note of the node index that has satisfied the condition. This
                //is so we can work backwards from the index to extract the path from
                //the shortest path tree.
                m_iTarget = NextClosestNode;

                return (int)TimeSliceResult.target_found;
            }

            SparseGraph.EdgeIterator EdgeItr = new SparseGraph.EdgeIterator(m_Graph, NextClosestNode);
            while(EdgeItr.MoveNext())
            {

                //the total cost to the node this edge points to is the cost to the
                //current node plus the cost of the edge connecting them.
                float NewCost = m_CostToThisNode[NextClosestNode] + EdgeItr.Current.Cost();

                //if this edge has never been on the frontier make a note of the cost
                //to get to the node it points to, then add the edge to the frontier
                //and the destination node to the PQ.
                if (NavGraphEdge.IsNull(m_SearchFrontier[EdgeItr.Current.To()]))
                {
                    m_CostToThisNode[EdgeItr.Current.To()] = NewCost;

                    m_pPQ.insert(EdgeItr.Current.To());

                    m_SearchFrontier[EdgeItr.Current.To()] = EdgeItr.Current;
                }

                //else test to see if the cost to reach the destination node via the
                //current node is cheaper than the cheapest cost found so far. If
                //this path is cheaper, we assign the new cost to the destination
                //node, update its entry in the PQ to reflect the change and add the
                //edge to the frontier
                else if ((NewCost < m_CostToThisNode[EdgeItr.Current.To()]) &&
                          NavGraphEdge.IsNull(m_ShortestPathTree[EdgeItr.Current.To()]))
                {
                    m_CostToThisNode[EdgeItr.Current.To()] = NewCost;

                    //because the cost is less than it was previously, the PQ must be
                    //re-sorted to account for this.
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
        public override float GetCostToTarget()
        {
            return m_CostToThisNode[m_iTarget];
        }

    }
}
