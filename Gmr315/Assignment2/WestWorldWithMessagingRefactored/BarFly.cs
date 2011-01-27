using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.BarFlyStates;

namespace WestWorldWithMessagingRefactored
{
	public class BarFly : BaseGameEntity
	{
		private readonly StateMachine<BarFly> stateMachine;
		private int currentBeerCount;

        public BarFly(int id) : base(id)
        {
			currentBeerCount = 0;
			stateMachine = new StateMachine<BarFly> (this, Sober.Instance);
			OutputMessageColor = ConsoleColor.Blue;
		}

		public StateMachine<BarFly> GetFSM()
		{
			return stateMachine;
		}

		public int HowDrunkAmI()
		{
			return currentBeerCount;
		}

		public override bool HandleMessage(Message message)
		{
			return GetFSM().HandleMessage(message);
		}

		public override void Update ()
		{
			currentBeerCount++;
			GetFSM().Update();
		}
	}
}
