using System;
using System.Collections.Generic;
using System.IO;
using Common._2D;
using Common.Game;
using Common.Misc;
using Common.Time;
using Common.Triggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.goals;
using Raven.lua;
using Raven.navigation;

namespace Raven
{
    public abstract class AbstractBot : MovingEntity, ITriggerSystem, IDisposable, IEntity
    {
        public const float  TIME_FACTOR = 1.0e06f;
        protected static readonly Raven_UserOptions UserOptions = Raven_UserOptions.Instance();
        protected static Raven_Scriptor script = Raven_Scriptor.Instance();
        protected readonly float m_dFieldOfView;
        protected readonly int m_iMaxHealth;

        //this object handles the arbitration and processing of high level goals
        protected Goal_Think m_pBrain;
        protected Regulator m_pGoalArbitrationRegulator;
        protected Raven_PathPlanner m_pPathPlanner;

        //this is a class that acts as the bots sensory memory. Whenever this
        //bot sees or hears an opponent, a record of the event is updated in the 
        //memory.
        protected Raven_SensoryMemory m_pSensoryMem;

        //the bot uses this object to steer
        protected AbstractSteering m_pSteering;

        //the bot uses this to plan paths
        protected Regulator m_pTargetSelectionRegulator;
        protected AbstractTargetingSystem m_pTargSys;
        protected Regulator m_pTriggerTestRegulator;
        protected Regulator m_pVisionUpdateRegulator;
        protected Regulator m_pWeaponSelectionRegulator;
        protected AbstractWeaponSystem m_pWeaponSys;
        protected readonly Raven_Game m_pWorld;

        //the bot's health. Every time the bot is shot this value is decreased. If
        //it reaches zero then the bot dies (and respawns)

        //set to true when the bot is hit, and remains true until 
        //m_iNumUpdatesHitPersistant becomes zero. (used by the render method to
        //draw a thick red circle around a bot to indicate it's been hit)
        protected bool m_bHit;

        //set to true when a human player takes over control of the bot
        protected bool m_bPossessed;
        protected int m_iHealth;
        protected int m_iNumUpdatesHitPersistant;
        protected int m_iScore;
        protected Status m_Status;

        //a vertex buffer containing the bot's geometry
        protected List<Vector2> m_vecBotVB;
        //the buffer for the transformed vertices
        protected List<Vector2> m_vecBotVBTrans;
        protected Vector2 m_vFacing;
        protected int m_iDeaths;

        protected AbstractBot(Raven_Game world, Vector2 pos) :
            base(pos,
                 script.GetDouble("Bot_Scale"),
                 new Vector2(0, 0),
                 script.GetDouble("Bot_MaxSpeed"),
                 new Vector2(1, 0),
                 script.GetDouble("Bot_Mass"),
                 new Vector2(script.GetDouble("Bot_Scale"), script.GetDouble("Bot_Scale")),
                 script.GetDouble("Bot_MaxHeadTurnRate"),
                 script.GetDouble("Bot_MaxForce"))
        {
            m_iMaxHealth = script.GetInt("Bot_MaxHealth");
            m_iHealth = script.GetInt("Bot_MaxHealth");
            m_pPathPlanner = null;
            m_pSteering = null;
            m_pWorld = world;
            m_pBrain = null;
            m_iNumUpdatesHitPersistant = (int) (Constants.FrameRate*script.GetDouble("HitFlashTime"));
            m_bHit = false;
            m_iScore = 0;
            m_Status = Status.spawning;
            m_bPossessed = false;
            m_dFieldOfView = Utils.DegsToRads(script.GetDouble("Bot_FOV"));
            m_iDeaths = 0;

           
        }

        #region IDisposable Members

        public new void Dispose()
        {
            Debug.WriteLine("deleteing raven bot (id = " + ID() + ")");


            m_pBrain.Dispose();
            m_pPathPlanner.Dispose();
            m_pSteering.Dispose();
            m_pWeaponSelectionRegulator.Dispose();
            m_pTargSys.Dispose();
            m_pGoalArbitrationRegulator.Dispose();
            m_pTargetSelectionRegulator.Dispose();
            m_pTriggerTestRegulator.Dispose();
            m_pVisionUpdateRegulator.Dispose();
            m_pWeaponSys.Dispose();
            m_pSensoryMem.Dispose();
        }

        #endregion

        #region ITriggerSystem Members

        public bool isAlive()
        {
            return m_Status == Status.alive;
        }

        public virtual bool isReadyForTriggerUpdate()
        {
            return m_pTriggerTestRegulator.isReady();
        }

        #endregion

        //this method is called from the update method. It calculates and applies
        //the steering force for this time-step.
        protected void UpdateMovement()
        {
            //calculate the combined steering force
            Vector2 force = m_pSteering.Calculate();
            Utils.TestVector(force);
            //if no steering force is produced decelerate the player by applying a
            //braking force
            if (m_pSteering.Force() == Vector2.Zero)
            {
                const float BrakingRate = 0.8f;

                SetVelocity(Velocity()*BrakingRate);
            }

            //calculate the acceleration
            Vector2 accel = force/Mass();

            Utils.TestVector(Velocity() + accel);
            //update the velocity
            SetVelocity(Velocity() + accel);

            Utils.TestVector(Velocity().Truncate(MaxSpeed()));
            //make sure vehicle does not exceed maximum velocity
            SetVelocity(Velocity().Truncate(MaxSpeed()));

            //update the position
            SetPos(Pos() + Velocity());

            //if the vehicle has a non zero velocity the heading and side vectors must 
            //be updated
            if (Velocity() != Vector2.Zero)
            {
                if (Velocity().Length() - 1.0f < 0.00001f)
                {
                    m_pSteering.SeekOn();
                }
                SetHeading(Vector2.Normalize(Velocity()));
            }
        }

        //initializes the bot's VB with its geometry
        protected void SetUpVertexBuffer()
        {
            //setup the vertex buffers and calculate the bounding radius
            const int NumBotVerts = 4;
            var bot = new[]
                          {
                              new Vector2(-3, 8),
                              new Vector2(3, 10),
                              new Vector2(3, -10),
                              new Vector2(-3, -8)
                          };

            SetBRadius(0.0f);
            float scale = script.GetDouble("Bot_Scale");
            m_vecBotVB = new List<Vector2>();
            for (int vtx = 0; vtx < NumBotVerts; ++vtx)
            {
                m_vecBotVB.Add(bot[vtx]);

                //set the bounding radius to the length of the 
                //greatest extent
                if (Math.Abs(bot[vtx].X)*scale > BRadius())
                {
                    SetBRadius(Math.Abs(bot[vtx].X*scale));
                }

                if (Math.Abs(bot[vtx].Y)*scale > BRadius())
                {
                    SetBRadius(Math.Abs(bot[vtx].Y)*scale);
                }
            }
        }


        //the usual suspects
        public void Render(PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch, SpriteFont gameFont)
        {
            //when a bot is hit by a projectile this value is set to a constant user
            //defined value which dictates how long the bot should have a thick red
            //circle drawn around it (to indicate it's been hit) The circle is drawn
            //as long as this value is positive. (see Render)
            m_iNumUpdatesHitPersistant--;


            if (isDead() || isSpawning()) return;

            Vector2 facingPerp = Facing().Perp();
            m_vecBotVBTrans = Transformations.WorldTransform(m_vecBotVB,
                                                             ref m_vPosition,
                                                             ref m_vFacing,
                                                             ref facingPerp,
                                                             ref m_vScale);
            Drawing.DrawClosedShaped(primitiveBatch, m_vecBotVBTrans, GetBodyColor());

            //draw the head
            Drawing.DrawFilledCircle(primitiveBatch, Pos(), 6.0f*Scale().X, GetHeadColor());

            //render the bot's weapon
            m_pWeaponSys.RenderCurrentWeapon(primitiveBatch);

            //render a thick red circle if the bot gets hit by a weapon
            if (m_bHit)
            {
                for (float x = BRadius() + 0.5f; x <= BRadius() + 1.0f; x= x+0.1f )
                    Drawing.DrawCircle(primitiveBatch, Pos(), x, Color.Red);

                if (m_iNumUpdatesHitPersistant <= 0)
                {
                    m_bHit = false;
                }
            }

            var textColor = new Color(0, 255, 0);

            if (UserOptions.m_bShowBotIDs)
            {
                spriteBatch.DrawString(gameFont, GetName() + ID(), new Vector2(Pos().X - 10, Pos().Y - 20), textColor);
            }

            if (UserOptions.m_bShowBotHealth)
            {
                spriteBatch.DrawString(gameFont, "H: " + Health(), new Vector2(Pos().X - 40, Pos().Y - 5), textColor);
            }

            if (UserOptions.m_bShowScore)
            {
                spriteBatch.DrawString(gameFont, "Scr: " + Score(), new Vector2(Pos().X - 40, Pos().Y + 10),
                                       textColor);
            }
        }


        public override void Update()
        {
            DoUpdate();
        }


        public override void Write(StreamWriter writer)
        {
/*not implemented*/
        }

        public override void Read(StreamReader reader)
        {
/*not implemented*/
        }

        //this rotates the bot's heading until it is facing directly at the target
        //position. Returns false if not facing at the target.
        public bool RotateFacingTowardPosition(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(target - Pos());

            float dot = Vector2.Dot(m_vFacing, toTarget);

            //clamp to rectify any rounding errors
            Utils.Clamp(ref dot, -1, 1);

            //determine the angle between the heading vector and the target
            var angle = (float) Math.Acos(dot);

            //return true if the bot's facing is within WeaponAimTolerance degs of
            //facing the target
            const float WeaponAimTolerance = 0.01f; //2 degs approx

            if (angle < WeaponAimTolerance)
            {
                m_vFacing = toTarget;
                return true;
            }

            //clamp the amount to turn to the max turn rate
            if (angle > MaxTurnRate()) angle = MaxTurnRate();

            //The next few lines use a rotation matrix to rotate the player's facing
            //vector accordingly
            var RotationMatrix = new C2DMatrix();

            //notice how the direction of rotation has to be determined when creating
            //the rotation matrix
            RotationMatrix.Rotate(angle*m_vFacing.Sign(toTarget));
            RotationMatrix.TransformVector2s(ref m_vFacing);

            return false;
        }

        //methods for accessing attribute data
        public int Health()
        {
            return m_iHealth;
        }

        public int MaxHealth()
        {
            return m_iMaxHealth;
        }

        public void ReduceHealth(int val)
        {
            m_iHealth -= val;

            if (m_iHealth <= 0)
            {
                SetDead();
                m_iDeaths++;
            }

            m_bHit = true;

            m_iNumUpdatesHitPersistant = (int) (Constants.FrameRate*script.GetDouble("HitFlashTime"));
        }

        public void IncreaseHealth(int val)
        {
            m_iHealth += val;
            Utils.Clamp(ref m_iHealth, 0, m_iMaxHealth);
        }

        public void RestoreHealthToMaximum()
        {
            m_iHealth = m_iMaxHealth;
        }

        public int Deaths()
        {
            return m_iDeaths;
        }
        public int Score()
        {
            return m_iScore;
        }

        public void IncrementScore()
        {
            ++m_iScore;
        }

        public Vector2 Facing()
        {
            return m_vFacing;
        }

        public float FieldOfView()
        {
            return m_dFieldOfView;
        }

        public bool isPossessed()
        {
            return m_bPossessed;
        }

        public bool isDead()
        {
            return m_Status == Status.dead;
        }

        public bool isSpawning()
        {
            return m_Status == Status.spawning;
        }

        public void SetSpawning()
        {
            m_Status = Status.spawning;
        }

        public void SetDead()
        {
            m_Status = Status.dead;
        }

        public void SetAlive()
        {
            m_Status = Status.alive;
        }

        //returns a value indicating the time in seconds it will take the bot
        //to reach the given position at its current speed.
        public float CalculateTimeToReachPosition(Vector2 pos)
        {
            return Vector2.Distance(Pos(), pos)/(MaxSpeed()*Constants.FrameRate);
        }

        //returns true if the bot is close to the given position
        public bool isAtPosition(Vector2 pos)
        {
            const float tolerance = 10.0f;

            return Vector2.DistanceSquared(Pos(), pos) < tolerance*tolerance;
        }


        //interface for human player
        public void FireWeapon(Vector2 pos)
        {
            m_pWeaponSys.ShootAt(pos);
        }

        public void ChangeWeapon(int type)
        {
            m_pWeaponSys.ChangeWeapon(type);
        }

        public void TakePossession()
        {
            if (!(isSpawning() || isDead()))
            {
                m_bPossessed = true;
                Debug.WriteLine("Player posses bot" + ID());
            }
        }

        public void Exorcise()
        {
            m_bPossessed = false;

            //when the player is exorcised then the bot should resume normal service
            m_pBrain.AddGoal_Explore();

            Debug.WriteLine("Player is exorcised from bot " + ID());
        }


        //spawns the bot at the given position
        public virtual void Spawn(Vector2 pos)
        {
            SetAlive();
            SetPos(pos);
            RestoreHealthToMaximum();
            m_pBrain.RemoveAllSubgoals();
            m_pTargSys.ClearTarget();
            m_pWeaponSys.Initialize();
            
        }

        //returns true if this bot is ready to test against all triggers

        //returns true if the bot has line of sight to the given position.
        public virtual bool hasLOSto(Vector2 pos)
        {
            return m_pWorld.isLOSOkay(Pos(), pos);
        }

        //returns true if this bot can move directly to the given position
        //without bumping into any walls
        public virtual bool canWalkTo(Vector2 pos)
        {
            return !m_pWorld.isPathObstructed(Pos(), pos, BRadius());
        }

        //similar to above. Returns true if the bot can move between the two
        //given positions without bumping into any walls
        public virtual bool canWalkBetween(Vector2 from, Vector2 to)
        {
            return !m_pWorld.isPathObstructed(from, to, BRadius());
        }


        //returns true if there is space enough to step in the indicated direction
        //If true PositionOfStep will be assigned the offset position
        public virtual bool canStepLeft(ref Vector2 PositionOfStep)
        {
            float StepDistance = BRadius()*2f;

            PositionOfStep = Pos() - Facing().Perp()*StepDistance - Facing().Perp()*BRadius();

            return canWalkTo(PositionOfStep);
        }

        public virtual bool canStepRight(ref Vector2 PositionOfStep)
        {
            float StepDistance = BRadius()*2f;

            PositionOfStep = Pos() + Facing().Perp()*StepDistance + Facing().Perp()*BRadius();

            return canWalkTo(PositionOfStep);
        }

        public virtual bool canStepForward(ref Vector2 PositionOfStep)
        {
            float StepDistance = BRadius()*2f;

            PositionOfStep = Pos() + Facing()*StepDistance + Facing()*BRadius();

            return canWalkTo(PositionOfStep);
        }

        public virtual bool canStepBackward(ref Vector2 PositionOfStep)
        {
            float StepDistance = BRadius()*2f;

            PositionOfStep = Pos() - Facing()*StepDistance - Facing()*BRadius();

            return canWalkTo(PositionOfStep);
        }


        public Raven_Game GetWorld()
        {
            return m_pWorld;
        }

        public AbstractSteering GetSteering()
        {
            return m_pSteering;
        }

        public Raven_PathPlanner GetPathPlanner()
        {
            return m_pPathPlanner;
        }

        public Goal_Think GetBrain()
        {
            return m_pBrain;
        }

        public AbstractTargetingSystem GetTargetSys()
        {
            return m_pTargSys;
        }

        public AbstractBot GetTargetBot()
        {
            return m_pTargSys.GetTarget();
        }

        public AbstractWeaponSystem GetWeaponSys()
        {
            return m_pWeaponSys;
        }

        public Raven_SensoryMemory GetSensoryMem()
        {
            return m_pSensoryMem;
        }

        public abstract Color GetBodyColor();
        public abstract Color GetHeadColor();
        public abstract string GetName();
        protected abstract void DoUpdate();

        #region Nested type: Status

        protected enum Status
        {
            alive,
            dead,
            spawning
        } ;

        #endregion
    }
}