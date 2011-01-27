#region Using

using System;
using System.Threading;
using WestWorldWithMessagingRefactored;
using WestWorldWithMessagingRefactored.Misc;

#endregion

namespace WestWorldWithMessagingRefactored
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //create a miner
            Miner Bob = new Miner((int) EntityName.MinerBob);

            // create his wife
            MinersWife Elsa = new MinersWife((int) EntityName.Elsa);

			//create the barfly
			BarFly BarFly = new BarFly((int) EntityName.BarFly);

            // register them with the entity manager
            EntityManager.Instance.RegisterEntity(Bob);
            EntityManager.Instance.RegisterEntity(Elsa);
			EntityManager.Instance.RegisterEntity(BarFly);

            //simply run the miner through a few Update calls
            for (int i = 0; i < 30; ++i)
            {
                Bob.Update();
                Elsa.Update();
				BarFly.Update();

                // Dispatch any delayed messages
                MessageDispatcher.Instance.DispatchDelayedMessages();

                Thread.Sleep(800);
            }

            //wait for a keypress before exiting
            ConsoleManager.PressAnyKeyToContinue();
            return;
        }
    }
}