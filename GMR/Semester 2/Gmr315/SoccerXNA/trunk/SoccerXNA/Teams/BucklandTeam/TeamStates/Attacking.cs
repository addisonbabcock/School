using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;

namespace SoccerXNA.Teams.BucklandTeam.TeamStates
{
    public class Attacking : State<AbstractSoccerTeam>
    {
        private static Attacking instance;

        private readonly int[] BlueRegions = new[] { 1, 12, 14, 6, 4 };
        private readonly int[] RedRegions = new[] { 16, 3, 5, 9, 13 };


        private Attacking()
        {
        }

        public static Attacking Instance()
        {
            if (instance == null)
            {
                instance = new Attacking();
            }
            return instance;
        }

       

        public override void Enter(AbstractSoccerTeam team)
        {
            Debug.WriteLine(team.Name() + " enters Attacking state");

            //these define the home regions for this state of each of the players


            //set up the player's home regions
            if (team.Color() == AbstractSoccerTeam.team_color.blue)
            {
                team.ChangePlayerHomeRegions(BlueRegions);
            }
            else
            {
                team.ChangePlayerHomeRegions(RedRegions);
            }

            //if a player is in either the Wait or ReturnToHomeRegion states, its
            //steering target must be updated to that of its new home region to enable
            //it to move into the correct position.
            team.UpdateTargetsOfWaitingPlayers();
        }

        public override void Execute(AbstractSoccerTeam team)
        {
            //if this team is no longer in control change states
            if (!team.InControl())
            {
                team.GetFSM().ChangeState(Defending.Instance());
                return;
            }

            //calculate the best position for any supporting attacker to move to
            team.DetermineBestSupportingPosition();
        }

        public override void Exit(AbstractSoccerTeam team)
        {
            //there is no supporting player for defense
            team.SetSupportingPlayer(null);
        }

        public override bool OnMessage(AbstractSoccerTeam team, Telegram message)
        {
            return false;
        }
    }
}