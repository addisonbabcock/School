using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.BarFlyStates
{
	public class Sober : State<BarFly>
	{
		private static Sober instance;

		private Sober()
		{
		}

		public static Sober Instance
		{
			get
			{
				if (instance == null)
					instance = new Sober();
				return instance;
			}
		}

		public override void Enter(BarFly entity)
		{
			entity.OutputStatusMessage("I now sober :(");
		}

		public override void Execute(BarFly entity)
		{
			entity.OutputStatusMessage("I am still sober! Need more drinks!");
			if (entity.HowDrunkAmI() >= 10)
			{
				entity.GetFSM().ChangeState(Inebriated.Instance);
			}
		}

		public override void Exit(BarFly entity)
		{
			entity.OutputStatusMessage("Sober no more!");
		}
	}
}
