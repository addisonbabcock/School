using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using SoccerXNA.Teams.ThomahawksTeam.TeamStates;

namespace SoccerXNA.Teams.ThomahawksTeam.FieldPlayerStates
{
    public class Defense : State<FieldPlayer>
    {
        private static Defense instance;

        private Defense()
        {
        }

        public static Defense Instance()
        {
            if (instance == null)
            {
                instance = new Defense();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            Debug.WriteLine("Player " + player.ID() + " enters wait state");

            player.Steering().PursuitOff();
            player.Steering().ArriveOn();

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
            //if this player's team is controlling AND this player is not the attacker
            //AND is further up the field than the attacker he should request a pass.
            if (player.Team().InControl())
            {
                if (player.isClosestTeamMemberToBall())
                {
                    player.GetFSM().ChangeState(ChaseBall.Instance());
                }
                else
                {
                    player.GetFSM().ChangeState(SupportAttacker.Instance());
                }
            }
            else
            {
                //if the ball is nearer this player than any other team member  AND
                //there is not an assigned receiver AND neither goalkeeper has
                //the ball, go chase it
                if (player.isClosestTeamMemberToBall() &&
                    player.Team().Receiver() == null &&
                    !player.Pitch().GoalKeeperHasBall())
                {
                    player.GetFSM().ChangeState(ChaseBall.Instance());

                    return;
                }
                else
                {
                    //Vector2 vec = player.Team().HomeGoal().Center() - player.Ball().Pos();
                    //player.Steering().SetTarget(player.Ball().Pos() + (vec * 0.5f));
                    //player.Steering().SetTarget(player.Team().HomeGoal().Center());

                    //if (player.HomeRegion().ID() < 6 && !player.isClosestTeamMemberToBall())
                    //{
                    player.Steering().SetTarget((player.Ball().Pos() + player.HomeRegion().Center()) * 0.5f);
                    //}

                }
                return;
            }

        }

        public override void Exit(FieldPlayer player)
        {
            player.Steering().PursuitOff();
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}