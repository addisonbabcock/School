﻿#region Using

using System;

#endregion

namespace WestWorldWithWoman.FSM
{
    public abstract class State<T> : IDisposable where T : BaseGameEntity
    {
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

        ~State()
        {
            Dispose(false);
        }

        //this will execute when the state is entered
        public abstract void Enter(T entity);

        //this is the state's normal update function
        public abstract void Execute(T entity);

        //this will execute when the state is exited. (My word, isn't
        //life full of surprises... ;o))
        public abstract void Exit(T entity);
    }
}