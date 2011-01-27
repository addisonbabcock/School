using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.Misc;

namespace WestWorldWithMessagingRefactored.MinersWifeStates
{
    public class DoHouseWork : State<MinersWife>
    {
        private static DoHouseWork instance;

        private DoHouseWork()
        {
        }

        public static DoHouseWork Instance
        {
            get {
            if (instance == null)
            {
                instance = new DoHouseWork();
            }
            return instance;
            }
        }

        public override void Enter(MinersWife entity)
        {
            entity.OutputStatusMessage("Time to do some more housework!");
        }

        public override void Execute(MinersWife entity)
        {
            switch (Utils.RandInt(0, 2))
            {
                case 0:
                    entity.OutputStatusMessage("Moppin' the floor");
                    break;

                case 1:
                    entity.OutputStatusMessage("Washin' the dishes");
                    break;

                case 2:
                    entity.OutputStatusMessage("Makin' the bed");
                    break;
            }
        }

        public override void Exit(MinersWife entity)
        {
        }
    }
}
