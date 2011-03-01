using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;

namespace SoccerXNA.Teams.AddisonTeam.GoalKeeperStates
{
    public class TendGoal : State<GoalKeeper>
    {
        private static readonly ParamLoader Prm = ParamLoader.Instance;
        private static TendGoal instance;

        private TendGoal()
        {
        }

        public static TendGoal Instance()
        {
            if (instance == null)
            {
                instance = new TendGoal();
            }
            return instance;
        }

        public override void Enter(GoalKeeper keeper)
        {
            //turn interpose on
            keeper.Steering().InterposeOn(Prm.GoalKeeperTendingDistance);

            //interpose will position the agent between the ball position and a target
            //position situated along the goal mouth. This call sets the target
            keeper.Steering().SetTarget(keeper.GetRearInterposeTarget());
        }

        public override void Execute(GoalKeeper keeper)
        {
            //the rear interpose target will change as the ball's position changes
            //so it must be updated each update-step 
            keeper.Steering().SetTarget(keeper.GetRearInterposeTarget());

            //if the ball comes in range the keeper traps it and then changes state
            //to put the ball back in play
            if (keeper.BallWithinKeeperRange())
            {
                keeper.Ball().Trap();

                keeper.Pitch().SetGoalKeeperHasBall(true);

                keeper.GetFSM().ChangeState(PutBallBackInPlay.Instance());

                return;
            }

            //if ball is within a predefined distance, the keeper moves out from
            //position to try and intercept it.
            if (keeper.BallWithinRangeForIntercept() && !keeper.Team().InControl())
            {
                keeper.GetFSM().ChangeState(InterceptBall.Instance());
            }

            //if the keeper has ventured too far away from the goal-line and there
            //is no threat from the opponents he should move back towards it
            if (keeper.TooFarFromGoalMouth() && keeper.Team().InControl())
            {
                keeper.GetFSM().ChangeState(ReturnHome.Instance());

                return;
            }
        }

        public override void Exit(GoalKeeper keeper)
        {
            keeper.Steering().InterposeOff();
        }

        public override bool OnMessage(GoalKeeper keeper, Telegram message)
        {
            return false;
        }
    }
}