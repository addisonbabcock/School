#region Using

using System;
using WestWorldWithWoman.FSM;
using WestWorldWithWoman.Misc;

#endregion

namespace WestWorldWithWoman
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
            if (Utils.RandFloat() < 0.1)
            {
                entity.GetFSM().ChangeState(VisitBathroom.Instance());
            }
        }

        public override void Exit(MinersWife entity)
        {
        }
    }

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
    }

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
    }
}