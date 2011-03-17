#region Using

using System;
using System.IO;
using Common._2D;
using Common.Interfaces;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Game
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class BaseGameEntity : DrawableGameComponent
    {
        #region EntityTypes enum

        public
            enum EntityTypes
        {
            default_entity_type = -1
        }

        #endregion

        private static SpriteFont gameFont;

        private static int m_iNextValidID;
        private static PrimitiveBatch primitiveBatch;
        private static SpriteBatch spriteBatch;
        private bool m_bTag;
        protected float m_dBoundingRadius;

        //each entity has a unique ID
        private int m_ID;

        //every entity has a type associated with it (health, troll, ammo etc)
        private int m_iType;

        //this is a generic flag. 


        //its location in the environment
        protected Vector2 m_vPosition;

        protected Vector2 m_vScale;

        //the magnitude of this object's bounding radius


        protected BaseGameEntity(Microsoft.Xna.Framework.Game game, int ID) : base(game)
        {
            m_dBoundingRadius = 0.0f;
            m_vScale = new Vector2(1.0f, 1.0f);
            m_iType = (int) EntityTypes.default_entity_type;
            m_bTag = false;

            SetID(ID);
        }

        protected PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        protected virtual SpriteFont GameFont
        {
            get
            {
                if (gameFont == null)
                {
                    IFontManager manager = (IFontManager) Game.Services.GetService(typeof (IFontManager));
                    gameFont = manager.GetFont("Default");
                }
                return gameFont;
            }
        }

        protected SpriteBatch SpriteBatch
        {
            get
            {
                if (spriteBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    spriteBatch = manager.GetSpriteBatch();
                }
                return spriteBatch;
            }
        }

        private void SetID(int val)
        {
            //make sure the val is equal to or greater than the next available ID
            if (val < m_iNextValidID)
            {
                throw new ArgumentException("Original assertion: BaseGameEntity.SetId was provided an invalid id", "val");
            }

            m_ID = val;

            m_iNextValidID = m_ID + 1;
        }

        public virtual void Update()
        {
        }

        public virtual bool HandleMessage(Telegram msg)
        {
            return false;
        }

        //entities should be able to read/write their data to a stream
        public virtual void Write(StreamWriter os)
        {
        }

        public virtual void Read(StreamReader inputStream)
        {
        }

        //use this to grab the next valid ID
        public static int GetNextValidID()
        {
            return m_iNextValidID;
        }

        //this can be used to reset the next ID
        public static void ResetNextValidID()
        {
            m_iNextValidID = 0;
        }


        public Vector2 Pos()
        {
            return m_vPosition;
        }

        public void SetPos(Vector2 new_pos)
        {
            m_vPosition = new_pos;
        }

        public float BRadius()
        {
            return m_dBoundingRadius;
        }

        public void SetBRadius(float r)
        {
            m_dBoundingRadius = r;
        }

        public int ID()
        {
            return m_ID;
        }

        public bool IsTagged()
        {
            return m_bTag;
        }

        public void Tag()
        {
            m_bTag = true;
        }

        public void UnTag()
        {
            m_bTag = false;
        }

        public Vector2 Scale()
        {
            return m_vScale;
        }

        public void SetScale(Vector2 val)
        {
            if (Scale() == Vector2.Zero)
            {
                SetBRadius(m_dBoundingRadius * Math.Max(val.X, val.Y));
            }
            else
            {
                m_dBoundingRadius *= Math.Max(val.X, val.Y)/Math.Max(m_vScale.X, m_vScale.Y);
            }
            m_vScale = val;
        }

        public void SetScale(float val)
        {
            m_dBoundingRadius *= (val/Math.Max(m_vScale.X, m_vScale.Y));
            m_vScale = new Vector2(val, val);
        }

        public int EntityType()
        {
            return m_iType;
        }

        public void SetEntityType(int new_type)
        {
            m_iType = new_type;
        }
    }
}