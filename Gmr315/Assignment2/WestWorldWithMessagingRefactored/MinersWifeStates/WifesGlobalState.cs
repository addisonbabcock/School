using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.Misc;

namespace WestWorldWithMessagingRefactored.MinersWifeStates
{
    public class WifesGlobalState : State<MinersWife>
    {
        private static WifesGlobalState instance;

        private WifesGlobalState()
        {
        }

        public static WifesGlobalState Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WifesGlobalState();
                }
                return instance;
            }
        }

        public override void Enter(MinersWife entity)
        {
        }

        public override void Execute(MinersWife entity)
        {
            //1 in 10 chance of needing the bathroom (provided she is not already
            //in the bathroom)
            if ((Utils.RandFloat() < 0.1) &&
                !entity.GetFSM().IsInState(VisitBathroom.Instance))
            {
                entity.GetFSM().ChangeState(VisitBathroom.Instance);
            }
        }

        public override void Exit(MinersWife entity)
        {
        }

        public override bool OnMessage(MinersWife entity, Message telegram)
        {
            switch (telegram.MessageType)
            {
                case MessageType.HiHoneyImHome:
                    {
                        MessageDispatcher.OutputHandledMessage(entity.ID);
                        entity.OutputStatusMessage("Hi honey. Let me make you some of mah fine country stew");
                        entity.GetFSM().ChangeState(CookStew.Instance);
                    }
                    return true;
            } //end switch
            return false;
        }
    }
}