#region Using

using System;
using System.IO;
using Common._2D;
using Common.Interfaces;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common.Game
{
    public class BaseGameEntity : IDisposable
    {
        protected static MessageDispatcher Dispatcher = MessageDispatcher.Instance();

        public void Dispose()
        {
        }

        #region EntityTypes enum

        public
            enum EntityTypes
        {
            default_entity_type = -1
        }

        #endregion

        private static int m_iNextValidID;
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


        protected BaseGameEntity(int ID)
        {
            SetBRadius(0.0f);
            SetScale(new Vector2(1.0f, 1.0f));
            SetEntityType((int) EntityTypes.default_entity_type);
            UnTag();

            SetID(ID);
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
            Utils.TestFloat(r);
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
            Utils.TestVector(val);
            if (Scale() == Vector2.Zero)
            {
                SetBRadius(BRadius() * Math.Max(val.X, val.Y));
            }
            else
            {
                SetBRadius(BRadius() * Math.Max(val.X, val.Y)/Math.Max(Scale().X, Scale().Y));
            }
            m_vScale = val;
        }

        public void SetScale(float val)
        {
            Utils.TestFloat(val);
            Utils.TestVector(m_vScale, true);
            if (Scale() == Vector2.Zero)
            {
                SetBRadius(BRadius() * val);
            }
            else
            {
                SetBRadius(BRadius()*(val/Math.Max(Scale().X, Scale().Y)));
            }
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