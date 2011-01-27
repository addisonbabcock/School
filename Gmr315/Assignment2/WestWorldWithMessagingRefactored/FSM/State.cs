#region Using

using WestWorldWithMessagingRefactored.Messaging;

#endregion

namespace WestWorldWithMessagingRefactored.FSM
{
    public abstract class State<T> where T : BaseGameEntity
    {
        /// <summary>
        /// Code to execute when entity first enters this state
        /// </summary>
        /// <param name="entity">The entity in the state</param>
        public abstract void Enter(T entity);

        /// <summary>
        /// Code to be executed on every update call.
        /// </summary>
        /// <param name="entity">The entity in the state</param>
        public abstract void Execute(T entity);

        /// <summary>
        /// Code that will always be executed when entity exits the state
        /// </summary>
        /// <param name="entity">The entity in the state</param>
        public abstract void Exit(T entity);

        /// <summary>
        /// This executes when the entity receives a message
        /// from the MessageDispatcher
        /// </summary>
        /// <param name="entity">The entity in the state</param>
        /// <param name="message">Message for the entity</param>
        /// <returns>true if message handled</returns>
        public virtual bool OnMessage(T entity, Message message)
        {
            return false;
        }
    }
}