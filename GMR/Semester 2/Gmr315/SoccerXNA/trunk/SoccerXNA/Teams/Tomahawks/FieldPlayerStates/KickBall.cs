using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams.ThomahawksTeam.FieldPlayerStates
{
    public class KickBall : State<FieldPlayer>
    {
        private static KickBall instance;
        private readonly MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        private readonly ParamLoader Prm = ParamLoader.Instance;

        private KickBall()
        {
        }

        public static KickBall Instance()
        {
            if (instance == null)
            {
                instance = new KickBall();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            //let the team know this player is controlling
            player.Team().SetControllingPlayer(player);

            //the player can only make so many kick attempts per second.
            if (!player.isReadyForNextKick())
            {
                player.GetFSM().ChangeState(ChaseBall.Instance());
            }

            Debug.WriteLine("Player " + player.ID() + " enters Kick state");
        }

        public bool TryShot(FieldPlayer player, float dot)
        {
            //if a shot is possible, this vector will hold the position along the 
            //opponent's goal line the player should aim for.
            Vector2 BallTarget = new Vector2();

            //the dot product is used to adjust the shooting force. The more
            //directly the ball is ahead, the more forceful the kick
            float power = Prm.MaxShootingForce * dot;

            //if it is determined that the player could score a goal from this position
            //OR if he should just kick the ball anyway, the player will attempt
            //to make the shot
            if (player.Team().CanShoot(player.Ball().Pos(),
                                       power,
                                       ref BallTarget) ||
                (Utils.RandFloat() < Prm.ChancePlayerAttemptsPotShot))
            {
                Debug.WriteLine("Player " + player.ID() + " attempts a shot at " + BallTarget);

                //add some noise to the kick. We don't want players who are 
                //too accurate! The amount of noise can be adjusted by altering
                //Prm.PlayerKickingAccuracy
                //  BallTarget = SoccerBall.AddNoiseToKick(player.Ball().Pos(), BallTarget);

                //this is the direction the ball will be kicked in
                Vector2 KickDirection = BallTarget - player.Ball().Pos();

                player.Ball().Kick(KickDirection, power);

                //change state   
                player.GetFSM().ChangeState(Defense.Instance());

                player.FindSupport();

                return true;
            }

            return false;
        }

        public bool TryPass(FieldPlayer player, float dot)
        {
            //if a shot is possible, this vector will hold the position along the 
            //opponent's goal line the player should aim for.
            Vector2 BallTarget = new Vector2();

            //if a receiver is found this will point to it
            PlayerBase receiver = null;

            float power = Prm.MaxPassingForce * dot;

            //test if there are any potential candidates available to receive a pass
            if (player.isThreatened() &&
                player.Team().FindPass(player,
                                       ref receiver,
                                       ref BallTarget,
                                       power,
                                       Prm.MinPassDist))
            {
                //add some noise to the kick
                // BallTarget = SoccerBall.AddNoiseToKick(player.Ball().Pos(), BallTarget);

                Vector2 KickDirection = BallTarget - player.Ball().Pos();

                player.Ball().Kick(KickDirection, power);

                Debug.WriteLine("Player " + player.ID() + " passes the ball with force " + power + " to player " + receiver.ID() + ".  Target is " + BallTarget);

                //let the receiver know a pass is coming 
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                       player.ID(),
                                       receiver.ID(),
                                       (int)SoccerMessages.Msg_ReceiveBall,
                                       BallTarget);


                //the player should wait at his current position unless instruced
                //otherwise  
                player.GetFSM().ChangeState(Defense.Instance());

                player.FindSupport();

                return true;
            }

            return false;
        }

        public void TryDribble()
        {
        }

        public override void Execute(FieldPlayer player)
        {
            //calculate the dot product of the vector pointing to the ball
            //and the player's heading
            Vector2 ToBall = player.Ball().Pos() - player.Pos();
            float dot = Vector2.Dot(player.Heading(), Vector2.Normalize(ToBall));

            //cannot kick the ball if the goalkeeper is in possession or if it is 
            //behind the player or if there is already an assigned receiver. So just
            //continue chasing the ball
            if (player.Team().Receiver() != null ||
                player.Pitch().GoalKeeperHasBall() ||
                (dot < 0))
            {
                Debug.WriteLine("Goalie has ball / ball behind player");

                player.GetFSM().ChangeState(ChaseBall.Instance());

                return;
            }

            /* Attempt a shot at the goal */

            if (TryShot(player, dot))
            {
                return;
            }

            /* Attempt a pass to a player */
            if (TryPass(player, dot))
            {
                return;
            }

            //cannot shoot or pass, so dribble the ball upfield

            player.FindSupport();

            player.GetFSM().ChangeState(Dribble.Instance());
        }

        public override void Exit(FieldPlayer player)
        {
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}