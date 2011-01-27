#region Using

using System;

#endregion

namespace Westworld1
{
    //------------------------------------------------------------------------
//
//  In this state the miner will walk to a goldmine and pick up a nugget
//  of gold. If the miner already has a nugget of gold he'll change state
//  to VisitBankAndDepositGold. If he gets thirsty he'll change state
//  to QuenchThirst
//------------------------------------------------------------------------
    public class EnterMineAndDigForNugget : State
    {
        private static EnterMineAndDigForNugget instance;

        private EnterMineAndDigForNugget()
        {
        }

        public static EnterMineAndDigForNugget Instance()
        {
            if (instance == null)
            {
                instance = new EnterMineAndDigForNugget();
            }

            return instance;
        }

        public override void Enter(Miner pMiner)
        {
            //if the miner is not already located at the goldmine, he must
            //change location to the gold mine
            if (pMiner.Location() != location_type.goldmine)
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: Walkin' to the goldmine", Environment.NewLine, pMiner.ID());
                Console.ForegroundColor = currentColor;

                pMiner.ChangeLocation(location_type.goldmine);
            }
        }


        public override void Execute(Miner pMiner)
        {
            //the miner digs for gold until he is carrying in excess of MaxNuggets. 
            //If he gets thirsty during his digging he packs up work for a while and 
            //changes state to go to the saloon for a whiskey.
            pMiner.AddToGoldCarried(1);

            pMiner.IncreaseFatigue();

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Pickin' up a nugget", Environment.NewLine, pMiner.ID());
            Console.ForegroundColor = currentColor;

            //if enough gold mined, go and put it in the bank
            if (pMiner.PocketsFull())
            {
                pMiner.ChangeState(VisitBankAndDepositGold.Instance());
            }

            if (pMiner.Thirsty())
            {
                pMiner.ChangeState(QuenchThirst.Instance());
            }
        }


        public override void Exit(Miner pMiner)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Ah'm leavin' the goldmine with mah pockets full o' sweet gold", Environment.NewLine,
                          pMiner.ID());
            Console.ForegroundColor = currentColor;
        }
    }

//------------------------------------------------------------------------
//
//  Entity will go to a bank and deposit any nuggets he is carrying. If the 
//  miner is subsequently wealthy enough he'll walk home, otherwise he'll
//  keep going to get more gold
//------------------------------------------------------------------------
    public class VisitBankAndDepositGold : State
    {
        private static VisitBankAndDepositGold instance;

        private VisitBankAndDepositGold()
        {
        }

        public static VisitBankAndDepositGold Instance()
        {
            if (instance == null)
            {
                instance = new VisitBankAndDepositGold();
            }

            return instance;
        }


        public override void Enter(Miner pMiner)
        {
            //on entry the miner makes sure he is located at the bank
            if (pMiner.Location() != location_type.bank)
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: Goin' to the bank. Yes siree", Environment.NewLine, pMiner.ID());
                Console.ForegroundColor = currentColor;
                pMiner.ChangeLocation(location_type.bank);
            }
        }


        public override void Execute(Miner pMiner)
        {
            //deposit the gold
            pMiner.AddToWealth(pMiner.GoldCarried());

            pMiner.SetGoldCarried(0);

            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Depositing gold.  Total savings now: {2}", Environment.NewLine, pMiner.ID(),
                          pMiner.Wealth());
            Console.ForegroundColor = currentColor;

            //wealthy enough to have a well earned rest?
            if (pMiner.Wealth() >= Miner.ComfortLevel)
            {
                currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: WooHoo! Rich enough for now. Back home to mah li'lle lady", Environment.NewLine,
                              pMiner.ID());
                Console.ForegroundColor = currentColor;
                pMiner.ChangeState(GoHomeAndSleepTilRested.Instance());
            }

                //otherwise get more gold
            else
            {
                pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
            }
        }


        public override void Exit(Miner pMiner)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Leavin' the bank", Environment.NewLine, pMiner.ID());
            Console.ForegroundColor = currentColor;
        }
    }


//------------------------------------------------------------------------
//
//  miner will go home and sleep until his fatigue is decreased
//  sufficiently
//------------------------------------------------------------------------
    public class GoHomeAndSleepTilRested : State
    {
        private static GoHomeAndSleepTilRested instance;

        private GoHomeAndSleepTilRested()
        {
        }

        public static GoHomeAndSleepTilRested Instance()
        {
            if (instance == null)
            {
                instance = new GoHomeAndSleepTilRested();
            }

            return instance;
        }

        public override void Enter(Miner pMiner)
        {
            if (pMiner.Location() != location_type.shack)
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: Walkin' home", Environment.NewLine, pMiner.ID());
                Console.ForegroundColor = currentColor;

                pMiner.ChangeLocation(location_type.shack);
            }
        }

        public override void Execute(Miner pMiner)
        {
            //if miner is not fatigued start to dig for nuggets again.
            if (!pMiner.Fatigued())
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: What a God darn fantastic nap! Time to find more gold", Environment.NewLine,
                              pMiner.ID());
                Console.ForegroundColor = currentColor;

                pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
            }

            else
            {
                //sleep
                pMiner.DecreaseFatigue();

                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: ZZZZZ....", Environment.NewLine, pMiner.ID());

                Console.ForegroundColor = currentColor;
            }
        }

        public override void Exit(Miner pMiner)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Leaving the house", Environment.NewLine, pMiner.ID());

            Console.ForegroundColor = currentColor;
        }
    }


//------------------------------------------------------------------------
//
//------------------------------------------------------------------------
    public class QuenchThirst : State
    {
        private static QuenchThirst instance;

        private QuenchThirst()
        {
        }

        public static QuenchThirst Instance()
        {
            if (instance == null)
            {
                instance = new QuenchThirst();
            }
            return instance;
        }

        public override void Enter(Miner pMiner)
        {
            if (pMiner.Location() != location_type.saloon)
            {
                pMiner.ChangeLocation(location_type.saloon);

                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: Boy, ah sure is thusty! Walking to the saloon", Environment.NewLine, pMiner.ID());
                Console.ForegroundColor = currentColor;
            }
        }

        public override void Execute(Miner pMiner)
        {
            if (pMiner.Thirsty())
            {
                pMiner.BuyAndDrinkAWhiskey();

                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: That's mighty fine sippin liquer", Environment.NewLine, pMiner.ID());
                Console.ForegroundColor = currentColor;

                pMiner.ChangeState(EnterMineAndDigForNugget.Instance());
            }

            else
            {
                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}ERROR!{0}ERROR!{0}ERROR!", Environment.NewLine);
                Console.ForegroundColor = currentColor;
            }
        }

        public override void Exit(Miner pMiner)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Leaving the saloon, feelin' good", Environment.NewLine, pMiner.ID());

            Console.ForegroundColor = currentColor;
        }
    }
}