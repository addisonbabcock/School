using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Graph;
using Common.Messaging;
using Common.Misc;
using Common.Triggers;
using Microsoft.Xna.Framework;
using Raven.lua;
using NodeType = Common.Graph.NavGraphNode;
using EdgeType = Common.Graph.NavGraphEdge;
using Path = System.Collections.Generic.List<Raven.navigation.PathEdge>;
using NavGraph = Common.Graph.SparseGraph;
using t_con = Raven.navigation.FindActiveTrigger;
using DijSearch = Raven.navigation.Graph_SearchDijkstras_TS;
using AStar =   Raven.navigation.Graph_SearchAStar_TS;

namespace Raven.navigation
{
    public class Raven_PathPlanner : IDisposable
    {
        protected static Raven_Scriptor script = Raven_Scriptor.Instance();
        protected static MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        public void Dispose()
        {

            GetReadyForNewSearch();

        }

        private const int no_closest_node_found = -1;




        //A pointer to the owner of this class
        private AbstractBot m_pOwner;

        //a reference to the navgraph
        private NavGraph m_NavGraph;

        //a pointer to an instance of the current graph search algorithm.
        private Graph_SearchTimeSliced<EdgeType> m_pCurrentSearch;

        //this is the position the bot wishes to plan a path to reach
        private Vector2 m_vDestinationPos;


        //returns the index of the closest visible and unobstructed graph node to
        //the given position
        private int GetClosestNodeToPosition(Vector2 pos)
        {
            float ClosestSoFar = float.MaxValue;
            int ClosestNode = no_closest_node_found;

            //when the cell space is queried this the the range searched for neighboring
            //graph nodes. This value is inversely proportional to the density of a 
            //navigation graph (less dense = bigger values)
            float range = m_pOwner.GetWorld().GetMap().GetCellSpaceNeighborhoodRange();

            //calculate the graph nodes that are neighboring this position
            m_pOwner.GetWorld().GetMap().GetCellSpace().CalculateNeighbors(pos, range);

            //iterate through the neighbors and sum up all the position vectors

            foreach(NodeType pN in m_pOwner.GetWorld().GetMap().GetCellSpace().Neighbors)
            {
                if (m_pOwner.canWalkBetween(pos, pN.Pos()))
                {
                    float dist = Vector2.DistanceSquared(pos, pN.Pos());

                    //keep a record of the closest so far
                    if (dist < ClosestSoFar)
                    {
                        ClosestSoFar = dist;
                        ClosestNode = pN.Index();
                    }
                }
            }

            return ClosestNode;
        }

        //smooths a path by removing extraneous edges. (may not remove all
        //extraneous edges)
        private void SmoothPathEdgesQuick(Path path)
        {

            int path2Index = 1;
            PathEdge e1 = path[0];
            PathEdge e2 = path[path2Index];

            while (e2 != path[path.Count - 1])
            {
                if ((e2.Behavior() == (int)EdgeType.GraphEdgeFlags.normal) &&
                    m_pOwner.canWalkBetween(e1.Source(), e2.Destination()))
                {
                    e1.SetDestination(e2.Destination());
                    path.RemoveAt(path2Index);
                    e2 = path[path2Index];
                }
                else
                {
                    e1 = e2;
                    path2Index++;
                    e2 = path[path2Index];
                }
            }


        }

        //smooths a path by removing extraneous edges. (removes *all* extraneous
        //edges)
        private void SmoothPathEdgesPrecise(Path path)
        {
            int path1Index = 0;
            int path2Index = 0;

            //point e1 to the beginning of the path
            PathEdge e1 = path[path1Index];
            PathEdge e2 = path[path2Index];

            while (e1 != path[path.Count - 1])
            {
                //point e2 to the edge immediately following e1
                path2Index = path1Index;
                path2Index++;
                e2 = path[path2Index];

                //while e2 is not the last edge in the path, step through the edges
                //checking to see if the agent can move without obstruction from the 
                //source node of e1 to the destination node of e2. If the agent can move
                //between those positions then the any edges between e1 and e2 are
                //replaced with a single edge.
                while (e2 != path[path.Count - 1])
                {
                    //check for obstruction, adjust and remove the edges accordingly
                    if ((e2.Behavior() == (int)EdgeType.GraphEdgeFlags.normal) &&
                          m_pOwner.canWalkBetween(e1.Source(), e2.Destination()))
                    {
                        path1Index++;
                        path2Index++;
                        for (int i = path1Index; i < path2Index; i++)
                        {
                            path.RemoveAt(i);
                        }

                        e2 = path[path2Index];
                        path1Index = path2Index;
                        path1Index--;
                        e1 = path[path1Index];
                    }

                    else
                    {
                        path2Index++;
                        e2 = path[path2Index];
                    }
                }

                path1Index++;
                e1 = path[path1Index];
            }
        }

        //called at the commencement of a new search request. It clears up the 
        //appropriate lists and memory in preparation for a new search request
        private void GetReadyForNewSearch()
        {
            //unregister any existing search with the path manager
            m_pOwner.GetWorld().GetPathManager().UnRegister(this);

            //clean up memory used by any existing search
            if (m_pCurrentSearch != null)
            {
                m_pCurrentSearch.Dispose();
                m_pCurrentSearch = null;
            }
        }


        public Raven_PathPlanner(AbstractBot owner)
        {
            m_pOwner = owner;
            m_NavGraph = m_pOwner.GetWorld().GetMap().GetNavGraph();
            m_pCurrentSearch = null;

        }

        //creates an instance of the A* time-sliced search and registers it with
        //the path manager
        public bool RequestPathToItem(int ItemType)
        {
            //clear the waypoint list and delete any active search
            GetReadyForNewSearch();

            //find the closest visible node to the bots position
            int ClosestNodeToBot = GetClosestNodeToPosition(m_pOwner.Pos());

            //remove the destination node from the list and return false if no visible
            //node found. This will occur if the navgraph is badly designed or if the bot
            //has managed to get itself *inside* the geometry (surrounded by walls),
            //or an obstacle
            if (ClosestNodeToBot == no_closest_node_found)
            {
#if SHOW_NAVINFO
    debug_con << "No closest node to bot found!" << "";
#endif

                return false;
            }

            //create an instance of the search algorithm


            m_pCurrentSearch = new DijSearch(m_NavGraph,
                                             ClosestNodeToBot,
                                             ItemType, new t_con());

            //register the search with the path manager
            m_pOwner.GetWorld().GetPathManager().Register(this);

            return true;
        }

        //creates an instance of the Dijkstra's time-sliced search and registers 
        //it with the path manager
        public bool RequestPathToPosition(Vector2 TargetPos)
        {
#if SHOW_NAVINFO
    debug_con << "------------------------------------------------" << "";
#endif
            GetReadyForNewSearch();

            //make a note of the target position.
            m_vDestinationPos = TargetPos;

            //if the target is walkable from the bot's position a path does not need to
            //be calculated, the bot can go straight to the position by ARRIVING at
            //the current waypoint
            if (m_pOwner.canWalkTo(TargetPos))
            {
                return true;
            }

            //find the closest visible node to the bots position
            int ClosestNodeToBot = GetClosestNodeToPosition(m_pOwner.Pos());

            //remove the destination node from the list and return false if no visible
            //node found. This will occur if the navgraph is badly designed or if the bot
            //has managed to get itself *inside* the geometry (surrounded by walls),
            //or an obstacle.
            if (ClosestNodeToBot == no_closest_node_found)
            {
#if SHOW_NAVINFO
    debug_con << "No closest node to bot found!" << "";
#endif

                return false;
            }

#if SHOW_NAVINFO
    debug_con << "Closest node to bot is " << ClosestNodeToBot << "";
#endif

            //find the closest visible node to the target position
            int ClosestNodeToTarget = GetClosestNodeToPosition(TargetPos);

            //return false if there is a problem locating a visible node from the target.
            //This sort of thing occurs much more frequently than the above. For
            //example, if the user clicks inside an area bounded by walls or inside an
            //object.
            if (ClosestNodeToTarget == no_closest_node_found)
            {
#if SHOW_NAVINFO
    debug_con << "No closest node to target (" << ClosestNodeToTarget << ") found!" << "";
#endif

                return false;
            }

#if SHOW_NAVINFO
    debug_con << "Closest node to target is " << ClosestNodeToTarget << "";
#endif

            //create an instance of a the distributed A* search class


            m_pCurrentSearch = new AStar(m_NavGraph,
                                         ClosestNodeToBot,
                                         ClosestNodeToTarget, Heuristic_Euclid.Calculate);

            //and register the search with the path manager
            m_pOwner.GetWorld().GetPathManager().Register(this);

            return true;
        }
        private static Raven_UserOptions UserOptions = Raven_UserOptions.Instance();
        //called by an agent after it has been notified that a search has terminated
        //successfully. The method extracts the path from m_pCurrentSearch, adds
        //additional edges appropriate to the search type and returns it as a list of
        //PathEdges.
        public Path GetPath()
        {
            if (m_pCurrentSearch == null)
            {
                throw new Exception("No current search");
            }

            Path path = m_pCurrentSearch.GetPathAsPathEdges();

            int closest = GetClosestNodeToPosition(m_pOwner.Pos());

            path.Add(new PathEdge(m_pOwner.Pos(),
                                      GetNodePosition(closest),
                                      (int)EdgeType.GraphEdgeFlags.normal));


            //if the bot requested a path to a location then an edge leading to the
            //destination must be added
            if (m_pCurrentSearch.GetSearchType() == (int)Graph_SearchTimeSliced<EdgeType>.SearchType.AStar)
            {
                path.Add(new PathEdge(path[path.Count - 1].Destination(),
                                        m_vDestinationPos,
                                        (int)Common.Graph.NavGraphEdge.GraphEdgeFlags.normal));
            }

            //smooth paths if required
            if (UserOptions.m_bSmoothPathsQuick)
            {
                SmoothPathEdgesQuick(path);
            }

            if (UserOptions.m_bSmoothPathsPrecise)
            {
                SmoothPathEdgesPrecise(path);
            }

            return path;
        }

        //returns the cost to travel from the bot's current position to a specific 
        //graph node. This method makes use of the pre-calculated lookup table
        //created by Raven_Game
        public float GetCostToNode(int NodeIdx)
        {
            //find the closest visible node to the bots position
            int nd = GetClosestNodeToPosition(m_pOwner.Pos());

            //add the cost to this node
            float cost = Vector2.Distance(m_pOwner.Pos(),
                                      m_NavGraph.GetNode(nd).Pos());

            //add the cost to the target node and return
            return cost + m_pOwner.GetWorld().GetMap().CalculateCostToTravelBetweenNodes(nd, NodeIdx);
        }

        //returns the cost to the closest instance of the GiverType. This method
        //also makes use of the pre-calculated lookup table. Returns -1 if no active
        //trigger found
        public float GetCostToClosestItem(int GiverType)
        {
            //find the closest visible node to the bots position
            int nd = GetClosestNodeToPosition(m_pOwner.Pos());

            //if no closest node found return failure
            if (nd == SparseGraph.invalid_node_index) return -1;

            float ClosestSoFar = float.MaxValue;

            //iterate through all the triggers to find the closest *active* trigger of 
            //type GiverType
            List<Common.Triggers.Trigger<Raven.AbstractBot>> triggers = m_pOwner.GetWorld().GetMap().GetTriggers();

            foreach (Trigger<AbstractBot> it in triggers)
            {
                if ((it.EntityType() == GiverType) && it.isActive())
                {
                    float cost =
                        m_pOwner.GetWorld().GetMap().CalculateCostToTravelBetweenNodes(nd,
                                                                                       it.GraphNodeIndex());

                    if (cost < ClosestSoFar)
                    {
                        ClosestSoFar = cost;
                    }
                }
            }

            //return a negative value if no active trigger of the type found
            if (Utils.isEqual(ClosestSoFar, float.MaxValue))
            {
                return -1;
            }

            return ClosestSoFar;
        }


        //the path manager calls this to iterate once though the search cycle
        //of the currently assigned search algorithm. When a search is terminated
        //the method messages the owner with either the msg_NoPathAvailable or
        //msg_PathReady messages
        public int CycleOnce()
        {
            if (m_pCurrentSearch == null)
            {
                throw new Exception("No search object instantied");
            }


            int result = m_pCurrentSearch.CycleOnce();

            //let the bot know of the failure to find a path
            if (result == (int)TimeSliceResult.target_not_found)
            {
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                        MessageDispatcher.SENDER_ID_IRRELEVANT,
                                        m_pOwner.ID(),
                                        (int)message_type.Msg_NoPathAvailable,
                                        MessageDispatcher.NO_ADDITIONAL_INFO);

            }

            //let the bot know a path has been found
            else if (result == (int)TimeSliceResult.target_found)
            {
                //if the search was for an item type then the final node in the path will
                //represent a giver trigger. Consequently, it's worth passing the pointer
                //to the trigger in the extra info field of the message. (The pointer
                //will just be NULL if no trigger)
                Trigger<AbstractBot> pTrigger = (Trigger<AbstractBot>)
                m_NavGraph.GetNode(m_pCurrentSearch.GetPathToTarget()[(m_pCurrentSearch.GetPathToTarget().Count - 1)]).ExtraInfo();

                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                        MessageDispatcher.SENDER_ID_IRRELEVANT,
                                        m_pOwner.ID(),
                                        (int)message_type.Msg_PathReady,
                                        pTrigger);
            }

            return result;
        }

        public Vector2 GetDestination() { return m_vDestinationPos; }
        public void SetDestination(Vector2 NewPos) { m_vDestinationPos = NewPos; }

        //used to retrieve the position of a graph node from its index. (takes
        //into account the enumerations 'non_graph_source_node' and 
        //'non_graph_target_node'
        public Vector2 GetNodePosition(int idx)
        {
            return m_NavGraph.GetNode(idx).Pos();
        }
    }
}
