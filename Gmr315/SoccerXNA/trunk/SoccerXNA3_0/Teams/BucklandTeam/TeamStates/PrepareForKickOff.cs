using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Common.Messaging;

namespace SoccerXNA.Teams.BucklandTeam.TeamStates
{
    public class PrepareForKickOff : State<AbstractSoccerTeam>
    {
        private static PrepareForKickOff instance;
        private readonly int[] BlueRegions = new[] { 1, 6, 8, 3, 5 };

        private readonly int[] RedRegions = new[] { 16, 9, 11, 12, 14 };

        private PrepareForKickOff()
        {
        }

        public static PrepareForKickOff Instance()
        {
            if (instance == null)
            {
                instance = new PrepareForKickOff();
            }
            return instance;
        }


        public override void Enter(AbstractSoccerTeam team)
        {
            //reset key player pointers
            team.SetControllingPlayer(null);
            team.SetSupportingPlayer(null);
            team.SetReceiver(null);
            team.SetPlayerClosestToBall(null);
            //if (team.Color() == AbstractSoccerTeam.team_color.blue)
            //{
            //    team.ChangePlayerHomeRegions(BlueRegions);
            //}
            //else
            //{
            //    team.ChangePlayerHomeRegions(RedRegions);
            //}

            //send Msg_GoHome to each player.
            team.ReturnAllFieldPlayersToHome();
        }

        public override void Execute(AbstractSoccerTeam team)
        {
            //if both teams in position, start the game
            if (team.AllPlayersAtHome() && team.Opponents().AllPlayersAtHome()
                || (team.AllPlayersAtHome() && team.Pitch().OneTeamReady()))
            {
                team.GetFSM().ChangeState(Defending.Instance());
            }
        }

        public override void Exit(AbstractSoccerTeam team)
        {
            if (team.Color() == AbstractSoccerTeam.team_color.blue)
            {
                team.Pitch().SetBlueTeamReady();
            }
            else
            {
                team.Pitch().SetRedTeamReady();
            }
        }

        public override bool OnMessage(AbstractSoccerTeam team, Telegram message)
        {
            return false;
        }
    }
}