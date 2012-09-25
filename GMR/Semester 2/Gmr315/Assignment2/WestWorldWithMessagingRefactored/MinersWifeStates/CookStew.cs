#region Using

using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

#endregion

namespace WestWorldWithMessagingRefactored.MinersWifeStates
{
    public class CookStew : State<MinersWife>
    {
        private static double MinimumCookingTime = 5;
        private static CookStew instance;

        private CookStew()
        {
        }

        public static CookStew Instance
        {get {
            if (instance == null)
            {
                instance = new CookStew();
            }
            return instance;
        }
        }

        public override void Enter(MinersWife entity)
        {
            //if not already cooking put the stew in the oven
            if (!entity.Cooking)
            {
                entity.OutputStatusMessage("Putting the stew in the oven");
                //send a delayed message myself so that I know when to take the stew
                //out of the oven
                MessageDispatcher.Instance.DispatchMessage(MinimumCookingTime, //time delay
                                                           entity.ID, //sender ID
                                                           entity.ID, //receiver ID
                                                           MessageType.StewReady, //msg
                                                           MessageDispatcher.NoAdditionalInfo);

                entity.Cooking = true;
            }
        }

        public override void Execute(MinersWife entity)
        {
            entity.OutputStatusMessage("Fussin' over food");
        }

        public override void Exit(MinersWife entity)
        {
            entity.OutputStatusMessage("Puttin' the stew on the table");
        }

        public override bool OnMessage(MinersWife entity, Message telegram)
        {
            switch (telegram.MessageType)
            {
                case MessageType.StewReady:
                    {
                        MessageDispatcher.OutputHandledMessage(entity.ID);
                        entity.OutputStatusMessage("Stew is ready! Lets eat"); 

                        //let hubby know the stew is ready
                        MessageDispatcher.Instance.DispatchMessage(MessageDispatcher.SendMessageImmediately,
                                                                   entity.ID,
                                                                   (int) EntityName.MinerBob,
                                                                   MessageType.StewReady,
                                                                   MessageDispatcher.NoAdditionalInfo);

                        entity.Cooking = false;

                        entity.GetFSM().ChangeState(DoHouseWork.Instance);
                    }

                    return true;
            } //end switch
            return false;
        }
    }
}