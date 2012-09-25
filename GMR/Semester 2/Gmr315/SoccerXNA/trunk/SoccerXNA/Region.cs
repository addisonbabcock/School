#region Using

using System;
using Common._2D;
using Common.Interfaces;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SoccerXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Region : DrawableGameComponent
    {
        #region region_modifier enum

        public enum region_modifier
        {
            halfsize,
            normal
        } ;

        #endregion

        private SpriteFont gameFont;

        protected float m_dBottom;
        protected float m_dHeight;


        protected float m_dLeft;
        protected float m_dRight;
        protected float m_dTop;

        protected float m_dWidth;

        protected int m_iID;
        protected Vector2 m_vCenter;
        private PrimitiveBatch primitiveBatch;
        private SpriteBatch spriteBatch;

        public Region(Game game)
            : this(game, 0, 0, 0, 0)
        {
        }

        public Region(Game game, float left, float top, float right, float bottom)
            : this(game, left, top, right, bottom, -1)
        {
        }

        public Region(Game game, float left,
                      float top,
                      float right,
                      float bottom,
                      int id) : base(game)
        {
            m_dTop = top;
            m_dRight = right;
            m_dLeft = left;
            m_dBottom = bottom;
            m_iID = id;

            //calculate center of region
            m_vCenter = new Vector2((left + right)*0.5f, (top + bottom)*0.5f);

            m_dWidth = Math.Abs(right - left);
            m_dHeight = Math.Abs(bottom - top);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            IFontManager manager = (IFontManager) Game.Services.GetService(typeof (IFontManager));
            gameFont = manager.GetFont("Default");

            IDrawingManager drawingManager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
            spriteBatch = drawingManager.GetSpriteBatch();
            primitiveBatch = drawingManager.GetPrimitiveBatch();
            base.Initialize();
        }


        ~Region()
        {
            Dispose(false);
        }

        public override void Draw(GameTime gameTime)
        {
            Draw(gameTime, false);
        }

        public void Draw(GameTime gameTime, bool ShowID)
        {
            Drawing.DrawRectangle(primitiveBatch, Color.Green, m_dLeft, m_dTop, m_dRight, m_dBottom);
            if (ShowID)
            {
                spriteBatch.DrawString(gameFont, ID().ToString(), Center(), Color.Green);
            }
            base.Draw(gameTime);
        }

        public bool Inside(Vector2 pos)
        {
            return Inside(pos, region_modifier.normal);
        }

        //returns true if the given position lays inside the region. The
        //region modifier can be used to contract the region bounderies
        public bool Inside(Vector2 pos, region_modifier r)
        {
            {
                if (r == region_modifier.normal)
                {
                    return ((pos.X > m_dLeft) && (pos.X < m_dRight) &&
                            (pos.Y > m_dTop) && (pos.Y < m_dBottom));
                }

                float marginX = m_dWidth*0.25f;
                float marginY = m_dHeight*0.25f;

                return ((pos.X > (m_dLeft + marginX)) && (pos.X < (m_dRight - marginX)) &&
                        (pos.Y > (m_dTop + marginY)) && (pos.Y < (m_dBottom - marginY)));
            }
        }

        //returns a vector representing a random location
        //within the region
        public Vector2 GetRandomPosition()
        {
            return new Vector2(Utils.RandInRange(m_dLeft, m_dRight),
                               Utils.RandInRange(m_dTop, m_dBottom));
        }

        //-------------------------------
        public float Top()
        {
            return m_dTop;
        }

        public float Bottom()
        {
            return m_dBottom;
        }

        public float Left()
        {
            return m_dLeft;
        }

        public float Right()
        {
            return m_dRight;
        }

        public float Width()
        {
            return Math.Abs(m_dRight - m_dLeft);
        }

        public float Height()
        {
            return Math.Abs(m_dTop - m_dBottom);
        }

        public float Length()
        {
            return Math.Max(Width(), Height());
        }

        public float Breadth()
        {
            return Math.Min(Width(), Height());
        }

        public Vector2 Center()
        {
            return m_vCenter;
        }

        public int ID()
        {
            return m_iID;
        }
    }
}