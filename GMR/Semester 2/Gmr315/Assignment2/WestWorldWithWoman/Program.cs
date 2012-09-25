#region Using

using System;
using System.Threading;

#endregion

namespace WestWorldWithWoman
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //create a miner
            Miner Bob = new Miner((int) EntityName.ent_Miner_Bob);

            // create his wife
            MinersWife Elsa = new MinersWife((int) EntityName.ent_Elsa);


            //simply run the miner through a few Update calls
            for (int i = 0; i < 20; ++i)
            {
                Bob.Update();
                Elsa.Update();

                Thread.Sleep(800);
            }


            //wait for a keypress before exiting
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            return;
        }
    }
}