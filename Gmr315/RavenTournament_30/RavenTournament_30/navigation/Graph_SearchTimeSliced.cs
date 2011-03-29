using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.navigation
{
    public abstract class Graph_SearchTimeSliced<T> : IDisposable
    {
        
        public void Dispose()
        {
        }

        public enum SearchType{AStar, Dijkstra};


private
  SearchType m_SearchType;




public  Graph_SearchTimeSliced(SearchType type)
{

    m_SearchType = type;
}

  //When called, this method runs the algorithm through one search cycle. The
  //method returns an enumerated value (target_found, target_not_found,
  //search_incomplete) indicating the status of the search
        public abstract int CycleOnce();

  //returns the vector of edges that the algorithm has examined
        public abstract List<T> GetSPT();


  //returns the total cost to the target
        public abstract float GetCostToTarget();

  //returns a list of node indexes that comprise the shortest path
  //from the source to the target
  public abstract  List<int>                GetPathToTarget();

  //returns the path as a list of PathEdges
  public abstract  List<PathEdge>           GetPathAsPathEdges();

  public SearchType                            GetSearchType(){return m_SearchType;}
    }
}
