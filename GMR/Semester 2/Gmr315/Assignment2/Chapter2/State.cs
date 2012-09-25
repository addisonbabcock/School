#region Using

using System;

#endregion

namespace Westworld1
{
    public abstract class State : IDisposable
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
        public abstract void Enter(Miner miner);

        //this is the state's normal update function
        public abstract void Execute(Miner miner);

        //this will execute when the state is exited. (My word, isn't
        //life full of surprises... ;o))
        public abstract void Exit(Miner miner);
    }
}