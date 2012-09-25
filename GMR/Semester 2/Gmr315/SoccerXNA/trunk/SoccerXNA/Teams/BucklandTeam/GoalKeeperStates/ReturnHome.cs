using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;

namespace SoccerXNA.Teams.BucklandTeam.GoalKeeperStates
{
    public class ReturnHome : State<GoalKeeper>
    {
        private static ReturnHome instance;

        private ReturnHome()
        {
        }

        public static ReturnHome Instance()
        {
            if (instance == null)
            {
                instance = new ReturnHome();
            }
            return instance;
        }

        public override void Enter(GoalKeeper keeper)
        {
            keeper.Steering().InterposeOff();
        }

        public override void Execute(GoalKeeper keeper)
        {
            keeper.Steering().SetTarget(keeper.HomeRegion().Center());

            //if close enough to home or the opponents get control over the ball,
            //change state to tend goal
            if (keeper.InHomeRegion() || !keeper.Team().InControl())
            {
                keeper.GetFSM().ChangeState(TendGoal.Instance());
            }
        }

        public override void Exit(GoalKeeper keeper)
        {
            keeper.Steering().ArriveOff();
        }

        public override bool OnMessage(GoalKeeper keeper, Telegram message)
        {
            return false;
        }
    }
}