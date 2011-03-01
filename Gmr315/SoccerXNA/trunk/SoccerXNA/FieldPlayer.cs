#region Using

using Common._2D;
using Common.FSM;
using Common.Game;
using Common.Messaging;
using Common.Misc;
using Common.Time;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SoccerXNA
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class FieldPlayer : PlayerBase
    {
        //an instance of the state machine class

        //limits the number of kicks a player may take per second
        private readonly Regulator m_pKickLimiter;
        private readonly StateMachine<FieldPlayer> m_pStateMachine;
        private readonly ParamLoader Prm = ParamLoader.Instance;


        public FieldPlayer(Game game, AbstractSoccerTeam home_team,
                           int home_region,
                           State<FieldPlayer> start_state,
            State<FieldPlayer> global_state,
                           Vector2 heading,
                           Vector2 velocity,
                           float mass,
                           float max_force,
                           float max_speed,
                           float max_turn_rate,
                           float scale,
                           player_role role)
            : base(
                game, home_team, home_region, heading, velocity, mass, max_force, max_speed, max_turn_rate, scale, role)
        {
            //set up the state machine
            m_pStateMachine = new StateMachine<FieldPlayer>(this);

            if (start_state != null)
            {
                m_pStateMachine.SetCurrentState(start_state);
                m_pStateMachine.SetPreviousState(start_state);
                m_pStateMachine.SetGlobalState(global_state);

                m_pStateMachine.CurrentState().Enter(this);
            }

            m_pSteering.SeparationOn();

            //set up the kick regulator
            m_pKickLimiter = new Regulator(Prm.PlayerKickFrequency);
        }

        ~FieldPlayer()
        {
            m_pKickLimiter.Dispose();
            m_pStateMachine.Dispose();
            Dispose(false);
        }


        //call this to update the player's position and orientation
        public override void Update(GameTime gameTime)
        {
            //run the logic for the current state
            m_pStateMachine.Update();

            //calculate the combined steering force
            m_pSteering.Calculate();

            
            //if no steering force is produced decelerate the player by applying a
            //braking force
            if (m_pSteering.Force() == Vector2.Zero)
            {
                const float BrakingRate = 0.8f;

                m_vVelocity = m_vVelocity*BrakingRate;
            }

 
            //the steering force's side component is a force that rotates the 
            //player about its axis. We must limit the rotation so that a player
            //can only turn by PlayerMaxTurnRate rads per update.
            float TurningForce = m_pSteering.SideComponent();

            TurningForce = Utils.Clamp(TurningForce, -Prm.PlayerMaxTurnRate, Prm.PlayerMaxTurnRate);

            //rotate the heading vector
            m_vHeading = Transformations.Vec2DRotateAroundOrigin(m_vHeading, TurningForce);

            //make sure the velocity vector points in the same direction as
            //the heading vector
            m_vVelocity = m_vHeading*m_vVelocity.Length();

            //and recreate m_vSide
            m_vSide = m_vHeading.Perp();


            //now to calculate the acceleration due to the force exerted by
            //the forward component of the steering force in the direction
            //of the player's heading
            Vector2 accel = m_vHeading*m_pSteering.ForwardComponent()/m_dMass;
            m_vVelocity += accel;

            //make sure player does not exceed maximum velocity
            m_vVelocity = m_vVelocity.Truncate(m_dMaxSpeed);



            //update the position
            m_vPosition += m_vVelocity;

            Debug.WriteLine("Player " + this.ID() + " end position: " + m_vPosition);

            //enforce a non-penetration constraint if desired
            if (Prm.bNonPenetrationConstraint)
            {
                EntityFunctionTemplates<PlayerBase>.EnforceNonPenetrationContraint(this, GetAllMembers());
            }
        }

        public override void Draw(GameTime game)
        {
            Color teamColor = Color.Red;

            //set appropriate team color
            if (Team().Color() == AbstractSoccerTeam.team_color.blue)
            {
                teamColor = Color.Blue;
            }


            //render the player's body
            m_vecPlayerVBTrans = Transformations.WorldTransform(m_vecPlayerVB,
                                                                Pos(),
                                                                Heading(),
                                                                Side(),
                                                                Scale());
            Drawing.DrawClosedShaped(PrimitiveBatch, m_vecPlayerVBTrans, teamColor);

            //and 'is 'ead
            Color circleColor = Color.Brown;

            if (Prm.bHighlightIfThreatened && (Team().ControllingPlayer() == this) && isThreatened())
            {
                circleColor = Color.Yellow;
            }
            Drawing.DrawCircle(PrimitiveBatch, Pos(), 6, circleColor);


            //render the state
            if (Prm.bStates)
            {
                SpriteBatch.DrawString(GameFont, m_pStateMachine.GetNameOfCurrentState(),
                                       new Vector2(m_vPosition.X, m_vPosition.Y - 20), new Color(0, 170, 0));
            }

            //show IDs
            if (Prm.bIDs)
            {
                SpriteBatch.DrawString(GameFont, ID().ToString(), new Vector2(m_vPosition.X - 20, m_vPosition.Y - 20),
                                       new Color(0, 170, 0));
            }


            if (Prm.bViewTargets)
            {
                Drawing.DrawCircle(PrimitiveBatch, Steering().Target(), 3, Color.Red);
                SpriteBatch.DrawString(GameFont, ID().ToString(), Steering().Target(), Color.Red);
            }
        }


        //-------------------- HandleMessage -------------------------------------
        //
        //  routes any messages appropriately
        //------------------------------------------------------------------------

        public override bool HandleMessage(Telegram msg)
        {
            return m_pStateMachine.HandleMessage(msg);
        }


        public StateMachine<FieldPlayer> GetFSM()
        {
            return m_pStateMachine;
        }

        public bool isReadyForNextKick()
        {
            return m_pKickLimiter.isReady();
        }
    }
}