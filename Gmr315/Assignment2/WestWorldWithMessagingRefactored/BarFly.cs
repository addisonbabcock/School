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
		private bool minerVisible;

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

		public void Drink()
		{
			++currentBeerCount;
		}

		public void Sleep()
		{
			currentBeerCount -= 3;
			currentBeerCount = Math.Min(currentBeerCount, 0);
		}

		public int HowDrunkAmI()
		{
			return currentBeerCount;
		}

		public bool IsMinerVisible()
		{
			return minerVisible;
		}

		public override bool HandleMessage(Message message)
		{
			switch (message.MessageType)
			{
				case MessageType.MinerEnteringTheBar:
					minerVisible = true;
					OutputStatusMessage("Yeah? Wha do ya want?");
					return true;

				case MessageType.MinerLeavingTheBar:
					OutputStatusMessage("Yeah that's what I thought.");
					minerVisible = false;
					return true;
			}

			return GetFSM().HandleMessage(message);
		}

		public override void Update ()
		{
			GetFSM().Update();
		}
	}
}
