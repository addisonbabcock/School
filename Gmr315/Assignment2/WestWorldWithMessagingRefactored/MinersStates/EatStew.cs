#region Using

using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;

#endregion

namespace WestWorldWithMessagingRefactored.MinersStates
{
    public class EatStew : State<Miner>
    {
        private static EatStew instance;

        private EatStew()
        {
        }

        public static EatStew Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EatStew();
                }
                return instance;
            }
        }

        public override void Enter(Miner entity)
        {
            entity.OutputStatusMessage("Smells Reaaal goood Elsa!");
        }

        public override void Execute(Miner entity)
        {
            entity.OutputStatusMessage("Tastes real good too!"); 
            entity.GetFSM().RevertToPreviousState();
        }

        public override void Exit(Miner entity)
        {
            entity.OutputStatusMessage("Thankya li'lle lady. Ah better get back to whatever ah wuz doin'");
        }
    }
}