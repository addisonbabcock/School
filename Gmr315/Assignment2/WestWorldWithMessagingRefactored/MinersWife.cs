#region Using

using System;
using WestWorldWithMessagingRefactored.FSM;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.MinersWifeStates;

#endregion

namespace WestWorldWithMessagingRefactored
{
    public class MinersWife : BaseGameEntity
    {
        //an instance of the state machine class
        private readonly StateMachine<MinersWife> stateMachine;

        //is she presently cooking?
        private Location location;


        public MinersWife(int id)
            : base(id)
        {
            location = WestWorldWithMessagingRefactored.Location.Shack;

            Cooking = false;

            //set up the state machine
            stateMachine = new StateMachine<MinersWife>(this, DoHouseWork.Instance, WifesGlobalState.Instance);
            OutputMessageColor = ConsoleColor.Green;
        }

      

        public StateMachine<MinersWife> GetFSM()
        {
            return stateMachine;
        }

        public Location Location()
        {
            return location;
        }

        public void ChangeLocation(Location newLocation)
        {
            location = newLocation;
        }

        public bool Cooking { get; set; }

        public override bool HandleMessage(Message message)
        {
            return GetFSM().HandleMessage(message);
        }


        public override void Update()
        {
            GetFSM().Update();
        }
    }
}