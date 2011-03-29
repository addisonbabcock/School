using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Misc
{
    public class CellSpacePartition : IDisposable
    {
        private readonly float m_dCellSizeX;
        private readonly float m_dCellSizeY;
        private readonly float m_dSpaceHeight;
        private readonly float m_dSpaceWidth;

        //the number of cells the space is going to be divided up into
        private readonly int m_iNumCellsX;
        private readonly int m_iNumCellsY;
        private List<Cell> m_Cells;
        private List<NavGraphNode> m_Neighbors;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">width of the environment</param>
        /// <param name="height">height ...</param>
        /// <param name="cellsX">number of cells horizontally</param>
        /// <param name="cellsY">number of cells vertically</param>
        /// <param name="MaxEntitys">maximum number of entities to add</param>
        public CellSpacePartition(float width,
                                  float height,
                                  int cellsX,
                                  int cellsY,
                                  int MaxEntitys)
        {
            m_dSpaceWidth = width;
            m_dSpaceHeight = height;
            m_iNumCellsX = cellsX;
            m_iNumCellsY = cellsY;
            m_Neighbors = new List<NavGraphNode>(MaxEntitys);
            //calculate bounds of each cell
            m_dCellSizeX = width/cellsX;
            m_dCellSizeY = height/cellsY;

            m_Cells = new List<Cell>(m_iNumCellsY*m_iNumCellsX);
            //create the cells
            for (int y = 0; y < m_iNumCellsY; ++y)
            {
                for (int x = 0; x < m_iNumCellsX; ++x)
                {
                    float left = x*m_dCellSizeX;
                    float right = left + m_dCellSizeX;
                    float top = y*m_dCellSizeY;
                    float bot = top + m_dCellSizeY;

                    m_Cells.Add(new Cell(new Vector2(left, top), new Vector2(right, bot)));
                }
            }
        }

        public List<NavGraphNode> Neighbors
        {
            get { return m_Neighbors; }
        }

        #region IDisposable Members

        public void Dispose()
        {
            EmptyCells();
            m_Cells = null;
            m_Neighbors = null;
        }

        #endregion

        private int PositionToIndex(Vector2 pos)
        {
            int idx = (int) (m_iNumCellsX*pos.X/m_dSpaceWidth) +
                      ((int) ((m_iNumCellsY)*pos.Y/m_dSpaceHeight)*m_iNumCellsX);

            //if the entity's position is equal to vector2d(m_dSpaceWidth, m_dSpaceHeight)
            //then the index will overshoot. We need to check for this and adjust
            if (idx > m_Cells.Count() - 1) idx = m_Cells.Count() - 1;

            return idx;
        }

        //adds entities to the class by allocating them to the appropriate cell
        public void AddEntity(NavGraphNode ent)
        {
            System.Diagnostics.Debug.Assert(ent != null);

            int NewIdx = PositionToIndex(ent.Pos());

            m_Cells[NewIdx].Members.Add(ent);
        }

        //update an entity's cell by calling this from your entity's Update method 
        public void UpdateEntity(NavGraphNode ent, Vector2 OldPos)
        {
            System.Diagnostics.Debug.Assert(ent != null);

            //if the index for the old pos and the new pos are not equal then
            //the entity has moved to another cell.
            int OldIdx = PositionToIndex(OldPos);
            int NewIdx = PositionToIndex(ent.Pos());

            if (NewIdx == OldIdx) return;

            //the entity has moved into another cell so delete from current cell
            //and add to new one
            m_Cells[OldIdx].Members.Remove(ent);
            m_Cells[NewIdx].Members.Add(ent);
        }

        //this method calculates all a target's neighbors and stores them in
        //the neighbor vector. After you have called this method use the begin, 
        //next and end methods to iterate through the vector.
        public void CalculateNeighbors(Vector2 TargetPos, float QueryRadius)
        {
            var QueryBox = new InvertedAABBox2D(TargetPos - new Vector2(QueryRadius, QueryRadius),
                                                TargetPos + new Vector2(QueryRadius, QueryRadius));
            m_Neighbors.Clear();

            float queryRadiusSquared = QueryRadius*QueryRadius;

            //iterate through each cell and test to see if its bounding box overlaps
            //with the query box. If it does and it also contains entities then make further proximity tests.

            foreach (Cell curCell in m_Cells)
            {
                if ((curCell.Members.Count != 0) && curCell.BBox.isOverlappedWith(ref QueryBox))
                {
                    //add any entities found within query radius to the neighbor list
                    foreach (NavGraphNode objEntity in curCell.Members)
                    {
                        if (Vector2.DistanceSquared(objEntity.Pos(), TargetPos) < queryRadiusSquared)
                        {
                            m_Neighbors.Add(objEntity);
                        }
                    }
                }
            } // next cell
        }

        //empties the cells of entities
        public void EmptyCells()
        {
            foreach (Cell it in m_Cells)
            {
                it.Members.Clear();
            }
        }

        //call this to use the gdi to render the cell edges
        public void RenderCells(PrimitiveBatch batch)
        {
            foreach (Cell curCell in m_Cells)
            {
                curCell.BBox.Render(batch, false, Color.DarkGray);
            }
        }

        #region Nested type: Cell

        public class Cell
        {
            //all the entities inhabiting this cell

            //the cell's bounding box (it's inverted because the Window's default
            //co-ordinate system has a y axis that increases as it descends)
            public InvertedAABBox2D BBox;
            public List<NavGraphNode> Members;

            public Cell(Vector2 topleft,
                        Vector2 botright)
            {
                BBox = new InvertedAABBox2D(topleft, botright);
                Members = new List<NavGraphNode>();
            }
        }

        #endregion
    }
}