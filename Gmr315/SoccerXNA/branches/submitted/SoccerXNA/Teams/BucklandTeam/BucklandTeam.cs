using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.FSM;
using Microsoft.Xna.Framework;
using SoccerXNA.Teams.BucklandTeam.FieldPlayerStates;
using SoccerXNA.Teams.BucklandTeam.GoalKeeperStates;
using SoccerXNA.Teams.BucklandTeam.TeamStates;

namespace SoccerXNA.Teams.BucklandTeam
{
    public class BucklandTeam : AbstractSoccerTeam, ITeam
    {
        public BucklandTeam(Game game,
            Goal home_goal,
                       Goal opponents_goal,
                       SoccerPitch pitch,
                       team_color color)
            : base(game, home_goal, opponents_goal, pitch, color)
        {
            InitStateMachine();
            CreatePlayers();
            RegisterPlayers();
            InitPlayers();

            //create the sweet spot calculator
            m_pSupportSpotCalc = new SupportSpotCalculator(game,
                Prm.NumSupportSpotsX,
                                                           Prm.NumSupportSpotsY,
                                                           this);
        }
        public override void InitStateMachine()
        {
            m_pStateMachine = new StateMachine<AbstractSoccerTeam>(this);

            m_pStateMachine.SetCurrentState(Defending.Instance());
            m_pStateMachine.SetPreviousState(Defending.Instance());
            m_pStateMachine.SetGlobalState(null);
        }

        public override void InitPlayers()
        {
            foreach (PlayerBase it in m_Players)
            {
                it.Steering().SeparationOn();
            }
        }



        //------------------------- CreatePlayers --------------------------------
        //
        //  creates the players
        //------------------------------------------------------------------------
        public override void CreatePlayers()
        {
            m_Players = new List<PlayerBase>();
            if (Color() == team_color.blue)
            {
                //goalkeeper
                m_Players.Add(new GoalKeeper(Game, this,
                                           1,
                                           TendGoal.Instance(),
                                           GlobalKeeperState.Instance(),
                                           new Vector2(0, 1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale));

                //create the players
                m_Players.Add(new FieldPlayer(Game, this,
                                           6,
                                           Wait.Instance(),
                                           GlobalPlayerState.Instance(),
                                           new Vector2(0, 1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale,
                                           PlayerBase.player_role.attacker));



                m_Players.Add(new FieldPlayer(Game, this,
                                       8,
                                       Wait.Instance(),
                                       GlobalPlayerState.Instance(),
                                       new Vector2(0, 1),
                                       new Vector2(0.0f, 0.0f),
                                       Prm.PlayerMass,
                                       Prm.PlayerMaxForce,
                                       Prm.PlayerMaxSpeedWithoutBall,
                                       Prm.PlayerMaxTurnRate,
                                       Prm.PlayerScale,
                                       PlayerBase.player_role.attacker));





                m_Players.Add(new FieldPlayer(Game, this,
                                       3,
                                       Wait.Instance(),
                                       GlobalPlayerState.Instance(),
                                       new Vector2(0, 1),
                                       new Vector2(0.0f, 0.0f),
                                       Prm.PlayerMass,
                                       Prm.PlayerMaxForce,
                                       Prm.PlayerMaxSpeedWithoutBall,
                                       Prm.PlayerMaxTurnRate,
                                       Prm.PlayerScale,
                                       PlayerBase.player_role.defender));


                m_Players.Add(new FieldPlayer(Game, this,
                                       5,
                                       Wait.Instance(),
                                       GlobalPlayerState.Instance(),
                                       new Vector2(0, 1),
                                       new Vector2(0.0f, 0.0f),
                                       Prm.PlayerMass,
                                       Prm.PlayerMaxForce,
                                       Prm.PlayerMaxSpeedWithoutBall,
                                       Prm.PlayerMaxTurnRate,
                                       Prm.PlayerScale,
                                      PlayerBase.player_role.defender));

            }

            else
            {

                //goalkeeper
                m_Players.Add(new GoalKeeper(Game, this,
                                           16,
                                           TendGoal.Instance(),
                                           GlobalKeeperState.Instance(),
                                           new Vector2(0, -1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale));


                //create the players
                m_Players.Add(new FieldPlayer(Game, this,
                                           9,
                                           Wait.Instance(),
                                           GlobalPlayerState.Instance(),
                                           new Vector2(0, -1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale,
                                           PlayerBase.player_role.attacker));

                m_Players.Add(new FieldPlayer(Game, this,
                                           11,
                                           Wait.Instance(),
                                           GlobalPlayerState.Instance(),
                                           new Vector2(0, -1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale,
                                           PlayerBase.player_role.attacker));



                m_Players.Add(new FieldPlayer(Game, this,
                                           12,
                                           Wait.Instance(),
                                           GlobalPlayerState.Instance(),
                                           new Vector2(0, -1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale,
                                           PlayerBase.player_role.defender));


                m_Players.Add(new FieldPlayer(Game, this,
                                           14,
                                           Wait.Instance(),
                                           GlobalPlayerState.Instance(),
                                           new Vector2(0, -1),
                                           new Vector2(0.0f, 0.0f),
                                           Prm.PlayerMass,
                                           Prm.PlayerMaxForce,
                                           Prm.PlayerMaxSpeedWithoutBall,
                                           Prm.PlayerMaxTurnRate,
                                           Prm.PlayerScale,
                                           PlayerBase.player_role.defender));

            }
        }

        public override void PrepareForKickoff()
        {
            GetFSM().ChangeState(PrepareForKickOff.Instance());
        }

        public override string Name()
        {
            return "Buckland";
        }

        //---------------------- UpdateTargetsOfWaitingPlayers ------------------------
        //
        //  
        public override void UpdateTargetsOfWaitingPlayers()
        {
            foreach (PlayerBase it in m_Players)
            {
                if (it.Role() != PlayerBase.player_role.goal_keeper)
                {
                    FieldPlayer plyr = it as FieldPlayer;
                    if (plyr.GetFSM().isInState(Wait.Instance()) || (plyr.GetFSM().isInState(ReturnToHomeRegion.Instance())))
                    {
                        plyr.Steering().SetTarget(plyr.HomeRegion().Center());
                    }
                }
            }

        }
    }
}
