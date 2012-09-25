using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.BarFlyStates
{
	public class Sleeping : State<BarFly>
	{
		private static Sleeping instance;

		private Sleeping()
		{
		}

		public static Sleeping Instance
		{
			get
			{
				if (instance == null)
					instance = new Sleeping();
				return instance;
			}
		}

		public override void Enter(BarFly entity)
		{
			entity.OutputStatusMessage("So tirrred...");
		}

		public override void Execute(BarFly entity)
		{
			entity.OutputStatusMessage("ZZZZzZZZzZzzzzzz");
			entity.Sleep();
			if (entity.HowDrunkAmI() == 0)
				entity.GetFSM().ChangeState(HungOver.Instance);
		}

		public override void Exit(BarFly entity)
		{
			entity.OutputStatusMessage("Done sleeping.");
		}
	}
}
