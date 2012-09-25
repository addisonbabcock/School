using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.MinersWifeStates
{
    public class VisitBathroom : State<MinersWife>
    {
        private static VisitBathroom instance;

        private VisitBathroom()
        {
        }

        public static VisitBathroom Instance
        { get{
            if (instance == null)
            {
                instance = new VisitBathroom();
            }
            return instance;
        }
        }

        public override void Enter(MinersWife entity)
        {
            entity.OutputStatusMessage("Walkin' to the can. Need to powda mah pretty li'lle nose");
        }

        public override void Execute(MinersWife entity)
        {
            entity.OutputStatusMessage("Ahhhhhh! Sweet relief!");

            entity.GetFSM().RevertToPreviousState();
        }

        public override void Exit(MinersWife entity)
        {
            entity.OutputStatusMessage("Leavin' the Jon");
        }
    }
}
