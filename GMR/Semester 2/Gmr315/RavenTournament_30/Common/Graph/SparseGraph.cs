using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
public class SparseGraph : IDisposable
    {
        public const int invalid_node_index = -1;

        //the nodes that comprise this graph
        private List<NavGraphNode> m_Nodes;

        //a list of adjacency edge lists. (each node index keys into the
        //list of edges associated with that node)
        private List<List<NavGraphEdge>> m_Edges;

        //is this a directed graph?
        private bool m_bDigraph;

        //the index of the next node to be added
        private int m_iNextNodeIndex;

    public void Dispose()
    {
        Clear();
        m_Nodes = null;
        m_Edges = null;
    }
        public SparseGraph(bool digraph)
        {
            m_iNextNodeIndex = 0;
            m_bDigraph = digraph;

            m_Nodes = new List<NavGraphNode>();
            m_Edges = new List<List<NavGraphEdge>>();
        }

        //retrieves the next free node index
        public int GetNextFreeNodeIndex() { return m_iNextNodeIndex; }

        //returns the number of active + inactive nodes present in the graph
        public int NumNodes() { return m_Nodes.Count; }

        //returns the number of active nodes present in the graph (this method's
        //performance can be improved greatly by caching the value)
        public int NumActiveNodes()
        {
            int count = 0;

            foreach (NavGraphNode curNode in m_Nodes)
            {
                if (curNode.Index() != invalid_node_index) ++count;
            }

            return count;
        }

        //returns the total number of edges present in the graph
        public int NumEdges()
        {
            int tot = 0;

            foreach (List<NavGraphEdge> curEdgeList in m_Edges)
            {
                tot += curEdgeList.Count;
            }

            return tot;
        }

        //returns true if the graph is directed
        public bool isDigraph() { return m_bDigraph; }

        //returns true if the graph contains no nodes
        public bool isEmpty() { return m_Nodes.Count < 0; }

        //clears the graph ready for new node insertions
        public void Clear()
        {
            m_iNextNodeIndex = 0;
            m_Nodes.Clear();
            m_Edges.Clear();
        }

        public void RemoveEdges()
        {
            foreach (List<NavGraphEdge> curEdgeList in m_Edges)
            {
                curEdgeList.Clear();
            }
        }

        #region " Enumerator helper classes "

        public class EdgeIterator : IEnumerator<NavGraphEdge>
        {
            private SparseGraph mGraph;
            private int mNodeIndex;
            private int enumeratorPosition;

            public EdgeIterator(SparseGraph graph, int node)
            {
                mGraph = graph;
                mNodeIndex = node;

                enumeratorPosition = -1;
            }

            public bool MoveNext()
            {
                enumeratorPosition++;
                return (enumeratorPosition < mGraph.m_Edges[mNodeIndex].Count);
            }

            public void Reset()
            {
                enumeratorPosition = -1;
            }

            public NavGraphEdge Current
            {
                get
                {
                    try
                    {
                        return mGraph.m_Edges[mNodeIndex][enumeratorPosition];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            object System.Collections.IEnumerator.Current { get { return Current; } }
            void IDisposable.Dispose() { }
        }

        public class NodeIterator : IEnumerator<NavGraphNode>
        {
            private SparseGraph mGraph;
            private int enumeratorPosition;

            public NodeIterator(SparseGraph graph)
            {
                mGraph = graph;

                enumeratorPosition = -1;
            }

            //if a graph node is removed, it is not removed from the
            //list of nodes (because that would mean changing all the indices of
            //all the nodes that have a higher index).
            public bool MoveNext()
            {
                enumeratorPosition++;

                if (enumeratorPosition >= mGraph.m_Nodes.Count) { return false; }

                while (mGraph.m_Nodes[enumeratorPosition].Index() == invalid_node_index)
                {
                    enumeratorPosition++;

                    if (enumeratorPosition == mGraph.m_Nodes.Count) { break; }
                }

                return (enumeratorPosition < mGraph.m_Nodes.Count);
            }

            public void Reset()
            {
                enumeratorPosition = -1;
            }

            public NavGraphNode Current
            {
                get
                {
                    try
                    {
                        return mGraph.m_Nodes[enumeratorPosition];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            object System.Collections.IEnumerator.Current { get { return Current; } }
            void IDisposable.Dispose() { }
        }

        #endregion

        //  returns true if a node with the given index is present in the graph
        public bool isNodePresent(int nd)
        {
            if ((nd >= (int)m_Nodes.Count || (m_Nodes[nd].Index() == invalid_node_index)))
            {
                return false;
            }
            else return true;
        }


        //  returns true if an edge with the given from/to is present in the graph
        public bool isEdgePresent(int from, int to)
        {
            if (isNodePresent(from) && isNodePresent(to)) // BUG in original:  if (isNodePresent(from) && isNodePresent(from))
            {
                foreach (NavGraphEdge curEdge in m_Edges[from])
                {
                    if (curEdge.To() == to) return true;
                }

                return false;
            }
            else return false;
        }

        //  method for obtaining a reference to a specific node
        public NavGraphNode GetNode(int idx)
        {
            Debug.Assert((idx < (int)m_Nodes.Count) && (idx >= 0), "<SparseGraph::GetNode>: invalid index");
            return m_Nodes[idx];
        }

        // method for obtaining a reference to a specific edge
        public NavGraphEdge GetEdge(int from, int to)
        {
            Debug.Assert((from < m_Nodes.Count) && (from >= 0) && m_Nodes[from].Index() != invalid_node_index, "<SparseGraph::GetEdge>: invalid 'from' index");
            Debug.Assert((to < m_Nodes.Count) && (to >= 0) && m_Nodes[to].Index() != invalid_node_index, "<SparseGraph::GetEdge>: invalid 'to' index");

            foreach (NavGraphEdge curEdge in m_Edges[from])
            {
                if (curEdge.To() == to) return curEdge;
            }

            throw new Exception("<SparseGraph::GetEdge>: edge does not exist");
        }

        //-------------------------------- UniqueEdge ----------------------------
        //
        //  returns true if the edge is not present in the graph. Used when adding
        //  edges to prevent duplication
        //------------------------------------------------------------------------
        public bool UniqueEdge(int from, int to)
        {
            foreach (NavGraphEdge curEdge in m_Edges[from])
            {
                if (curEdge.To() == to)
                {
                    return false;
                }
            }

            return true;
        }

        //-------------------------- AddEdge ------------------------------------------
        //
        //  Use this to add an edge to the graph. The method will ensure that the
        //  edge passed as a parameter is valid before adding it to the graph. If the
        //  graph is a digraph then a similar edge connecting the nodes in the opposite
        //  direction will be automatically added.
        //-----------------------------------------------------------------------------
        public void AddEdge(NavGraphEdge edge)
        {
            //first make sure the from and to nodes exist within the graph
            Debug.Assert((edge.From() < m_iNextNodeIndex) && (edge.To() < m_iNextNodeIndex), "<SparseGraph::AddEdge>: invalid node index");

            //make sure both nodes are active before adding the edge
            if ((m_Nodes[edge.To()].Index() != invalid_node_index) && (m_Nodes[edge.From()].Index() != invalid_node_index))
            {
                //add the edge, first making sure it is unique
                if (UniqueEdge(edge.From(), edge.To()))
                {
                    m_Edges[edge.From()].Add(edge);
                }

                //if the graph is undirected we must add another connection in the opposite direction
                if (!m_bDigraph)
                {
                    //check to make sure the edge is unique before adding
                    if (UniqueEdge(edge.To(), edge.From()))
                    {
                        NavGraphEdge NewEdge = new NavGraphEdge(edge.To(), edge.From(), edge.Cost());

                        m_Edges[edge.To()].Add(NewEdge);
                    }
                }
            }
        }

        public void RemoveEdge(int from, int to)
        {
            Debug.Assert((from < (int)m_Nodes.Count) && (to < (int)m_Nodes.Count), "<SparseGraph::RemoveEdge>:invalid node index");

            if (!m_bDigraph)
            {
                for (int i = m_Edges[to].Count - 1; i >= 0; i--)
                {
                    if (m_Edges[to][i].To() == from)
                    {
                        m_Edges[to].RemoveAt(i);
                        break;
                    }
                }
            }

            for (int i = m_Edges[from].Count - 1; i >= 0; i--)
            {
                if (m_Edges[from][i].To() == to)
                {
                    m_Edges[from].RemoveAt(i);
                    break;
                }
            }
        }

        //-------------------------- AddNode -------------------------------------
        //
        //  Given a node this method first checks to see if the node has been added
        //  previously but is now innactive. If it is, it is reactivated.
        //
        //  If the node has not been added previously, it is checked to make sure its
        //  index matches the next node index before being added to the graph
        //------------------------------------------------------------------------
        public int AddNode(NavGraphNode node)
        {
            if (node.Index() < (int)m_Nodes.Count)
            {
                //make sure the client is not trying to add a node with the same ID as a currently active node
                Debug.Assert(m_Nodes[node.Index()].Index() == invalid_node_index, "<SparseGraph::AddNode>: Attempting to add a node with a duplicate ID");

                m_Nodes[node.Index()] = node;

                return m_iNextNodeIndex;
            }

            else
            {
                //make sure the new node has been indexed correctly
                Debug.Assert(node.Index() == m_iNextNodeIndex, "<SparseGraph::AddNode>:invalid index");

                m_Nodes.Add(node);
                m_Edges.Add(new List<NavGraphEdge>());

                return m_iNextNodeIndex++;
            }
        }

        //  iterates through all the edges in the graph and removes any that point to an invalidated node
        public void CullInvalidEdges()
        {
            foreach (List<NavGraphEdge> curEdgeList in m_Edges)
            {
                for (int i = curEdgeList.Count - 1; i >= 0; i--)
                {
                    if (m_Nodes[curEdgeList[i].To()].Index() == invalid_node_index || m_Nodes[curEdgeList[i].From()].Index() == invalid_node_index)
                    {
                        curEdgeList.RemoveAt(i);
                    }
                }
            }
        }

        //  Removes a node from the graph and removes any links to neighbouring nodes
        public void RemoveNode(int node)
        {
            Debug.Assert(node < (int)m_Nodes.Count, "<SparseGraph::RemoveNode>: invalid node index");

            //set this node's index to invalid_node_index
            m_Nodes[node].SetIndex(invalid_node_index);

            //if the graph is not directed remove all edges leading to this node and then
            //clear the edges leading from the node
            if (!m_bDigraph)
            {
                //visit each neighbour and erase any edges leading to this node
                foreach (NavGraphEdge curEdge in m_Edges[node])
                {
                    for (int i = m_Edges[curEdge.To()].Count - 1; i >= 0; i--)
                    {
                        if (m_Edges[curEdge.To()][i].To() == node)
                        {
                            m_Edges[curEdge.To()].RemoveAt(i);

                            break;
                        }
                    }
                }

                //finally, clear this node's edges
                m_Edges[node].Clear();
            }
            //if a digraph remove the edges the slow way
            else
            {
                CullInvalidEdges();
            }

        }

        //  Sets the cost of a specific edge
        public void SetEdgeCost(int from, int to, float NewCost)
        {
            //make sure the nodes given are valid
            Debug.Assert((from < m_Nodes.Count) && (to < m_Nodes.Count), "<SparseGraph::SetEdgeCost>: invalid index");


            //visit each neighbour and erase any edges leading to this node
            foreach (NavGraphEdge curEdge in m_Edges[from])
            {
                if (curEdge.To() == to)
                {
                    curEdge.SetCost(NewCost);
                    break;
                }
            }
        }
        //methods for loading and saving graphs from an open file stream or from
        //a file name 
        public bool Save(string FileName)
        {
            StreamWriter writer = File.CreateText(FileName);
            return Save(writer);
        }
        public bool Save(StreamWriter stream)
        {
            stream.WriteLine(m_Nodes.Count);
            foreach (NavGraphNode curNode in m_Nodes)
            {
                curNode.Write(stream);
            }
            stream.WriteLine(NumEdges());
            for (int nodeIdx = 0; nodeIdx < m_Nodes.Count; ++nodeIdx)
            {
                foreach (NavGraphEdge curEdge in m_Edges[nodeIdx])
                {
                    curEdge.Write(stream);
                }
            }
            return true;
        }

        public bool Load(string FileName)
        {
            StreamReader reader = File.OpenText(FileName);
            return Load(reader);
        }
        public bool Load(StreamReader stream)
        {
            Clear();


            //get the number of nodes and read them in
            int NumNodes = int.Parse(stream.ReadLine());
            m_Nodes = new List<NavGraphNode>(NumNodes);
            m_Edges = new List<List<NavGraphEdge>>(NumNodes);
            for (int n = 0; n < NumNodes; ++n)
            {
                NavGraphNode NewNode = new NavGraphNode();
                NewNode.Load(stream);

                //when editing graphs it's possible to end up with a situation where some
                //of the nodes have been invalidated (their id's set to invalid_node_index). Therefore
                //when a node of index invalid_node_index is encountered, it must still be added.
                if (NewNode.Index() != invalid_node_index)
                {
                    AddNode(NewNode);
                }
                else
                {
                    m_Nodes.Add(NewNode);

                    //make sure an edgelist is added for each node
                    m_Edges.Add(new List<NavGraphEdge>());

                    ++m_iNextNodeIndex;
                }
            }
            int NumEdges = int.Parse(stream.ReadLine());

            //now add the edges
            for (int e = 0; e < NumEdges; ++e)
            {
                NavGraphEdge NextEdge = new NavGraphEdge();
                NextEdge.Load(stream);
                AddEdge(NextEdge);
            }

            return true;
        }
       

    }
}
