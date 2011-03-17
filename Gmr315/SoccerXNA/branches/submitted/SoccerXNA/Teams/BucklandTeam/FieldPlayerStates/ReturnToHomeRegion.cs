using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;

namespace SoccerXNA.Teams.BucklandTeam.FieldPlayerStates
{
    public class ReturnToHomeRegion : State<FieldPlayer>
    {
        private static ReturnToHomeRegion instance;

        private ReturnToHomeRegion()
        {
        }

        public static ReturnToHomeRegion Instance()
        {
            if (instance == null)
            {
                instance = new ReturnToHomeRegion();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            player.Steering().ArriveOn();

            if (!player.HomeRegion().Inside(player.Steering().Target(), Region.region_modifier.halfsize))
            {
                player.Steering().SetTarget(player.HomeRegion().Center());
            }

            Debug.WriteLine("Player " + player.ID() + " enters Return To Home State");
        }

        public override void Execute(FieldPlayer player)
        {
            if (player.Pitch().GameOn())
            {
                //if the ball is nearer this player than any other team member  &&
                //there is not an assigned receiver && the goalkeeper does not gave
                //the ball, go chase it
                if (player.isClosestTeamMemberToBall() &&
                    (player.Team().Receiver() == null) &&
                    !player.Pitch().GoalKeeperHasBall())
                {
                    player.GetFSM().ChangeState(ChaseBall.Instance());

                    return;
                }
            }

            //if game is on and close enough to home, change state to wait and set the 
            //player target to his current position.(so that if he gets jostled out of 
            //position he can move back to it)
            if (player.Pitch().GameOn() && player.HomeRegion().Inside(player.Pos(),
                                                                      Region.region_modifier.halfsize))
            {
                player.Steering().SetTarget(player.Pos());
                player.GetFSM().ChangeState(Wait.Instance());
            }
                //if game is not on the player must return much closer to the center of his
                //home region
            else if (!player.Pitch().GameOn() && player.AtTarget())
            {
                player.GetFSM().ChangeState(Wait.Instance());
            }
        }

        public override void Exit(FieldPlayer player)
        {
            player.Steering().ArriveOff();
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}