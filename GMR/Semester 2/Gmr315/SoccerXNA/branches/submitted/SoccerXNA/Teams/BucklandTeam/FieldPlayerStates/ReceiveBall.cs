using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams.BucklandTeam.FieldPlayerStates
{
    public class ReceiveBall : State<FieldPlayer>
    {
        private static ReceiveBall instance;
        private readonly ParamLoader Prm = ParamLoader.Instance;

        private ReceiveBall()
        {
        }

        public static ReceiveBall Instance()
        {
            if (instance == null)
            {
                instance = new ReceiveBall();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            //let the team know this player is receiving the ball
            player.Team().SetReceiver(player);

            //this player is also now the controlling player
            player.Team().SetControllingPlayer(player);

            //there are two types of receive behavior. One uses arrive to direct
            //the receiver to the position sent by the passer in its telegram. The
            //other uses the pursuit behavior to pursue the ball. 
            //This statement selects between them dependent on the probability
            //ChanceOfUsingArriveTypeReceiveBehavior, whether or not an opposing
            //player is close to the receiving player, and whether or not the receiving
            //player is in the opponents 'hot region' (the third of the pitch closest
            //to the opponent's goal
            const float PassThreatRadius = 70.0f;

            if ((player.InHotRegion() ||
                 Utils.RandFloat() < Prm.ChanceOfUsingArriveTypeReceiveBehavior) &&
                !player.Team().isOpponentWithinRadius(player.Pos(), PassThreatRadius))
            {
                player.Steering().ArriveOn();

                Debug.WriteLine("Player " + player.ID() + " enters receive state (Using Arrive)");

            }
            else
            {
                player.Steering().PursuitOn();

                Debug.WriteLine("Player " + player.ID() + " enters receive state (Using Pursuit)");
            }
        }

        public override void Execute(FieldPlayer player)
        {
            //if the ball comes close enough to the player or if his team lose control
            //he should change state to chase the ball
            if (player.BallWithinReceivingRange() || !player.Team().InControl())
            {
                player.GetFSM().ChangeState(ChaseBall.Instance());

                return;
            }

            if (player.Steering().PursuitIsOn())
            {
                player.Steering().SetTarget(player.Ball().Pos());
            }

            //if the player has 'arrived' at the steering target he should wait and
            //turn to face the ball
            if (player.AtTarget())
            {
                player.Steering().ArriveOff();
                player.Steering().PursuitOff();
                player.TrackBall();
                player.SetVelocity(new Vector2(0, 0));
            }
        }

        public override void Exit(FieldPlayer player)
        {
            player.Steering().ArriveOff();
            player.Steering().PursuitOff();

            player.Team().SetReceiver(null);
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}