using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Game;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
    
        /// <summary>
        /// Graph node for use in creating a navigation graph.This node contains
        /// the position of the node and a pointer to a BaseGameEntity... useful
        /// if you want your nodes to represent health packs, gold mines and the like
        /// </summary>
    public class NavGraphNode 
    {
        //every node has an index. A valid index is >= 0
        protected int m_iIndex;

        //the node's position
        protected Vector2 m_vPosition;

        //often you will require a navgraph node to contain additional information.
        //For example a node might represent a pickup such as armor in which
        //case m_ExtraInfo could be an enumerated value denoting the pickup type,
        //thereby enabling a search algorithm to search a graph for specific items.
        //Going one step further, m_ExtraInfo could be a pointer to the instance of
        //the item type the node is twinned with. This would allow a search algorithm
        //to test the status of the pickup during the search. 
        object m_ExtraInfo;

        //ctors
        public NavGraphNode()
        {
            SetIndex(SparseGraph.invalid_node_index);
            m_ExtraInfo = null;
        }

        public NavGraphNode(int idx,
                     Vector2 pos)
        {
            SetIndex(idx);
            SetPos(pos);
            m_ExtraInfo = null;
        }
        public int Index()
        {
            return m_iIndex;
        }
        public void SetIndex(int NewIndex)
        {
            m_iIndex = NewIndex;
        }
        //stream constructor
        public NavGraphNode(StreamReader reader)
        {
            Load(reader);

        }

        public void Load(StreamReader reader)
        {
            m_ExtraInfo = null;
            string line = reader.ReadLine();
            string[] tokens = line.Split(' ');
            m_iIndex = int.Parse(tokens[1]);
            SetPos(new Vector2(float.Parse(tokens[3]), float.Parse(tokens[5])));
        }


        public Vector2 Pos() { return m_vPosition; }
        public void SetPos(Vector2 NewPosition)
        {
            m_vPosition = NewPosition;
            if (m_vPosition.X < 0 || m_vPosition.Y < 0)
            {
                throw new Exception();
            }
        }

        public object ExtraInfo() { return m_ExtraInfo; }
        public void SetExtraInfo(object info) { m_ExtraInfo = info; }

        //for reading and writing to streams.
        public void Write(StreamWriter stream)
        {
            stream.WriteLine("Index: " + m_iIndex + " PosX: " + Pos().X + " PosY: " + Pos().Y);
        }
    }
}
