#region Using

using System;
using System.Threading;

#endregion

namespace WestWorldWithMessaging
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //create a miner
            Miner Bob = new Miner((int) EntityName.ent_Miner_Bob);

            // create his wife
            MinersWife Elsa = new MinersWife((int) EntityName.ent_Elsa);

            // register them with the entity manager
            EntityManager.Instance().RegisterEntity(Bob);
            EntityManager.Instance().RegisterEntity(Elsa);

            //simply run the miner through a few Update calls
            for (int i = 0; i < 30; ++i)
            {
                Bob.Update();
                Elsa.Update();

                // Dispatch any delayed messages
                MessageDispatcher.Instance().DispatchDelayedMessages();

                Thread.Sleep(800);
            }

            Bob.Dispose();
            Elsa.Dispose();
            //wait for a keypress before exiting
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            return;
        }
    }
}