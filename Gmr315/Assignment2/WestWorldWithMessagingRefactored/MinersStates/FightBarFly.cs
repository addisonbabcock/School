using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.MinersStates
{
	public class FightBarFly : State <Miner>
	{
		private static FightBarFly instance;

		private Random rng;

		private FightBarFly()
		{
			rng = new Random();
		}

		public static FightBarFly Instance
		{
			get
			{
				if (instance == null)
					instance = new HungOver();
				return instance;
			}
		}

		public override void Enter (Miner entity)
		{
			entity.OutputStatusMessage ("So ye want a fight do ye?");
		}

		public override void Execute (Miner entity)
		{
			int rand = rng.Next (100);
			if (rand < 30)
			{
				//attack with punch
				entity.OutputStatusMessage ("FALCON PUUUUUUUUUNCH");
				MessageDispatcher.Instance.DispatchMessage (
					0.0,
					(int)EntityName.MinerBob,
					(int)EntityName.BarFly,
					MessageType.MinerAttacksWithPunch,
					MessageDispatcher.NoAdditionalInfo);
			}
			else if (rand < 60)
			{
				//attack with chair
				entity.OutputStatusMessage ("FALCON CHAIR!");
				MessageDispatcher.Instance.DispatchMessage (
					0.0,
					(int)EntityName.MinerBob,
					(int)EntityName.BarFly,
					MessageType.MinerAttacksWithChair,
					MessageDispatcher.NoAdditionalInfo);
			}
			else
			{
			}
		}

		public override void Exit (Miner entity)
		{
			throw new NotImplementedException ();
		}
	}
}
