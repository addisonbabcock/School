#region Using

using System;
using WestWorldWithMessaging.FSM;
using WestWorldWithMessaging.Messaging;

#endregion

namespace WestWorldWithMessaging
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

        public override bool OnMessage(Miner pMiner, Telegram msg)
        {
            //send msg to global message handler
            return false;
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

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            return false;
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
                MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                                             pMiner.ID(), (int) EntityName.ent_Elsa,
                                                             (int) message_type.Msg_HiHoneyImHome,
                                                             MessageDispatcher.NO_ADDITIONAL_INFO);
            }
        }

        public override void Execute(Miner pMiner)
        {
            //if miner is not fatigued start to dig for nuggets again.
            if (!pMiner.Fatigued())
            {
                Console.Write("{0}{1}: All mah fatigue has drained away. Time to find more gold!", Environment.NewLine,
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
        }

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            ConsoleColor currentBackground = Console.BackgroundColor;
            ConsoleColor currentForeground = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            switch (telegram.Msg)
            {
                case (int) message_type.Msg_StewReady:
                    Console.Write("{0}Message handled by {1} at time: {2}", Environment.NewLine,
                                  EntityNames.GetNameOfEntity(entity.ID()), DateTime.Now);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("{0}{1}: Okay Hun, ahm a comin'!", Environment.NewLine,
                                  EntityNames.GetNameOfEntity(entity.ID()));
                    entity.GetFSM().ChangeState(EatStew.Instance());
                    Console.BackgroundColor = currentBackground;
                    Console.ForegroundColor = currentForeground;
                    return true;
            }
            Console.BackgroundColor = currentBackground;
            Console.ForegroundColor = currentForeground;
            return false;
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
            pMiner.BuyAndDrinkAWhiskey();


            Console.Write("{0}{1}: That's mighty fine sippin liquer", Environment.NewLine,
                          EntityNames.GetNameOfEntity(pMiner.ID()));

            pMiner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance());
        }

        public override void Exit(Miner pMiner)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("{0}{1}: Leaving the saloon, feelin' good", Environment.NewLine,
                          EntityNames.GetNameOfEntity(pMiner.ID()));

            Console.ForegroundColor = currentColor;
        }

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            return false;
        }
    }

    public class EatStew : State<Miner>
    {
        private static EatStew instance;

        private EatStew()
        {
        }

        public static EatStew Instance()
        {
            if (instance == null)
            {
                instance = new EatStew();
            }
            return instance;
        }

        public override void Enter(Miner entity)
        {
            Console.Write("{0}{1}: Smells Reaaal goood Elsa!", Environment.NewLine,
                          EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override void Execute(Miner entity)
        {
            Console.Write("{0}{1}: Tastes real good too!", Environment.NewLine, EntityNames.GetNameOfEntity(entity.ID()));
            entity.GetFSM().RevertToPreviousState();
        }

        public override void Exit(Miner entity)
        {
            Console.Write("{0}{1}: Thankya li'lle lady. Ah better get back to whatever ah wuz doin'",
                          Environment.NewLine, EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override bool OnMessage(Miner entity, Telegram telegram)
        {
            return false;
        }
    }
}