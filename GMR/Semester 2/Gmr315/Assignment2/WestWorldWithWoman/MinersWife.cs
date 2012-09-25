#region Using

using System;
using WestWorldWithWoman.FSM;

#endregion

namespace WestWorldWithWoman
{
    public class MinersWife : BaseGameEntity
    {
        //an instance of the state machine class
        private readonly StateMachine<MinersWife> m_pStateMachine;

        private location_type m_Location;


        public MinersWife(int id)
            : base(id)
        {
            m_Location = location_type.shack;

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