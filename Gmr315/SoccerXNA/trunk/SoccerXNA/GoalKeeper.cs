#region Using

using Common._2D;
using Common.FSM;
using Common.Game;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SoccerXNA
{
    public class GoalKeeper : PlayerBase
    {
        private static readonly ParamLoader Prm = ParamLoader.Instance;

        //an instance of the state machine class
        private readonly StateMachine<GoalKeeper> m_pStateMachine;

        //this vector is updated to point towards the ball and is used when
        //rendering the goalkeeper (instead of the underlaying vehicle's heading)
        //to ensure he always appears to be watching the ball
        private Vector2 m_vLookAt;


        public GoalKeeper(Game game, AbstractSoccerTeam home_team,
                          int home_region,
                          State<GoalKeeper> start_state,
            State<GoalKeeper> global_state,
                          Vector2 heading,
                          Vector2 velocity,
                          float mass,
                          float max_force,
                          float max_speed,
                          float max_turn_rate,
                          float scale)
            : base(game,
                   home_team, home_region, heading, velocity, mass, max_force, max_speed, max_turn_rate, scale,
                   player_role.goal_keeper)
        {
            //set up the state machine
            m_pStateMachine = new StateMachine<GoalKeeper>(this);

            m_pStateMachine.SetCurrentState(start_state);
            m_pStateMachine.SetPreviousState(start_state);
            m_pStateMachine.SetGlobalState(global_state);

            m_pStateMachine.CurrentState().Enter(this);
        }

        ~GoalKeeper()
        {
            m_pStateMachine.Dispose();
            Dispose(false);
        }

        //these must be implemented
        public override void Update(GameTime gameTime)
        {
            Debug.debug = false;
            //run the logic for the current state
            m_pStateMachine.Update();

            //calculate the combined force from each steering behavior 
            Vector2 SteeringForce = m_pSteering.Calculate();

            //Acceleration = Force/Mass
            Vector2 Acceleration = SteeringForce/m_dMass;

            //update velocity
            m_vVelocity += Acceleration;

            //make sure player does not exceed maximum velocity
            m_vVelocity = m_vVelocity.Truncate(m_dMaxSpeed);

            //update the position
            m_vPosition += m_vVelocity;

            //enforce a non-penetration constraint if desired
            if (Prm.bNonPenetrationConstraint)
            {
                EntityFunctionTemplates<PlayerBase>.EnforceNonPenetrationContraint(this, GetAllMembers());
            }

            //update the heading if the player has a non zero velocity
            if (m_vVelocity != Vector2.Zero)
            {
                m_vHeading = Vector2.Normalize(m_vVelocity);

                m_vSide = m_vHeading.Perp();
            }

            //look-at vector always points toward the ball
            if (!Pitch().GoalKeeperHasBall())
            {
                m_vLookAt = Vector2.Normalize(Ball().Pos() - Pos());
            }
            Debug.debug = false;
        }


        public override void Draw(GameTime gameTime)
        {
            Color drawColor = Color.Red;
            if (Team().Color() == AbstractSoccerTeam.team_color.blue)
                drawColor = Color.Blue;


            m_vecPlayerVBTrans = Transformations.WorldTransform(m_vecPlayerVB,
                                                                Pos(),
                                                                m_vLookAt,
                                                                m_vLookAt.Perp(),
                                                                Scale());

            Drawing.DrawClosedShaped(PrimitiveBatch, m_vecPlayerVBTrans, drawColor);

            //draw the head
            Drawing.DrawCircle(PrimitiveBatch, Pos(), 6, Color.Brown);

            //draw the ID
            if (Prm.bIDs)
            {
                SpriteBatch.DrawString(GameFont, ID().ToString(), new Vector2(Pos().X - 20, Pos().Y - 20),
                                       new Color(0, 170, 0));
            }

            //draw the state
            if (Prm.bStates)
            {
                SpriteBatch.DrawString(GameFont, m_pStateMachine.GetNameOfCurrentState(),
                                       new Vector2(m_vPosition.X, m_vPosition.Y - 20), new Color(0, 170, 0));
            }

            if(Prm.bShowGoalKeeperTendingDistance)
            {
                Drawing.DrawCircle(PrimitiveBatch, Pos(), Prm.GoalKeeperTendingDistance, Color.DarkSlateGray);
            }
            if (Prm.bShowGoalKeeperInterceptRange)
            {
                Drawing.DrawCircle(PrimitiveBatch, Pos(), Prm.GoalKeeperInterceptRange, Color.OrangeRed);
            }
            if (Prm.bShowGoalkeeperMinPassDist)
            {
                Drawing.DrawCircle(PrimitiveBatch, Pos(), Prm.GoalkeeperMinPassDist, Color.GreenYellow);
            }
            base.Draw(gameTime);
        }

        //-------------------- HandleMessage -------------------------------------
        //
        //  routes any messages appropriately
        //------------------------------------------------------------------------

        public override bool HandleMessage(Telegram msg)
        {
            return m_pStateMachine.HandleMessage(msg);
        }


        //returns true if the ball comes close enough for the keeper to 
        //consider intercepting
        public bool BallWithinRangeForIntercept()
        {
            return (Vector2.DistanceSquared(Team().HomeGoal().Center(), Ball().Pos()) <=
                    Prm.GoalKeeperInterceptRangeSq);
        }


        //returns true if the keeper has ventured too far away from the goalmouth
        public bool TooFarFromGoalMouth()
        {
            return (Vector2.DistanceSquared(Pos(), GetRearInterposeTarget()) >
                    Prm.GoalKeeperInterceptRangeSq);
        }


        //this method is called by the Intercept state to determine the spot
        //along the goalmouth which will act as one of the interpose targets
        //(the other is the ball).
        //the specific point at the goal line that the keeper is trying to cover
        //is flexible and can move depending on where the ball is on the field.
        //To achieve this we just scale the ball's y value by the ratio of the
        //goal width to playingfield width

        public Vector2 GetRearInterposeTarget()
        {
            float xPosTarget = Team().HomeGoal().Center().X;

            float yPosTarget = Pitch().PlayingArea().Center().Y - 
                Prm.GoalWidth * 0.5f + (Ball().Pos().Y * Prm.GoalWidth) / Pitch().PlayingArea().Height();

            return new Vector2(xPosTarget, yPosTarget);
        }


        public StateMachine<GoalKeeper> GetFSM()
        {
            return m_pStateMachine;
        }


        public Vector2 LookAt()
        {
            return m_vLookAt;
        }

        public void SetLookAt(Vector2 v)
        {
            m_vLookAt = v;
        }
    }
}