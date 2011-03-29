using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common._2D;
using Common.Game;
using Common.Graph;
using Common.Misc;
using Common.Script;
using Common.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;
using Raven.triggers;
using GraphNode = Common.Graph.NavGraphNode;
using NavGraph = Common.Graph.SparseGraph;
using TriggerType =  Common.Triggers.Trigger<Raven.AbstractBot>;
using TriggerSystem =  Common.Triggers.TriggerSystem<Raven.AbstractBot>;

namespace Raven
{
    public class Raven_Map : IDisposable
    {
        private static Raven_Scriptor script = Raven_Scriptor.Instance();
        private static EntityManager EntityMgr = EntityManager.Instance();
        /// <summary>
        /// the walls that comprise the current map's architecture. 
        /// </summary>   public       
        private List<Wall2D> m_Walls;

        /// <summary>
        /// trigger are objects that define a region of space. When a raven bot
        /// enters that area, it 'triggers' an event. That event may be anything
        /// from increasing a bot's health to opening a door or requesting a lift.
        /// </summary>
        private TriggerSystem m_TriggerSystem;

        /// <summary>
        ///   this holds a number of spawn positions. When a bot is instantiated
        /// it will appear at a randomly selected point chosen from this vector
        /// </summary>
        private List<Vector2> m_SpawnPoints;

        /// <summary>
        /// a map may contain a number of sliding doors.
        /// </summary>
        private List<Raven_Door> m_Doors;

        //this map's accompanying navigation graph
        private NavGraph m_pNavGraph;

        //the graph nodes will be partitioned enabling fast lookup
        private CellSpacePartition m_pSpacePartition;

        //the size of the search radius the cellspace partition uses when looking for 
        //neighbors 
        private float m_dCellSpaceNeighborhoodRange;

        private int m_iSizeX;
        private int m_iSizeY;

        private void PartitionNavGraph()
        {
            if (m_pSpacePartition != null)
            {
                m_pSpacePartition.Dispose();
            }

            m_pSpacePartition = new CellSpacePartition(m_iSizeX,
                                                                  m_iSizeY,
                                                                  script.GetInt("NumCellsX"),
                                                                  script.GetInt("NumCellsY"),
                                                                  m_pNavGraph.NumNodes());
            SparseGraph.NodeIterator nodeIterator = new SparseGraph.NodeIterator(m_pNavGraph);
            while(nodeIterator.MoveNext())
            {
                m_pSpacePartition.AddEntity(nodeIterator.Current);
            }
        }

        //this will hold a pre-calculated lookup table of the cost to travel from
        //one node to any other.
        private List<List<float>> m_PathCosts;


        //stream constructors for loading from a file
        private void AddWall(string[] tokens)
        {

            m_Walls.Add(new Wall2D(tokens));

        }


        /// <summary>
        ///adds a wall and returns a pointer to that wall. (this method can be
        ///used by objects such as doors to add walls to the environment)
        /// </summary>
        /// <param name="from">Left Side of Door?</param>
        /// <param name="to">Right Side of Door?</param>
        /// <returns>The wall</returns>
        public Wall2D AddWall(Vector2 from, Vector2 to)
        {
            Wall2D w = new Wall2D(from, to);

            m_Walls.Add(w);

            return w;
        }

        private void AddSpawnPoint(string[] tokens)
        {
            float x = float.Parse(tokens[3]);
            float y = float.Parse(tokens[4]);


            m_SpawnPoints.Add(new Vector2(x, y));
        }

        private void AddHealth_Giver(string[] tokens)
        {
            Trigger_HealthGiver hg = new Trigger_HealthGiver(tokens);

            m_TriggerSystem.Register(hg);

            //let the corresponding navgraph node point to this object
            GraphNode node = m_pNavGraph.GetNode(hg.GraphNodeIndex());

            node.SetExtraInfo(hg);

            //register the entity 
            EntityMgr.RegisterEntity(hg);
        }
        private void AddWeapon_Giver(int type_of_weapon, string[] tokens)
        {
            Trigger_WeaponGiver wg = new Trigger_WeaponGiver(tokens);

            wg.SetEntityType(type_of_weapon);

            //add it to the appropriate vectors
            m_TriggerSystem.Register(wg);

            //let the corresponding navgraph node point to this object
            GraphNode node = m_pNavGraph.GetNode(wg.GraphNodeIndex());

            node.SetExtraInfo(wg);

            //register the entity 
            EntityMgr.RegisterEntity(wg);
        }
        private void AddDoor(string[] tokens)
        {
            Raven_Map map = this;
            Raven_Door pDoor = new Raven_Door(ref map, tokens);

            m_Doors.Add(pDoor);

            //register the entity 
            EntityMgr.RegisterEntity(pDoor);
        }

        private void AddDoorTrigger(string[] tokens){
  Trigger_OnButtonSendMsg<AbstractBot> tr = new Trigger_OnButtonSendMsg<AbstractBot>(tokens);

  m_TriggerSystem.Register(tr);

  //register the entity 
  EntityMgr.RegisterEntity(tr);
  
}

        private void Clear()
        {
            if (m_TriggerSystem != null)
            {
                //delete the triggers
                m_TriggerSystem.Clear();
            }
            if (m_Doors != null)
            {
                //delete the doors
                foreach (Raven_Door curDoor in m_Doors)
                {
                    curDoor.Dispose();
                }

                m_Doors.Clear();
            }
            if (m_Walls != null)
            {
                foreach (Wall2D curWall in m_Walls)
                {
                    curWall.Dispose();
                }
                m_Walls.Clear();
            }

            if (m_SpawnPoints != null)
            {
                m_SpawnPoints.Clear();
            }
            if (m_pNavGraph != null)
            {
                //delete the navgraph
                m_pNavGraph.Dispose();
            }
            if (m_pSpacePartition != null)
            {
                //delete the partioning info
                m_pSpacePartition.Dispose();
            }
        }



        public Raven_Map()
        {
            m_pNavGraph= null;
            m_pSpacePartition=null;
            m_iSizeY = 0;
            m_iSizeX = 0;
            m_dCellSpaceNeighborhoodRange = 0;
        }

        public void Dispose()
        {
            Clear();
            m_TriggerSystem = null;
            m_Doors = null;
            m_Walls = null;
            m_SpawnPoints = null;
            m_pNavGraph = null;
            m_pSpacePartition = null;
        }


        private static Raven_UserOptions UserOptions = Raven_UserOptions.Instance();
        public void Render(PrimitiveBatch primitiveBatch, SpriteBatch batch, SpriteFont font)
        {


            //render the navgraph
             if (UserOptions.m_bShowGraph)
            {
                DrawGraph(primitiveBatch, batch, font, m_pNavGraph, Color.Gray, UserOptions.m_bShowNodeIndices);
                //GraphHelper_DrawUsingGDI<NavGraph>(*m_pNavGraph, Cgdi::grey, UserOptions.m_bShowNodeIndices);
            }

            //render any doors
            foreach (Raven_Door curDoor in m_Doors)
            {
                curDoor.Render(primitiveBatch);
            }


            //render all the triggers
            m_TriggerSystem.Render(primitiveBatch);

            //render all the walls
            foreach (Wall2D curWall in m_Walls)
            {
                curWall.Draw(primitiveBatch);
            }


            foreach (Vector2 curSp in m_SpawnPoints)
            {
                Drawing.DrawCircle(primitiveBatch, curSp, 7, Color.Gray);

            }
           // m_pSpacePartition.RenderCells(primitiveBatch);
        }

        private void DrawGraph(PrimitiveBatch primitiveBatch, SpriteBatch batch, SpriteFont font, SparseGraph graph, Color gray, bool DrawNodeIDs)
        {
            //just return if the graph has no nodes
            if (graph.NumNodes() == 0) return;

            SparseGraph.NodeIterator nodeItr = new SparseGraph.NodeIterator(graph);
            while (nodeItr.MoveNext())
            {
                Drawing.DrawCircle(primitiveBatch, nodeItr.Current.Pos(), 2, Color.Gray);



                if (DrawNodeIDs)
                {
                    Color color = new Color(200, 200, 200);
                    Vector2 position = nodeItr.Current.Pos();
                    position.X += 5;
                    position.Y -= 5;
                    batch.DrawString(font, nodeItr.Current.Index().ToString(), position, color);
                }
                SparseGraph.EdgeIterator edgeItr = new SparseGraph.EdgeIterator(graph, nodeItr.Current.Index());
                while (edgeItr.MoveNext())
                {
                    Drawing.DrawLine(primitiveBatch, nodeItr.Current.Pos(), graph.GetNode(edgeItr.Current.To()).Pos(),
                                     Color.Gray);
                }
            }
        }

        //loads an environment from a file
        public bool LoadMap(string FileName)
        {
            StreamReader reader = File.OpenText(FileName);
            Clear();

            BaseGameEntity.ResetNextValidID();

            //first of all read and create the navgraph. This must be done before
            //the entities are read from the map file because many of the entities
            //will be linked to a graph node (the graph node will own a pointer
            //to an instance of the entity)
            m_pNavGraph = new NavGraph(false);

            m_pNavGraph.Load(reader);
            Debug.WriteLine("NavGraph for " + FileName + " loaded okay");

            //determine the average distance between graph nodes so that we can
            //partition them efficiently
            m_dCellSpaceNeighborhoodRange =
                HandyGraphFunctions.CalculateAverageGraphEdgeLength(m_pNavGraph) + 1;
            Debug.WriteLine("Average edge length is " +
                            HandyGraphFunctions.CalculateAverageGraphEdgeLength(m_pNavGraph));
            Debug.WriteLine("Neighborhood range set to " + m_dCellSpaceNeighborhoodRange);

            //load in the map size and adjust the client window accordingly
            string line = reader.ReadLine();
            string[] tokens = line.Split(' ');
            m_iSizeX = int.Parse(tokens[0]);
            m_iSizeY = int.Parse(tokens[1]);




            Debug.WriteLine("Partitioning navgraph nodes...");

            //partition the graph nodes
            PartitionNavGraph();


            //get the handle to the game window and resize the client area to accommodate
            //the map
            const int ExtraHeightRqdToDisplayInfo = 50;
            Game1.graphics.PreferredBackBufferHeight = m_iSizeY + ExtraHeightRqdToDisplayInfo;
            Game1.graphics.PreferredBackBufferWidth = m_iSizeX;
            Game1.graphics.ApplyChanges();
            Debug.WriteLine("Loading map...");


            m_Walls = new List<Wall2D>();
            m_Doors = new List<Raven_Door>();
            m_SpawnPoints = new List<Vector2>();
            m_TriggerSystem = new TriggerSystem<AbstractBot>();
            //now create the environment entities
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                tokens = line.Split(' ');

                //get type of next map object
                int EntityType = int.Parse(tokens[0]);

                Debug.WriteLine("Creating a " + Raven_ObjectEnumerations.GetNameOfType(EntityType) + "");
                //create the object
                switch (EntityType)
                {
                    case (int)Raven_Objects.type_wall:

                        AddWall(tokens);
                        break;

                    case (int)Raven_Objects.type_sliding_door:

                        AddDoor(tokens);
                        break;

                    case (int)Raven_Objects.type_door_trigger:

                        AddDoorTrigger(tokens);
                        break;

                    case (int)Raven_Objects.type_spawn_point:

                        AddSpawnPoint(tokens);
                        break;

                    case (int)Raven_Objects.type_health:

                        AddHealth_Giver(tokens);
                        break;

                    case (int)Raven_Objects.type_shotgun:

                        AddWeapon_Giver((int) Raven_Objects.type_shotgun, tokens);
                        break;

                    case (int)Raven_Objects.type_rail_gun:

                        AddWeapon_Giver((int)Raven_Objects.type_rail_gun, tokens);
                        break;

                    case (int)Raven_Objects.type_rocket_launcher:

                        AddWeapon_Giver((int)Raven_Objects.type_rocket_launcher, tokens);
                        break;

                    default:

                        throw new Exception("<Map::Load>: Attempting to load undefined object");

                } //end switch
            }

            Debug.WriteLine(FileName + " loaded okay");
            //calculate the cost lookup table
            m_PathCosts = HandyGraphFunctions.CreateAllPairsCostsTable(m_pNavGraph);

            return true;
        }




        public void AddSoundTrigger(AbstractBot pSoundSource, float range)
        {
            m_TriggerSystem.Register(new Trigger_SoundNotify(pSoundSource, range));
        }

        public float CalculateCostToTravelBetweenNodes(int nd1, int nd2)
        {
            if(nd1 < 0 || nd1 >= m_pNavGraph.NumNodes() || nd2 < 0 || nd2 >= m_pNavGraph.NumNodes())
            {
                throw new ArgumentException("Invalid Index");
            }
            return m_PathCosts[nd1][nd2];
        }

        //returns the position of a graph node selected at random
        public Vector2 GetRandomNodeLocation()
        {
            int RandIndex = Utils.RandInt(0, m_pNavGraph.NumActiveNodes() - 1);
            return m_pNavGraph.GetNode(RandIndex).Pos();
        }




        public void UpdateTriggerSystem(ref List<AbstractBot> bots)
        {
            m_TriggerSystem.Update(bots);
        }

        public List<Trigger<AbstractBot>> GetTriggers() { return m_TriggerSystem.GetTriggers(); }
        public List<Wall2D> GetWalls() { return m_Walls; }
        public NavGraph                          GetNavGraph()  {return  m_pNavGraph;}
        public List<Raven_Door> GetDoors() { return m_Doors; }
        public List<Vector2> GetSpawnPoints() { return m_SpawnPoints; }
        public CellSpacePartition                   GetCellSpace(){return m_pSpacePartition;}
        public Vector2 GetRandomSpawnPoint() { return m_SpawnPoints[Utils.RandInt(0, m_SpawnPoints.Count() - 1)]; }
        public int GetSizeX() { return m_iSizeX; }
        public int GetSizeY() { return m_iSizeY; }
        public int GetMaxDimension() { return Math.Max(m_iSizeX, m_iSizeY); }
        public float GetCellSpaceNeighborhoodRange() { return m_dCellSpaceNeighborhoodRange; }

    }
}

