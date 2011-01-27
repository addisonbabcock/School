#region Using

using System;
using WestWorldWithWoman.FSM;

#endregion

namespace WestWorldWithWoman
{
    public class EnterMineAndDigForNugget : State<Miner>
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
                Console.Write("{0}{1}: Walkin' to the goldmine", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));

                pMiner.ChangeLocation(location_type.goldmine);
            }
        }


        public override void Execute(Miner pMiner)
        {
            //Now the miner is at the goldmine he digs for gold until he
            //is carrying in excess of MaxNuggets. If he gets thirsty during
            //his digging he packs up work for a while and changes state to
            //gp to the saloon for a whiskey.
            pMiner.AddToGoldCarried(1);

            pMiner.IncreaseFatigue();

            Console.Write("{0}{1}: Pickin' up a nugget", Environment.NewLine, EntityNames.GetNameOfEntity(pMiner.ID()));

            //if enough gold mined, go and put it in the bank
            if (pMiner.PocketsFull())
            {
                pMiner.GetFSM().ChangeState(VisitBankAndDepositGold.Instance());
            }

            if (pMiner.Thirsty())
            {
                pMiner.GetFSM().ChangeState(QuenchThirst.Instance());
            }
        }


        public override void Exit(Miner pMiner)
        {
            Console.Write("{0}{1}: Ah'm leavin' the goldmine with mah pockets full o' sweet gold", Environment.NewLine,
                          EntityNames.GetNameOfEntity(pMiner.ID()));
        }
    }

    public class VisitBankAndDepositGold : State<Miner>
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
                Console.Write("{0}{1}: Goin' to the bank. Yes siree", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));
                pMiner.ChangeLocation(location_type.bank);
            }
        }


        public override void Execute(Miner pMiner)
        {
            //deposit the gold
            pMiner.AddToWealth(pMiner.GoldCarried());

            pMiner.SetGoldCarried(0);

            Console.Write("{0}{1}: Depositing gold.  Total savings now: {2}", Environment.NewLine,
                          EntityNames.GetNameOfEntity(pMiner.ID()),
                          pMiner.Wealth());

            //wealthy enough to have a well earned rest?
            if (pMiner.Wealth() >= Miner.ComfortLevel)
            {
                Console.Write("{0}{1}: WooHoo! Rich enough for now. Back home to mah li'lle lady", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));
                pMiner.GetFSM().ChangeState(GoHomeAndSleepTilRested.Instance());
            }

                //otherwise get more gold
            else
            {
                pMiner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance());
            }
        }


        public override void Exit(Miner pMiner)
        {
            Console.Write("{0}{1}: Leavin' the bank", Environment.NewLine, EntityNames.GetNameOfEntity(pMiner.ID()));
        }
    }

    public class GoHomeAndSleepTilRested : State<Miner>
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
                Console.Write("{0}{1}: Walkin' home", Environment.NewLine, pMiner.ID());

                pMiner.ChangeLocation(location_type.shack);
            }
        }

        public override void Execute(Miner pMiner)
        {
            //if miner is not fatigued start to dig for nuggets again.
            if (!pMiner.Fatigued())
            {
                Console.Write("{0}{1}: What a God darn fantastic nap! Time to find more gold", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));


                pMiner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance());
            }

            else
            {
                //sleep
                pMiner.DecreaseFatigue();

                ConsoleColor currentColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("{0}{1}: ZZZZZ....", Environment.NewLine, EntityNames.GetNameOfEntity(pMiner.ID()));
                Console.ForegroundColor = currentColor;
            }
        }

        public override void Exit(Miner pMiner)
        {
            Console.Write("{0}{1}: Leaving the house", Environment.NewLine, EntityNames.GetNameOfEntity(pMiner.ID()));
        }
    }

    public class QuenchThirst : State<Miner>
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

                Console.Write("{0}{1}: Boy, ah sure is thusty! Walking to the saloon", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));
            }
        }

        public override void Execute(Miner pMiner)
        {
            if (pMiner.Thirsty())
            {
                pMiner.BuyAndDrinkAWhiskey();


                Console.Write("{0}{1}: That's mighty fine sippin liquer", Environment.NewLine,
                              EntityNames.GetNameOfEntity(pMiner.ID()));

                pMiner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance());
            }
            else
            {
                Console.Write("{0}ERROR!{0}ERROR!{0}ERROR!", Environment.NewLine);
            }
        }

        public override void Exit(Miner pMiner)
        {
            Console.Write("{0}{1}: Leaving the saloon, feelin' good", Environment.NewLine,
                          EntityNames.GetNameOfEntity(pMiner.ID()));
        }
    }
}