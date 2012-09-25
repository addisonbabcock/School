using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams.BucklandTeam.GoalKeeperStates
{
    public class PutBallBackInPlay : State<GoalKeeper>
    {
        private static PutBallBackInPlay instance;

        private PutBallBackInPlay()
        {
        }

        public static PutBallBackInPlay Instance()
        {
            if (instance == null)
            {
                instance = new PutBallBackInPlay();
            }
            return instance;
        }

        public override void Enter(GoalKeeper keeper)
        {
            //let the team know that the keeper is in control
            keeper.Team().SetControllingPlayer(keeper);

            //send all the players home
            keeper.Team().Opponents().ReturnAllFieldPlayersToHome();
            keeper.Team().ReturnAllFieldPlayersToHome();
        }

        public override void Execute(GoalKeeper keeper)
        {
            ParamLoader Prm = ParamLoader.Instance;
            MessageDispatcher Dispatcher = MessageDispatcher.Instance();
            PlayerBase receiver = null;
            Vector2 BallTarget = new Vector2();

            //test if there are players further forward on the field we might
            //be able to pass to. If so, make a pass.
            if (keeper.Team().FindPass(keeper, ref receiver,
                                       ref BallTarget,
                                       Prm.MaxPassingForce,
                                       Prm.GoalkeeperMinPassDist))
            {
                //make the pass   
                keeper.Ball().Kick(Vector2.Normalize(BallTarget - keeper.Ball().Pos()),
                                   Prm.MaxPassingForce);

                //goalkeeper no longer has ball 
                keeper.Pitch().SetGoalKeeperHasBall(false);

                //let the receiving player know the ball's comin' at him
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                       keeper.ID(),
                                       receiver.ID(),
                                       (int)SoccerMessages.Msg_ReceiveBall,
                                       BallTarget);

                //go back to tending the goal   
                keeper.GetFSM().ChangeState(TendGoal.Instance());

                return;
            }

            keeper.SetVelocity(new Vector2());
        }

        public override void Exit(GoalKeeper keeper)
        {
        }

        public override bool OnMessage(GoalKeeper keeper, Telegram message)
        {
            return false;
        }
    }
}