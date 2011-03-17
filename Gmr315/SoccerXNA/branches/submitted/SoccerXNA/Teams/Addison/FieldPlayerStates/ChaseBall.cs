using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;

namespace SoccerXNA.Teams.AddisonTeam.FieldPlayerStates
{
    public class ChaseBall : State<FieldPlayer>
    {
        private static ChaseBall instance;

        private ChaseBall()
        {
        }

        public static ChaseBall Instance()
        {
            if (instance == null)
            {
                instance = new ChaseBall();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            player.Steering().SeekOn();
            Debug.WriteLine("Player " + player.ID() + " enters chase state");
        }

        public override void Execute(FieldPlayer player)
        {
            //if the ball is within kicking range the player changes state to KickBall.
            if (player.BallWithinKickingRange())
            {
                player.GetFSM().ChangeState(KickBall.Instance());

                return;
            }

            //if the player is the closest player to the ball then he should keep
            //chasing it
            if (player.isClosestTeamMemberToBall())
            {
                player.Steering().SetTarget(player.Ball().Pos());

                return;
            }

            //if the player is not closest to the ball anymore, he should return back
            //to his home region and wait for another opportunity
            player.GetFSM().ChangeState(ReturnToHomeRegion.Instance());
        }

        public override void Exit(FieldPlayer player)
        {
            player.Steering().SeekOff();
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}