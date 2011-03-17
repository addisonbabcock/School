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
    public class Wait : State<FieldPlayer>
    {
        private static Wait instance;

        private Wait()
        {
        }

        public static Wait Instance()
        {
            if (instance == null)
            {
                instance = new Wait();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            Debug.WriteLine("Player " + player.ID() + " enters wait state");

            //if the game is not on make sure the target is the center of the player's
            //home region. This is ensure all the players are in the correct positions
            //ready for kick off
            if (!player.Pitch().GameOn())
            {
                player.Steering().SetTarget(player.HomeRegion().Center());
            }
        }

        public override void Execute(FieldPlayer player)
        {
            //if the player has been jostled out of position, get back in position  
            if (!player.AtTarget())
            {
                player.Steering().ArriveOn();

                return;
            }

            player.Steering().ArriveOff();

            player.SetVelocity(new Vector2(0, 0));

            //the player should keep his eyes on the ball!
            player.TrackBall();

            if (player.Pitch().GameOn())
            {
                //if the ball is nearer this player than any other team member  AND
                //there is not an assigned receiver AND neither goalkeeper has
                //the ball, go chase it
                if (player.isClosestTeamMemberToBall() &&
                    player.Team().Receiver() == null &&
                    !player.Pitch().GoalKeeperHasBall())
                {
                    player.GetFSM().ChangeState(ChaseBall.Instance());
                }
                else if (player.Team().InControl())
                {
                    player.GetFSM().ChangeState(SupportAttacker.Instance());
                }
                else
                {
                    player.GetFSM().ChangeState(Defense.Instance());
                }
            }
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