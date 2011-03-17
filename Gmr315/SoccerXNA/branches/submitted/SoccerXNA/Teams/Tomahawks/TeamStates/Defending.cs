using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;
using Common.Misc;

namespace SoccerXNA.Teams.ThomahawksTeam.TeamStates
{
    public class Defending : State<AbstractSoccerTeam>
    {
        private readonly int[] BlueRegions = new[] { 1, 6, 8, 3, 5 };

        private readonly int[] RedRegions = new[] { 16, 9, 11, 12, 14 };
        private static Defending instance;

        private Defending()
        {
        }

       
        public static Defending Instance()
        {
            if (instance == null)
            {
                instance = new Defending();
            }
            return instance;
        }

        public override void Enter(AbstractSoccerTeam team)
        {
            Debug.WriteLine(team.Name() + " enters defending state");

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
            //steering target must be updated to that of its new home region
            team.UpdateTargetsOfWaitingPlayers();
        }

        public override void Execute(AbstractSoccerTeam team)
        {
            //if in control change states
            if (team.InControl())
            {
                team.GetFSM().ChangeState(Attacking.Instance());
                return;
            }
        }

        public override void Exit(AbstractSoccerTeam team)
        {
        }

        public override bool OnMessage(AbstractSoccerTeam team, Telegram message)
        {
            return false;
        }
    }
}