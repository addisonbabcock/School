#region Using

using System;

#endregion

namespace WestWorldWithWoman
{
    public abstract class BaseGameEntity : IDisposable
    {
        private static int m_iNextValidID;
        //every entity must have a unique identifying number
        private int m_ID;


        protected BaseGameEntity(int id)
        {
            SetID(id);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        ~BaseGameEntity()
        {
            Dispose(false);
        }

        //all entities must implement an update function
        public abstract void Update();

        public int ID()
        {
            return m_ID;
        }


        //----------------------------- SetID -----------------------------------------
        //
        //  this must be called within each constructor to make sure the ID is set
        //  correctly. It verifies that the value passed to the method is greater
        //  or equal to the next valid ID, before setting the ID and incrementing
        //  the next valid ID
        //-----------------------------------------------------------------------------
        private void SetID(int val)
        {
            //make sure the val is equal to or greater than the next available ID
            if (val < m_iNextValidID)
            {
                throw new ArgumentException(
                    "Set ID provided with an Invalid ID.  ID must be greater then or equal to the next available id");
            }

            m_ID = val;

            m_iNextValidID = m_ID + 1;
        }
    }
}