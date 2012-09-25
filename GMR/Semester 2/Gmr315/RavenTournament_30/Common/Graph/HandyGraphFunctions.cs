using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
    public static class HandyGraphFunctions
    {
        public static float CalculateAverageGraphEdgeLength(SparseGraph graph)
        {
            float TotalLength = 0;
            int NumEdgesCounted = 0;
            SparseGraph.NodeIterator nodeIterator = new SparseGraph.NodeIterator(graph);
            while(nodeIterator.MoveNext())
            {
                SparseGraph.EdgeIterator edgeIterator = new SparseGraph.EdgeIterator(graph, nodeIterator.Current.Index());
                while(edgeIterator.MoveNext())
                {
                    ++NumEdgesCounted;
                    TotalLength += Vector2.Distance(graph.GetNode(edgeIterator.Current.From()).Pos(), graph.GetNode(edgeIterator.Current.To()).Pos());
                }
            }
            return TotalLength / (float)NumEdgesCounted;
        }




        
        public static List<List<float>> CreateAllPairsCostsTable(SparseGraph graph)
        {
            //create a two dimensional vector
            List<List<float>> PathCosts = new List<List<float>>();

            for (int source = 0; source < graph.NumNodes(); ++source)
            {
                List<float> sourceToTarget = new List<float>();
                PathCosts.Add(sourceToTarget);

                //do the search
                Graph_SearchDijkstra search = new Graph_SearchDijkstra(graph, source);

                //iterate through every node in the graph and grab the cost to travel to
                //that node
                for (int target = 0; target < graph.NumNodes(); ++target)
                {
                    if (source != target)
                    {
                        sourceToTarget.Add(search.GetCostToNode(target));
                        
                    }else
                    {
                        sourceToTarget.Add(0);
                    }
                }//next target node

            }//next source node

            return PathCosts;
        }
    }
}
