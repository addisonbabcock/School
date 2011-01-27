using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.BarFlyStates
{
	public class HungOver : State<BarFly>
	{
		private static HungOver instance;

		private Random rng;
		private bool minerInTheBar;

		private HungOver()
		{
			rng = new Random();
			minerInTheBar = false;
		}

		public static HungOver Instance
		{
			get
			{
				if (instance == null)
					instance = new HungOver();
				return instance;
			}
		}

		public override bool OnMessage(BarFly entity, Message message)
		{
			switch (message.MessageType)
			{
				case MessageType.MinerEnteringTheBar:
					entity.OutputStatusMessage("Yeah? Wha do ya want?");
					minerInTheBar = true;
					return true;

				case MessageType.MinerLeavingTheBar:
					entity.OutputStatusMessage("Yeah that's what I thought.");
					minerInTheBar = false;
					return true;
			}

			return base.OnMessage(entity, message);
		}

		public override void Enter(BarFly entity)
		{
			entity.OutputStatusMessage("Owwww my head");
		}

		public override void Execute(BarFly entity)
		{
			entity.OutputStatusMessage("Need to drink to ease the pain...");
			entity.Drink();

			if (rng.Next(2) == 0)
			{
				//fight the miner
				if (minerInTheBar)
				{
					MessageDispatcher.Instance.DispatchMessage(
						0.0, 
						(int)EntityName.BarFly, 
						(int)EntityName.MinerBob, 
						MessageType.IChallengeYouToADuel, 
						MessageDispatcher.NoAdditionalInfo);
				}
			}

			if (entity.HowDrunkAmI() >= 5)
				entity.GetFSM().ChangeState(Sober.Instance);
		}

		public override void Exit(BarFly entity)
		{
			entity.OutputStatusMessage("Much better!");
		}
	}
}
