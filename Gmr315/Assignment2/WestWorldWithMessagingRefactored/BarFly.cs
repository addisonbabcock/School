using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.BarFlyStates;

namespace WestWorldWithMessagingRefactored
{
	public class BarFly : BaseGameEntity
	{
		private readonly StateMachine<BarFly> stateMachine;

        public BarFly(int id) : base(id)
        {
			stateMachine = new StateMachine<BarFly> (this, Inebriated.Instance);
			OutputMessageColor = ConsoleColor.Blue;
		}

		public StateMachine<BarFly> GetFSM()
		{
			return stateMachine;
		}

		public override bool HandleMessage(Message message)
		{
			//throw new NotImplementedException ();
			return GetFSM().HandleMessage(message);
		}

		public override void Update ()
		{
			//throw new NotImplementedException ();
			GetFSM().Update();
		}
	}
}
