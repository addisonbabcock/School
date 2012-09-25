#region Using

using System;
using WestWorldWithMessagingRefactored.Messaging;

#endregion

namespace WestWorldWithMessagingRefactored.FSM
{
    public class StateMachine<T> where T : BaseGameEntity
    {
        /// <summary>
        /// The entity that owns this state machine
        /// </summary>
        private readonly T owner;

        /// <summary>
        /// Initializes the state machine for the owner.
        /// Current state is set to the inital state provided
        /// </summary>
        /// <param name="owner">Owner of the state machine</param>
        /// <param name="initialState">The owners initial state</param>
        public StateMachine(T owner, State<T> initialState) : this(owner, initialState, null)
        {
        }

        /// <summary>
        /// Initializes the state machine for the owner.
        /// Current state is set to the inital state provided
        /// Global state is set to the state provided
        /// </summary>
        /// <param name="owner">Owner of the state machine</param>
        /// <param name="initialState">initial state the owner starts in</param>
        /// <param name="globalState">global state the owner is in</param>
        public StateMachine(T owner, State<T> initialState, State<T> globalState)
        {
            this.owner = owner;
            CurrentState = initialState;
            PreviousState = null;
            GlobalState = globalState;
        }

        /// <summary>
        /// Gets or sets the entities CurrentState
        /// </summary>
        public State<T> CurrentState { get; private set; }

        /// <summary>
        /// A global state that is called everytime the 
        /// entity is updated.
        /// </summary>
        public State<T> GlobalState { get; private set; }

        /// <summary>
        /// a record of the last state the entity was in
        /// </summary>
        public State<T> PreviousState { get; private set; }


        /// <summary>
        /// Executes the entity's state.
        /// </summary>
        public void Update()
        {
            ExecuteState(GlobalState);
            ExecuteState(CurrentState);
        }

        /// <summary>
        /// Confirms there is a state to execute and then executes it.
        /// </summary>
        /// <param name="state">Possible state to execute</param>
        private void ExecuteState(State<T> state)
        {
            if (state != null) state.Execute(owner);
        }

        /// <summary>
        /// Handle the message sent to this entity
        /// </summary>
        /// <param name="message">Message to be dealt with</param>
        /// <returns>true if message handled</returns>
        public bool HandleMessage(Message message)
        {
            //first see if the current state is valid and that it can handle
            //the message
            if (HandleMessage(CurrentState, message))
            {
                return true;
            }

            return HandleMessage(GlobalState, message);
        }

        /// <summary>
        /// Confirms that the state to send the message to exists
        /// and then sends the message on
        /// </summary>
        /// <param name="state">State to send the message to</param>
        /// <param name="message">Message to be sent</param>
        /// <returns>true if handled</returns>
        private bool HandleMessage(State<T> state, Message message)
        {
            if (state != null && state.OnMessage(owner, message))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Puts the owner in the new state and records the owners previous state.
        /// </summary>
        /// <param name="newState">Owners new state</param>
        public void ChangeState(State<T> newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException("newState", "Cannot change into a null state.");
            }

            PreviousState = CurrentState;
            CurrentState.Exit(owner);
            CurrentState = newState;
            CurrentState.Enter(owner);
        }

        /// <summary>
        /// Places the owner back into his most recently recorded state.
        /// </summary>
        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }

        /// <summary>
        /// Checks to see if the owner is currently in the provided state
        /// </summary>
        /// <param name="state">The state we wish to check if the owner is in</param>
        /// <returns>true if in the provided state</returns>
        public bool IsInState(State<T> state)
        {
            if (CurrentState.GetType() == state.GetType()) return true;
            return false;
        }
    }
}