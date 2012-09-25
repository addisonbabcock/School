#region Using

using System;
using System.Threading;

#endregion

namespace Westworld1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //create a miner
            Miner miner = new Miner((int) EntityName.ent_Miner_Bob);
            Miner miner2 = new Miner(2);
            miner2.AddToGoldCarried(2);
            //simply run the miner through a few Update calls
            for (int i = 0; i < 20; ++i)
            {
                miner.Update();
                miner2.Update();

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