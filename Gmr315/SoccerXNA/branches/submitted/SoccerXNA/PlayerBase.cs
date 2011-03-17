#region Using

using System;
using System.Collections.Generic;
using Common.Game;
using Common.Messaging;
using Microsoft.Xna.Framework;

#endregion

namespace SoccerXNA
{
    public abstract class PlayerBase : MovingEntity, IEntity
    {
        /* Autolist functionality added to this class as C# cannot do this */

        #region player_role enum

        public enum player_role
        {
            goal_keeper,
            attacker,
            defender
        } ;

        #endregion

        private static readonly List<PlayerBase> members = new List<PlayerBase>();

        private static readonly Vector2[] player = new[]
                                                       {
                                                           new Vector2(-3, 8),
                                                           new Vector2(3, 10),
                                                           new Vector2(3, -10),
                                                           new Vector2(-3, -8)
                                                       };

        private readonly MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        private readonly ParamLoader Prm = ParamLoader.Instance;
        protected float m_dDistSqToBall;
        protected int m_iDefaultRegion;
        protected int m_iHomeRegion;


        //this player's role in the team
        protected player_role m_PlayerRole;

        //a pointer to this player's team

        //the steering behaviors
        protected SteeringBehaviors m_pSteering;
        protected AbstractSoccerTeam m_pTeam;

        //the region that this player is assigned to.


        //the vertex buffer
        protected List<Vector2> m_vecPlayerVB;
        //the buffer for the transformed vertices
        protected List<Vector2> m_vecPlayerVBTrans;


        public PlayerBase(Game game, AbstractSoccerTeam home_team,
                          int home_region,
                          Vector2 heading,
                          Vector2 velocity,
                          float mass,
                          float max_force,
                          float max_speed,
                          float max_turn_rate,
                          float scale,
                          player_role role)
            : base(game, home_team.Pitch().GetRegionFromIndex(home_region).Center(),
                   scale*10, velocity, max_speed, heading, mass,
                   new Vector2(scale, scale), max_turn_rate, max_force)
        {
            m_vecPlayerVB = new List<Vector2>();
            m_pTeam = home_team;
            m_dDistSqToBall = float.MaxValue;
            m_iHomeRegion = home_region;
            m_iDefaultRegion = home_region;
            m_PlayerRole = role;
            //setup the vertex buffers and calculate the bounding radius
            const int NumPlayerVerts = 4;

            for (int vtx = 0; vtx < NumPlayerVerts; ++vtx)
            {
                m_vecPlayerVB.Add(player[vtx]);

                //set the bounding radius to the length of the 
                //greatest extent
                if (Math.Abs(player[vtx].X) > m_dBoundingRadius)
                {
                    m_dBoundingRadius = Math.Abs(player[vtx].X);
                }

                if (Math.Abs(player[vtx].Y) > m_dBoundingRadius)
                {
                    m_dBoundingRadius = Math.Abs(player[vtx].Y);
                }
            }

            //set up the steering behavior class
            m_pSteering = new SteeringBehaviors(game, this,
                                                m_pTeam.Pitch(),
                                                Ball());

            //a player's start target is its start position (because it's just waiting)
            m_pSteering.SetTarget(home_team.Pitch().GetRegionFromIndex(home_region).Center());
            members.Add(this);
        }

        public static List<PlayerBase> GetAllMembers()
        {
            return members;
        }

        ~PlayerBase()
        {
            m_pSteering.Dispose();
            Dispose(false);
        }

        //------------------------- IsThreatened ---------------------------------
        //
        //  returns true if there is an opponent within this player's 
        //  comfort zone
        //------------------------------------------------------------------------

        public bool isThreatened()
        {
            //check against all opponents to make sure non are within this
            //player's comfort zone
            foreach (PlayerBase curOpp in Team().Opponents().Members())
            {
                //calculate distance to the player. if dist is less than our
                //comfort zone, and the opponent is infront of the player, return true
                if (PositionInFrontOfPlayer((curOpp).Pos()) &&
                    (Vector2.DistanceSquared(Pos(), (curOpp).Pos()) < Prm.PlayerComfortZoneSq))
                {
                    return true;
                }
            } // next opp

            return false;
        }


        //rotates the player to face the ball or the player's current target
        //----------------------------- TrackBall --------------------------------
        //
        //  sets the player's heading to point at the ball
        //------------------------------------------------------------------------

        public void TrackBall()
        {
            // Disable by Korin.  Just can't seem to get it working right.
            // RotateHeadingToFacePosition(Ball().Pos());
            SetHeading(Vector2.Normalize(Ball().Pos() - Pos()));
        }

        //----------------------------- TrackTarget --------------------------------
        //
        //  sets the player's heading to point at the current target
        //------------------------------------------------------------------------

        public void TrackTarget()
        {
            SetHeading(Vector2.Normalize(Steering().Target() - Pos()));
        }

        //       private  bool  SortByDistanceToOpponentsGoal(PlayerBase p1,
        //                                    PlayerBase p2)
        //{
        //  return (p1.DistToOppGoal() < p2.DistToOppGoal());
        //}

        //private bool  SortByReversedDistanceToOpponentsGoal( PlayerBase p1,
        //                                            PlayerBase p2)
        //{
        //  return (p1.DistToOppGoal() > p2.DistToOppGoal());
        //}

        //this messages the player that is closest to the supporting spot to
        //change state to support the attacking player
        //----------------------------- FindSupport -----------------------------------
        //
        //  determines the player who is closest to the SupportSpot and messages him
        //  to tell him to change state to SupportAttacker
        //-----------------------------------------------------------------------------
        public void FindSupport()
        {
            PlayerBase BestSupportPly = Team().DetermineBestSupportingAttacker();
            //if there is no support we need to find a suitable player.
            if (Team().SupportingPlayer() == null)
            {
                Team().SetSupportingPlayer(BestSupportPly);

                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                       ID(),
                                       Team().SupportingPlayer().ID(),
                                       (int) SoccerMessages.Msg_SupportAttacker,
                                       null);
            }

            //if the best player available to support the attacker changes, update
            //the pointers and send messages to the relevant players to update their
            //states
            if (BestSupportPly != null && (BestSupportPly != Team().SupportingPlayer()))
            {
                if (Team().SupportingPlayer() != null)
                {
                    Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                           ID(),
                                           Team().SupportingPlayer().ID(),
                                           (int) SoccerMessages.Msg_GoHome,
                                           null);
                }


                Team().SetSupportingPlayer(BestSupportPly);

                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                       ID(),
                                       Team().SupportingPlayer().ID(),
                                       (int) SoccerMessages.Msg_SupportAttacker,
                                       null);
            }
        }


        //returns true if the ball can be grabbed by the goalkeeper
        public bool BallWithinKeeperRange()
        {
            return (Vector2.DistanceSquared(Pos(), Ball().Pos()) < Prm.KeeperInBallRangeSq);
        }


        //returns true if the ball is within kicking range
        public bool BallWithinKickingRange()
        {
            return (Vector2.DistanceSquared(Ball().Pos(), Pos()) < Prm.PlayerKickingDistanceSq);
        }


        //returns true if a ball comes within range of a receiver
        public bool BallWithinReceivingRange()
        {
            return (Vector2.DistanceSquared(Pos(), Ball().Pos()) < Prm.BallWithinReceivingRangeSq);
        }


        //returns true if the player is located within the boundaries 
        //of his home region
        public bool InHomeRegion()
        {
            if (m_PlayerRole == player_role.goal_keeper)
            {
                return Pitch().GetRegionFromIndex(m_iHomeRegion).Inside(Pos(), Region.region_modifier.normal);
            }
            return Pitch().GetRegionFromIndex(m_iHomeRegion).Inside(Pos(), Region.region_modifier.halfsize);
        }


        //returns true if this player is ahead of the attacker
        public bool isAheadOfAttacker()
        {
            return Math.Abs(Pos().X - Team().OpponentsGoal().Center().X) <
                   Math.Abs(Team().ControllingPlayer().Pos().X - Team().OpponentsGoal().Center().X);
        }


        //returns true if a player is located at the designated support spot
        // public bool        AtSupportSpot();

        //returns true if the player is located at his steering target
        public bool AtTarget()
        {
            return (Vector2.DistanceSquared(Pos(), Steering().Target()) < Prm.PlayerInTargetRangeSq);
        }


        //returns true if the player is the closest player in his team to
        //the ball
        public bool isClosestTeamMemberToBall()
        {
            return Team().PlayerClosestToBall() == this;
        }


        //returns true if the point specified by 'position' is located in
        //front of the player
        //------------------------- WithinFieldOfView ---------------------------
        //
        //  returns true if subject is within field of view of this player
        //-----------------------------------------------------------------------
        public bool PositionInFrontOfPlayer(Vector2 position)
        {
            Vector2 ToSubject = position - Pos();

            if (Vector2.Dot(ToSubject, Heading()) > 0)
            {
                return true;
            }
            return false;
        }

        //returns true if the player is the closest player on the pitch to the ball
        public bool isClosestPlayerOnPitchToBall()
        {
            return isClosestTeamMemberToBall() &&
                   (DistSqToBall() < Team().Opponents().ClosestDistToBallSq());
        }


        //returns true if this player is the controlling player
        public bool isControllingPlayer()
        {
            return Team().ControllingPlayer() == this;
        }


        //returns true if the player is located in the designated 'hot region' --
        //the area close to the opponent's goal
        public bool InHotRegion()
        {
            return Math.Abs(Pos().Y - Team().OpponentsGoal().Center().Y) <
                   Pitch().PlayingArea().Length()/3.0;
        }


        public player_role Role()
        {
            return m_PlayerRole;
        }

        public float DistSqToBall()
        {
            return m_dDistSqToBall;
        }

        public void SetDistSqToBall(float val)
        {
            m_dDistSqToBall = val;
        }

        //calculate distance to opponent's/home goal. Used frequently by the passing
        //methods
        public float DistToOppGoal()
        {
            return Math.Abs(Pos().X - Team().OpponentsGoal().Center().X);
        }

        public float DistToHomeGoal()
        {
            return Math.Abs(Pos().X - Team().HomeGoal().Center().X);
        }

        public void SetDefaultHomeRegion()
        {
            m_iHomeRegion = m_iDefaultRegion;
        }

        public SoccerBall Ball()
        {
            return Team().Pitch().Ball();
        }

        public SoccerPitch Pitch()
        {
            return Team().Pitch();
        }

        public SteeringBehaviors Steering()
        {
            return m_pSteering;
        }

        public Region HomeRegion()
        {
            return Pitch().GetRegionFromIndex(m_iHomeRegion);
        }

        public void SetHomeRegion(int NewRegion)
        {
            m_iHomeRegion = NewRegion;
        }

        public AbstractSoccerTeam Team()
        {
            return m_pTeam;
        }
    }
}