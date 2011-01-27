using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.BarFlyStates
{
	public class Inebriated : State<BarFly>
	{
		private static Inebriated instance;

		private Inebriated()
		{
		}

		public static Inebriated Instance
		{
			get
			{
				if (instance == null)
					instance = new Inebriated();
				return instance;
			}
		}

		public override void Enter(BarFly entity)
		{
			entity.OutputStatusMessage("Entering inebriated state.");
		}

		public override void Execute(BarFly entity)
		{
			entity.OutputStatusMessage("Executing inebriated state.");
		}

		public override void Exit(BarFly entity)
		{
			entity.OutputStatusMessage("Leaving inebriated state.");
		}
	}
}
