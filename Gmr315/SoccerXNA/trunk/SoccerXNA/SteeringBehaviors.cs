#region Using

using System;
using System.Collections.Generic;
using Common._2D;
using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace SoccerXNA
{
    public class SteeringBehaviors : DrawableGameComponent
    {
        //multipliers. 
        protected static PrimitiveBatch primitiveBatch;
        private readonly List<Vector2> m_Antenna;
        private readonly float m_dMultSeparation;

        //how far it can 'see'
        private readonly float m_dViewDistance;
        private readonly SoccerBall m_pBall;
        private readonly PlayerBase m_pPlayer;
        private readonly ParamLoader Prm = ParamLoader.Instance;


        //binary flags to indicate whether or not a behavior should be active

        //used by group behaviors to tag neighbours
        private bool m_bTagged;
        private float m_dInterposeDist;
        private int m_iFlags;
        private Vector2 m_vSteeringForce;

        //the current target (usually the ball or predicted ball position)
        private Vector2 m_vTarget;

        public SteeringBehaviors(Game game, PlayerBase agent,
                                 SoccerPitch world,
                                 SoccerBall ball) : base(game)
        {
            m_pPlayer = agent;
            m_iFlags = 0;
            m_dMultSeparation = Prm.SeparationCoefficient;
            m_bTagged = false;
            m_dViewDistance = Prm.ViewDistance;
            m_pBall = ball;
            m_dInterposeDist = 0.0f;
            m_Antenna = new List<Vector2>(5);
            for (int i = 0; i < 5; i++)
            {
                m_Antenna.Add(new Vector2());
            }
            m_vSteeringForce = new Vector2();
        }

        protected PrimitiveBatch PrimitiveBatch
        {
            get
            {
                if (primitiveBatch == null)
                {
                    IDrawingManager manager = (IDrawingManager) Game.Services.GetService(typeof (IDrawingManager));
                    primitiveBatch = manager.GetPrimitiveBatch();
                }
                return primitiveBatch;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        //Arrive makes use of these to determine how quickly a vehicle
        //should decelerate to its target


        //this behavior moves the agent towards a target position
        //------------------------------- Seek -----------------------------------
        //
        //  Given a target, this behavior returns a steering force which will
        //  allign the agent with the target and move the agent in the desired
        //  direction
        //------------------------------------------------------------------------
        private Vector2 Seek(Vector2 target)
        {
            Vector2 toTarget = target - m_pPlayer.Pos();

            Vector2 DesiredVelocity = Vector2.Zero;
            
            if(toTarget != Vector2.Zero) {
                DesiredVelocity = Vector2.Normalize(toTarget)* m_pPlayer.MaxSpeed();
            }

            return (DesiredVelocity - m_pPlayer.Velocity());
        }


        //this behavior is similar to seek but it attempts to arrive 
        //at the target with a zero velocity
        //--------------------------- Arrive -------------------------------------
        //
        //  This behavior is similar to seek but it attempts to arrive at the
        //  target with a zero velocity
        //------------------------------------------------------------------------
        private Vector2 Arrive(Vector2 target, Deceleration deceleration)
        {
            Vector2 ToTarget = target - m_pPlayer.Pos();

            //calculate the distance to the target
            float dist = ToTarget.Length();

            if (dist > 0)
            {
                //because Deceleration is enumerated as an int, this value is required
                //to provide fine tweaking of the deceleration..
                const float DecelerationTweaker = 0.3f;

                //calculate the speed required to reach the target given the desired
                //deceleration
                float speed = dist/((float) deceleration*DecelerationTweaker);

                //make sure the velocity does not exceed the max
                speed = Math.Min(speed, m_pPlayer.MaxSpeed());

                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2 DesiredVelocity = ToTarget*speed/dist;

                return (DesiredVelocity - m_pPlayer.Velocity());
            }

            return new Vector2(0, 0);
        }


        //This behavior predicts where its prey will be and seeks
        //to that location
        //------------------------------ Pursuit ---------------------------------
        //
        //  this behavior creates a force that steers the agent towards the 
        //  ball
        //------------------------------------------------------------------------
        private Vector2 Pursuit(SoccerBall ball)
        {
            Vector2 ToBall = ball.Pos() - m_pPlayer.Pos();

            //the lookahead time is proportional to the distance between the ball
            //and the pursuer; 
            float LookAheadTime = 0.0f;

            if (ball.Speed() != 0.0)
            {
                LookAheadTime = ToBall.Length()/ball.Speed();
            }

            //calculate where the ball will be at this time in the future
            m_vTarget = ball.FuturePosition(LookAheadTime);

            //now seek to the predicted future position of the ball
            return Arrive(m_vTarget, Deceleration.fast);
        }

        //---------------------------- Separation --------------------------------
        //
        // this calculates a force repelling from the other neighbors
        //------------------------------------------------------------------------

        private Vector2 Separation()
        {
            //iterate through all the neighbors and calculate the vector from the
            Vector2 SteeringForce = new Vector2();

            List<PlayerBase> AllPlayers = PlayerBase.GetAllMembers();
            foreach (PlayerBase curPlyr in AllPlayers)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent is close enough
                if ((curPlyr != m_pPlayer) && curPlyr.Steering().Tagged())
                {
                    Vector2 ToAgent = m_pPlayer.Pos() - curPlyr.Pos();

                    if (ToAgent != Vector2.Zero && ToAgent.Length() != 0)
                    {


                        //scale the force inversely proportional to the agents distance  
                        //from its neighbor.
                        SteeringForce += Vector2.Normalize(ToAgent)/ToAgent.Length();
                    }
                }
            }

            return SteeringForce;
        }


        //this attempts to steer the agent to a position between the opponent
        //and the object
        //--------------------------- Interpose ----------------------------------
        //
        //  Given an opponent and an object position this method returns a 
        //  force that attempts to position the agent between them
        //------------------------------------------------------------------------

        private Vector2 Interpose(SoccerBall ball,
                                  Vector2 target,
                                  float DistFromTarget)
        {
            Vector2 toTarget = ball.Pos() - target;
            Vector2 normalized = Vector2.Zero;
            if(toTarget != Vector2.Zero)
            {
                normalized = Vector2.Normalize(toTarget);
            }
            return Arrive(target + normalized *
                                   DistFromTarget, Deceleration.normal);
        }


        //finds any neighbours within the view radius
        //-------------------------- FindNeighbours ------------------------------
        //
        //  tags any vehicles within a predefined radius
        //------------------------------------------------------------------------
        private void FindNeighbours()
        {
            float viewDistanceSquared = m_dViewDistance*m_dViewDistance;
            List<PlayerBase> AllPlayers = PlayerBase.GetAllMembers();

            foreach (PlayerBase curPlyr in AllPlayers)
            {

                //first clear any current tag
                curPlyr.Steering().UnTag();

                if (curPlyr != m_pPlayer)
                {
                    //work in distance squared to avoid sqrts
                    Vector2 to = curPlyr.Pos() - m_pPlayer.Pos();

                    if (to.LengthSquared() < (viewDistanceSquared))
                    {
                        curPlyr.Steering().Tag();
                    }
                }
            } //next
        }


        //this function tests if a specific bit of m_iFlags is set
        private bool On(behavior_type bt)
        {
            return (m_iFlags & (int) bt) == (int) bt;
        }

        //--------------------- AccumulateForce ----------------------------------
        //
        //  This function calculates how much of its max steering force the 
        //  vehicle has left to apply and then applies that amount of the
        //  force to add.
        //------------------------------------------------------------------------
        private bool AccumulateForce(ref Vector2 sf, Vector2 ForceToAdd)
        {
            //first calculate how much steering force we have left to use
            float MagnitudeSoFar = sf.Length();

            float magnitudeRemaining = m_pPlayer.MaxForce() - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (magnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            float MagnitudeToAdd = ForceToAdd.Length();

            //now calculate how much of the force we can really add  
            if (MagnitudeToAdd > magnitudeRemaining)
            {
                MagnitudeToAdd = magnitudeRemaining;
            }

            //add it to the steering force
            if (ForceToAdd != Vector2.Zero)
            {
                ForceToAdd.Normalize();
            }
            sf += (ForceToAdd*MagnitudeToAdd);

            return true;
        }

        //-------------------------- SumForces -----------------------------------
        //
        //  this method calls each active steering behavior and acumulates their
        //  forces until the max steering force magnitude is reached at which
        //  time the function returns the steering force accumulated to that 
        //  point
        //------------------------------------------------------------------------

        private Vector2 SumForces()
        {
            Vector2 force = new Vector2();

            //the soccer players must always tag their neighbors
            FindNeighbours();

            if (On(behavior_type.separation))
            {
                force += Separation()*m_dMultSeparation;

                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            if (On(behavior_type.seek))
            {
                force += Seek(m_vTarget);

                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            if (On(behavior_type.arrive))
            {
                force += Arrive(m_vTarget, Deceleration.fast);

                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            if (On(behavior_type.pursuit))
            {
                force += Pursuit(m_pBall);

                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            if (On(behavior_type.interpose))
            {
                force += Interpose(m_pBall, m_vTarget, m_dInterposeDist);

                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            return m_vSteeringForce;
        }


        //a vertex buffer to contain the feelers rqd for dribbling

        ~SteeringBehaviors()
        {
            Dispose(false);
        }

        //---------------------- Calculate ---------------------------------------
        //
        //  calculates the overall steering force based on the currently active
        //  steering behaviors. 
        //------------------------------------------------------------------------
        public Vector2 Calculate()
        {
            //reset the force
            m_vSteeringForce = Vector2.Zero;

            //this will hold the value of each individual steering force
            m_vSteeringForce = SumForces();

            //make sure the force doesn't exceed the vehicles maximum allowable
            m_vSteeringForce = m_vSteeringForce.Truncate(m_pPlayer.MaxForce());

            return m_vSteeringForce;
        }

        //calculates the component of the steering force that is parallel
        //with the vehicle heading
        //------------------------- ForwardComponent -----------------------------
        //
        //  calculates the forward component of the steering force
        //------------------------------------------------------------------------

        public float ForwardComponent()
        {
            return Vector2.Dot(m_pPlayer.Heading(), m_vSteeringForce);
        }

        //calculates the component of the steering force that is perpendicuar
        //with the vehicle heading
        //--------------------------- SideComponent ------------------------------
        //
        //  //  calculates the side component of the steering force
        //------------------------------------------------------------------------

        public float SideComponent()
        {
            return Vector2.Dot(m_pPlayer.Side(), m_vSteeringForce)*m_pPlayer.MaxTurnRate();
        }


        public Vector2 Force()
        {
            return m_vSteeringForce;
        }

        //renders visual aids and info for seeing how each behavior is
        //calculated
        public void RenderAids()
        {
            Drawing.DrawLine(PrimitiveBatch, m_pPlayer.Pos(), m_pPlayer.Pos() + m_vSteeringForce*20, Color.Red);
        }


        public Vector2 Target()
        {
            return m_vTarget;
        }

        public void SetTarget(Vector2 t)
        {
            m_vTarget = t;
        }

        public float InterposeDistance()
        {
            return m_dInterposeDist;
        }

        public void SetInterposeDistance(float d)
        {
            m_dInterposeDist = d;
        }

        public bool Tagged()
        {
            return m_bTagged;
        }

        public void Tag()
        {
            m_bTagged = true;
        }

        public void UnTag()
        {
            m_bTagged = false;
        }


        public void SeekOn()
        {
            m_iFlags |= (int) behavior_type.seek;
        }

        public void ArriveOn()
        {
            m_iFlags |= (int) behavior_type.arrive;
        }

        public void PursuitOn()
        {
            m_iFlags |= (int) behavior_type.pursuit;
        }

        public void SeparationOn()
        {
            m_iFlags |= (int) behavior_type.separation;
        }

        public void InterposeOn(float d)
        {
            m_iFlags |= (int) behavior_type.interpose;
            m_dInterposeDist = d;
        }


        public void SeekOff()
        {
            if (On(behavior_type.seek)) m_iFlags ^= (int) behavior_type.seek;
        }

        public void ArriveOff()
        {
            if (On(behavior_type.arrive)) m_iFlags ^= (int) behavior_type.arrive;
        }

        public void PursuitOff()
        {
            if (On(behavior_type.pursuit)) m_iFlags ^= (int) behavior_type.pursuit;
        }

        public void SeparationOff()
        {
            if (On(behavior_type.separation)) m_iFlags ^= (int) behavior_type.separation;
        }

        public void InterposeOff()
        {
            if (On(behavior_type.interpose)) m_iFlags ^= (int) behavior_type.interpose;
        }


        public bool SeekIsOn()
        {
            return On(behavior_type.seek);
        }

        public bool ArriveIsOn()
        {
            return On(behavior_type.arrive);
        }

        public bool PursuitIsOn()
        {
            return On(behavior_type.pursuit);
        }

        public bool SeparationIsOn()
        {
            return On(behavior_type.separation);
        }

        public bool InterposeIsOn()
        {
            return On(behavior_type.interpose);
        }

        #region Nested type: behavior_type

        private enum behavior_type
        {
            none = 0x0000,
            seek = 0x0001,
            arrive = 0x0002,
            separation = 0x0004,
            pursuit = 0x0008,
            interpose = 0x0010
        } ;

        #endregion

        #region Nested type: Deceleration

        private enum Deceleration
        {
            slow = 3,
            normal = 2,
            fast = 1
        } ;

        #endregion
    }
}