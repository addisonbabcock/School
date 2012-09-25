#region Using

using Common.Messaging;

#endregion

namespace Common.FSM
{
    public abstract class State<T>
    {
        ~State()
        {
        }

        //this will execute when the state is entered
        public abstract void Enter(T entity);

        //this is the states normal update function
        public abstract void Execute(T entity);

        //this will execute when the state is exited. 
        public abstract void Exit(T entity);

        //this executes if the agent receives a message from the 
        //message dispatcher
        public abstract bool OnMessage(T entity, Telegram message);
    }
}