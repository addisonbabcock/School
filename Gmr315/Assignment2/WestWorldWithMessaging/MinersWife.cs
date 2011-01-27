#region Using

using System;
using WestWorldWithMessaging.FSM;
using WestWorldWithMessaging.Messaging;

#endregion

namespace WestWorldWithMessaging
{
    public class MinersWife : BaseGameEntity
    {
        //an instance of the state machine class
        private readonly StateMachine<MinersWife> m_pStateMachine;

        //is she presently cooking?
        private bool m_bCooking;
        private location_type m_Location;


        public MinersWife(int id)
            : base(id)
        {
            m_Location = location_type.shack;

            m_bCooking = false;

            //set up the state machine
            m_pStateMachine = new StateMachine<MinersWife>(this);

            m_pStateMachine.SetCurrentState(DoHouseWork.Instance());

            m_pStateMachine.SetGlobalState(WifesGlobalState.Instance());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                m_pStateMachine.Dispose();
            }
        }

        public StateMachine<MinersWife> GetFSM()
        {
            return m_pStateMachine;
        }

        //----------------------------------------------------accessors
        public location_type Location()
        {
            return m_Location;
        }

        public void ChangeLocation(location_type loc)
        {
            m_Location = loc;
        }

        public bool Cooking()
        {
            return m_bCooking;
        }

        public void SetCooking(bool val)
        {
            m_bCooking = val;
        }

        public override bool HandleMessage(Telegram msg)
        {
            return m_pStateMachine.HandleMessage(msg);
        }


        public override void Update()
        {
            //set text color to green
            ConsoleColor currentColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            m_pStateMachine.Update();
            Console.ForegroundColor = currentColor;
        }
    }
}