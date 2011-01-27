#region Using

using System;
using WestWorldWithMessaging.FSM;
using WestWorldWithMessaging.Messaging;
using WestWorldWithMessaging.Misc;

#endregion

namespace WestWorldWithMessaging
{
    public class WifesGlobalState : State<MinersWife>
    {
        private static WifesGlobalState instance;

        private WifesGlobalState()
        {
        }

        public static WifesGlobalState Instance()
        {
            if (instance == null)
            {
                instance = new WifesGlobalState();
            }
            return instance;
        }

        public override void Enter(MinersWife entity)
        {
        }

        public override void Execute(MinersWife entity)
        {
            //1 in 10 chance of needing the bathroom (provided she is not already
            //in the bathroom)
            if ((Utils.RandFloat() < 0.1) &&
                !entity.GetFSM().isInState(VisitBathroom.Instance()))
            {
                entity.GetFSM().ChangeState(VisitBathroom.Instance());
            }
        }

        public override void Exit(MinersWife entity)
        {
        }

        public override bool OnMessage(MinersWife entity, Telegram telegram)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            switch (telegram.Msg)
            {
                case (int) message_type.Msg_HiHoneyImHome:
                    {
                        Console.Write("{0}Message received by {1} at time: {2}", Environment.NewLine,
                                      EntityNames.GetNameOfEntity(entity.ID()), DateTime.Now);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0}{1}: Hi honey. Let me make you some of mah fine country stew",
                                      Environment.NewLine, EntityNames.GetNameOfEntity(entity.ID()));

                        entity.GetFSM().ChangeState(CookStew.Instance());
                    }
                    Console.BackgroundColor = currentBackgroundColor;
                    Console.ForegroundColor = currentForegroundColor;
                    return true;
            } //end switch
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;
            return false;
        }
    }


    //-------------------------------------------------------------------------DoHouseWork

    public class DoHouseWork : State<MinersWife>
    {
        private static DoHouseWork instance;

        private DoHouseWork()
        {
        }

        public static DoHouseWork Instance()
        {
            if (instance == null)
            {
                instance = new DoHouseWork();
            }
            return instance;
        }

        public override void Enter(MinersWife entity)
        {
            Console.Write("{0}{1}: Time to do some more housework!", Environment.NewLine,
                          EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override void Execute(MinersWife entity)
        {
            switch (Utils.RandInt(0, 2))
            {
                case 0:
                    Console.Write("{0}{1}: Moppin' the floor", Environment.NewLine,
                                  EntityNames.GetNameOfEntity(entity.ID()));


                    break;

                case 1:
                    Console.Write("{0}{1}: Washin' the dishes", Environment.NewLine,
                                  EntityNames.GetNameOfEntity(entity.ID()));


                    break;

                case 2:
                    Console.Write("{0}{1}: Makin' the bed", Environment.NewLine,
                                  EntityNames.GetNameOfEntity(entity.ID()));

                    break;
            }
        }

        public override void Exit(MinersWife entity)
        {
        }

        public override bool OnMessage(MinersWife entity, Telegram telegram)
        {
            return false;
        }
    }


    //------------------------------------------------------------------------VisitBathroom

    public class VisitBathroom : State<MinersWife>
    {
        private static VisitBathroom instance;

        private VisitBathroom()
        {
        }

        public static VisitBathroom Instance()
        {
            if (instance == null)
            {
                instance = new VisitBathroom();
            }
            return instance;
        }

        public override void Enter(MinersWife entity)
        {
            Console.Write("{0}{1}: Walkin' to the can. Need to powda mah pretty li'lle nose", Environment.NewLine,
                          EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override void Execute(MinersWife entity)
        {
            Console.Write("{0}{1}: Ahhhhhh! Sweet relief!", Environment.NewLine,
                          EntityNames.GetNameOfEntity(entity.ID()));

            entity.GetFSM().RevertToPreviousState();
        }

        public override void Exit(MinersWife entity)
        {
            Console.Write("{0}{1}: Leavin' the Jon", Environment.NewLine, EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override bool OnMessage(MinersWife entity, Telegram telegram)
        {
            return false;
        }
    }


    //------------------------------------------------------------------------CookStew

    public class CookStew : State<MinersWife>
    {
        private static CookStew instance;

        private CookStew()
        {
        }

        public static CookStew Instance()
        {
            if (instance == null)
            {
                instance = new CookStew();
            }
            return instance;
        }

        public override void Enter(MinersWife entity)
        {
            //if not already cooking put the stew in the oven
            if (!entity.Cooking())
            {
                Console.Write("{0}{1}: Putting the stew in the oven", Environment.NewLine,
                              EntityNames.GetNameOfEntity(entity.ID()));

                //send a delayed message myself so that I know when to take the stew
                //out of the oven
                MessageDispatcher.Instance().DispatchMessage(1.5, //time delay
                                                             entity.ID(), //sender ID
                                                             entity.ID(), //receiver ID
                                                             (int) message_type.Msg_StewReady, //msg
                                                             MessageDispatcher.NO_ADDITIONAL_INFO);

                entity.SetCooking(true);
            }
        }

        public override void Execute(MinersWife entity)
        {
            Console.Write("{0}{1}: Fussin' over food", Environment.NewLine, EntityNames.GetNameOfEntity(entity.ID()));
        }

        public override void Exit(MinersWife entity)
        {
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("{0}{1}: Puttin' the stew on the table", Environment.NewLine,
                          EntityNames.GetNameOfEntity(entity.ID()));
            Console.ForegroundColor = currentColor;
        }

        public override bool OnMessage(MinersWife entity, Telegram telegram)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            switch (telegram.Msg)
            {
                case (int) message_type.Msg_StewReady:
                    {
                        Console.Write("{0}Message received by {1} at time: {2}", Environment.NewLine,
                                      EntityNames.GetNameOfEntity(entity.ID()), DateTime.Now);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("{0}{1}: StewReady! Lets eat", Environment.NewLine,
                                      EntityNames.GetNameOfEntity(entity.ID()));


                        //let hubby know the stew is ready
                        MessageDispatcher.Instance().DispatchMessage(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                                                     entity.ID(),
                                                                     (int) EntityName.ent_Miner_Bob,
                                                                     (int) message_type.Msg_StewReady,
                                                                     MessageDispatcher.NO_ADDITIONAL_INFO);

                        entity.SetCooking(false);

                        entity.GetFSM().ChangeState(DoHouseWork.Instance());
                    }

                    Console.BackgroundColor = currentBackgroundColor;
                    Console.ForegroundColor = currentForegroundColor;
                    return true;
            } //end switch
            Console.BackgroundColor = currentBackgroundColor;
            Console.ForegroundColor = currentForegroundColor;
            return false;
        }
    }
}