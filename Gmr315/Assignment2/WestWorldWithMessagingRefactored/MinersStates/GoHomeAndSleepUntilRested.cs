using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

namespace WestWorldWithMessagingRefactored.MinersStates
{
    public class GoHomeAndSleepTilRested : State<Miner>
    {
        private static GoHomeAndSleepTilRested instance;

        private GoHomeAndSleepTilRested()
        {
        }

        public static GoHomeAndSleepTilRested Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GoHomeAndSleepTilRested();
                }

                return instance;
            }
        }

        public override void Enter(Miner miner)
        {
            if (miner.Location() != Location.Shack)
            {
                miner.OutputStatusMessage("Walkin' home");
                miner.ChangeLocation(Location.Shack);
                MessageDispatcher.Instance.DispatchMessage(MessageDispatcher.SendMessageImmediately,
                                                             miner.ID, (int)EntityName.Elsa,
                                                             MessageType.HiHoneyImHome,
                                                             MessageDispatcher.NoAdditionalInfo);
            }
        }

        public override void Execute(Miner miner)
        {
            if (!miner.Fatigued())
            {
                miner.OutputStatusMessage("All mah fatigue has drained away. Time to find more gold!");
                miner.GetFSM().ChangeState(EnterMineAndDigForNugget.Instance);
            }

            else
            {
                miner.DecreaseFatigue();
                miner.OutputStatusMessage("ZZZZZ....");
            }
        }

        public override void Exit(Miner pMiner)
        {
        }

        public override bool OnMessage(Miner entity, Message message)
        {
            
            switch (message.MessageType)
            {
                case MessageType.StewReady:
                    MessageDispatcher.OutputHandledMessage(entity.ID);
                    entity.OutputStatusMessage("Okay Hun, ahm a comin'!");
                    entity.GetFSM().ChangeState(EatStew.Instance);

                    return true;
            }
            return false;
        }
    }
}
