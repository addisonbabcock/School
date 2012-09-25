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
    public class SupportAttacker : State<FieldPlayer>
    {
        private static SupportAttacker instance;
        private readonly ParamLoader Prm = ParamLoader.Instance;

        private SupportAttacker()
        {
        }

        public static SupportAttacker Instance()
        {
            if (instance == null)
            {
                instance = new SupportAttacker();
            }
            return instance;
        }

        public override void Enter(FieldPlayer player)
        {
            player.Steering().ArriveOn();

            player.Steering().SetTarget(player.Team().GetSupportSpot());

            Debug.WriteLine("Player " + player.ID() + " enters support state");

        }

        public override void Execute(FieldPlayer player)
        {
            //if his team loses control go back home
            if (!player.Team().InControl())
            {
                player.GetFSM().ChangeState(Defense.Instance());
                return;
            }


            if (player.HomeRegion().ID() < 6 && !player.isClosestTeamMemberToBall())
            {
                player.Steering().SetTarget((player.Ball().Pos() + player.HomeRegion().Center()) * 0.5f);
            }
            else
            {
                //if the best supporting spot changes, change the steering target
                if (player.Team().GetSupportSpot() != player.Steering().Target())
                {
                    player.Steering().SetTarget(player.Team().GetSupportSpot());

                    player.Steering().ArriveOn();
                }
            }

            //if this player has a shot at the goal AND the attacker can pass
            //the ball to him the attacker should pass the ball to this player
            if (player.Team().CanShoot(player.Pos(),
                                       Prm.MaxShootingForce))
            {
                player.Team().RequestPass(player);
            }


            //if this player is located at the support spot and his team still have
            //possession, he should remain still and turn to face the ball
            if (player.AtTarget())
            {
                player.Steering().ArriveOff();

                player.SetVelocity(new Vector2(0, 0));

                //if not threatened by another player request a pass
                //if (!player.isThreatened())
                //{
                //    player.Team().RequestPass(player);
                //}
            }
        }

        public override void Exit(FieldPlayer player)
        {
            //set supporting player to null so that the team knows it has to 
            //determine a new one.
            player.Team().SetSupportingPlayer(null);

            player.Steering().ArriveOff();
        }

        public override bool OnMessage(FieldPlayer player, Telegram message)
        {
            return false;
        }
    }
}