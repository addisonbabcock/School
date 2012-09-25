using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.FSM;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace SoccerXNA.Teams.ThomahawksTeam.FieldPlayerStates
{
    public class Dribble : State<FieldPlayer>
    {
        private static Dribble instance;
        private readonly ParamLoader Prm = ParamLoader.Instance;

        private Dribble()
        {
        }

        public static Dribble Instance()
        {
            if (instance == null)
            {
                instance = new Dribble();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            //let the team know this player is controlling
            player.Team().SetControllingPlayer(player);
            Debug.WriteLine("Player " + player.ID() + " enters dribble state");
        }

        public override void Execute(FieldPlayer player)
        {
            float dot = Vector2.Dot(player.Team().HomeGoal().Facing(), player.Heading());

            //if the ball is between the player and the home goal, it needs to swivel
            // the ball around by doing multiple small kicks and turns until the player 
            //is facing in the correct direction
            if (dot < 0)
            {
                //the player's heading is going to be rotated by a small amount (Pi/4) 
                //and then the ball will be kicked in that direction
                Vector2 direction = player.Heading();

                //calculate the sign (+/-) of the angle between the player heading and the 
                //facing direction of the goal so that the player rotates around in the 
                //correct direction
                float angle = MathHelper.PiOver4 * -1 *
                              player.Team().HomeGoal().Facing().Sign(player.Heading());

                direction = Transformations.Vec2DRotateAroundOrigin(direction, angle);

                //this value works well whjen the player is attempting to control the
                //ball and turn at the same time
                const float KickingForce = 1.2f;

                player.Ball().Kick(direction, KickingForce);
            }

                //kick the ball down the field
            else
            {
                player.Ball().Kick(player.Team().HomeGoal().Facing(),
                                   Prm.MaxDribbleForce);
            }

            //the player has kicked the ball so he must now change state to follow it
            player.GetFSM().ChangeState(ChaseBall.Instance());

            return;
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