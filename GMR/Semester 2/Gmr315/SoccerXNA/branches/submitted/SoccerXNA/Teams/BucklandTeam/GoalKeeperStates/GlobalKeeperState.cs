using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;

namespace SoccerXNA.Teams.BucklandTeam.GoalKeeperStates
{
    public class GlobalKeeperState : State<GoalKeeper>
    {
        private static GlobalKeeperState instance;

        private GlobalKeeperState()
        {
        }

        public static GlobalKeeperState Instance()
        {
            if (instance == null)
            {
                instance = new GlobalKeeperState();
            }
            return instance;
        }

        public override void Enter(GoalKeeper keeper)
        {
        }

        public override void Execute(GoalKeeper keeper)
        {
        }

        public override void Exit(GoalKeeper keeper)
        {
        }

        public override bool OnMessage(GoalKeeper keeper, Telegram telegram)
        {
            switch (telegram.Msg)
            {
                case (int)SoccerMessages.Msg_GoHome:
                    {
                        keeper.SetDefaultHomeRegion();

                        keeper.GetFSM().ChangeState(ReturnHome.Instance());
                    }

                    break;

                case (int)SoccerMessages.Msg_ReceiveBall:
                    {
                        keeper.GetFSM().ChangeState(InterceptBall.Instance());
                    }

                    break;
            } //end switch

            return false;
        }
    }
}