using System;
using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Misc;
using Microsoft.Xna.Framework;
using Raven.lua;

namespace Raven
{
    public class AbstractSteering : IDisposable
    {
        #region summing_method enum

        public enum summing_method
        {
            weighted_average,
            prioritized,
            dithered
        } ;

        #endregion

        protected static Raven_Scriptor script = Raven_Scriptor.Instance();
        protected Deceleration m_Deceleration;
        protected float m_dViewDistance;


        //the length of the 'feeler/s' used in wall detection
        protected float m_dWallDetectionFeelerLength;
        protected float m_dWanderDistance;


        //the current position on the wander circle the agent is
        //attempting to steer towards

        //explained above
        protected float m_dWanderJitter;
        protected float m_dWanderRadius;
        protected float m_dWeightArrive;
        protected float m_dWeightSeek;


        //multipliers. These can be adjusted to effect strength of the  
        //appropriate behavior.
        protected float m_dWeightSeparation;
        protected float m_dWeightWallAvoidance;
        protected float m_dWeightWander;
        protected List<Vector2> m_Feelers;
        protected  AbstractBot m_pRaven_Bot;

        //pointer to the world data
        protected  Raven_Game m_pWorld;

        //is cell space partitioning to be used or not?
        protected bool m_bCellSpaceOn;
        protected int m_iFlags;
        protected AbstractBot m_pTargetAgent1;
        protected AbstractBot m_pTargetAgent2;

        //what type of method is used to sum any active behavior
        protected summing_method m_SummingMethod;
        protected Vector2 m_vSteeringForce;
        protected Vector2 m_vTarget;
        protected Vector2 m_vWanderTarget;

        public AbstractSteering(Raven_Game world, AbstractBot agent)
        {
            m_pWorld = world;
            m_pRaven_Bot = agent;
            m_iFlags = 0;
            m_Feelers = new List<Vector2>(3);
            m_pTargetAgent1 = null;
            m_pTargetAgent2 = null;
        }

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        //this function tests if a specific bit of m_iFlags is set
        protected bool On(behavior_type bt)
        {
            return (m_iFlags & (int) bt) == (int) bt;
        }

        protected bool AccumulateForce(ref Vector2 RunningTot, Vector2 ForceToAdd)
        {
            if(ForceToAdd == Vector2.Zero)
            {
                return true;
            }
            //calculate how much steering force the vehicle has used so far
            float MagnitudeSoFar = RunningTot.Length();

            //calculate how much steering force remains to be used by this vehicle
            float MagnitudeRemaining = m_pRaven_Bot.MaxForce() - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0f) return false;

            //calculate the magnitude of the force we want to add
            float MagnitudeToAdd = ForceToAdd.Length();

            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this vehicle, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                RunningTot += ForceToAdd;
            }

            else
            {
                MagnitudeToAdd = MagnitudeRemaining;

                //add it to the steering force
                RunningTot += (Vector2.Normalize(ForceToAdd)*MagnitudeToAdd);
                
            }
            Utils.TestVector(RunningTot);
            return true;
        }

        //creates the antenna utilized by the wall avoidance behavior
        protected void CreateFeelers()
        {
            m_Feelers.Clear();
            //feeler pointing straight in front
            m_Feelers.Add(m_pRaven_Bot.Pos() + m_dWallDetectionFeelerLength*
                                               m_pRaven_Bot.Heading()*m_pRaven_Bot.Speed());

            //feeler to left
            Vector2 temp = m_pRaven_Bot.Heading();
            temp = Transformations.Vec2DRotateAroundOrigin(ref temp, MathHelper.PiOver2*3.5f);
            if(temp != Vector2.Zero)
            {
                m_Feelers.Add(m_pRaven_Bot.Pos() + m_dWallDetectionFeelerLength / 2.0f * temp);    
            }
            

            //feeler to right
            temp = m_pRaven_Bot.Heading();
            temp = Transformations.Vec2DRotateAroundOrigin(ref temp, MathHelper.PiOver2*0.5f);
            if (temp != Vector2.Zero)
            {
                m_Feelers.Add(m_pRaven_Bot.Pos() + m_dWallDetectionFeelerLength/2.0f*temp);
            }
        }


        /* .......................................................

                    BEGIN BEHAVIOR DECLARATIONS

      .......................................................*/


        //this behavior moves the agent towards a target position
        protected virtual Vector2 Seek(Vector2 target)
        {
            Vector2 targetVector = target - m_pRaven_Bot.Pos();
            Vector2 DesiredVelocity = Vector2.Zero;
            if(targetVector != Vector2.Zero)
            {
                DesiredVelocity = Vector2.Normalize(target - m_pRaven_Bot.Pos())
                                  *m_pRaven_Bot.MaxSpeed();
            }
            return (DesiredVelocity - m_pRaven_Bot.Velocity());
        }

        //this behavior is similar to seek but it attempts to arrive 
        //at the target with a zero velocity
        protected virtual Vector2 Arrive(Vector2 target,
                               Deceleration deceleration)
        {
            Vector2 ToTarget = target - m_pRaven_Bot.Pos();

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
                speed = Math.Min(speed, m_pRaven_Bot.MaxSpeed());

                //from here proceed just like Seek except we don't need to normalize 
                //the ToTarget vector because we have already gone to the trouble
                //of calculating its length: dist. 
                Vector2 DesiredVelocity = ToTarget*speed/dist;

                return (DesiredVelocity - m_pRaven_Bot.Velocity());
            }

            return Vector2.Zero;
        }


        //this behavior makes the agent wander about randomly
        protected virtual Vector2 Wander()
        {
            //first, add a small random vector to the target's position
            m_vWanderTarget += new Vector2(Utils.RandomClamped()*m_dWanderJitter,
                                           Utils.RandomClamped()*m_dWanderJitter);

            //reproject this new vector back on to a unit circle
            if (m_vWanderTarget != Vector2.Zero)
            {
                m_vWanderTarget.Normalize();
            }
            //increase the length of the vector to the same as the radius
            //of the wander circle
            m_vWanderTarget *= m_dWanderRadius;

            //move the target into a position WanderDist in front of the agent
            Vector2 target = m_vWanderTarget + new Vector2(m_dWanderDistance, 0);

            Vector2 heading = m_pRaven_Bot.Heading();
            Vector2 side = m_pRaven_Bot.Side();
            Vector2 pos = m_pRaven_Bot.Pos();
            Utils.TestVector(target);
            //project the target into world space
            Vector2 Target = Transformations.PointToWorldSpace(ref target,
                                                               ref heading,
                                                               ref side,
                                                               ref pos);
            Utils.TestVector(Target);
            //and steer towards it
            return Target - m_pRaven_Bot.Pos();
        }

        //this returns a steering force which will keep the agent away from any
        //walls it may encounter
        protected virtual Vector2 WallAvoidance(List<Wall2D> walls)
        {
            //the feelers are contained in a std::vector, m_Feelers
            CreateFeelers();

            float DistToThisIP = 0.0f;
            float DistToClosestIP = float.MaxValue;

            //this will hold an index into the vector of walls
            int ClosestWall = -1;

            var SteeringForce = new Vector2();

            var point = new Vector2();
            var ClosestPoint = new Vector2();
            //examine each feeler in turn
            for (int flr = 0; flr < m_Feelers.Count(); ++flr)
            {
                //run through each wall checking for any intersection points
                for (int w = 0; w < walls.Count(); ++w)
                {
                    if (Geometry.LineIntersection2D(m_pRaven_Bot.Pos(),
                                                    m_Feelers[flr],
                                                    walls[w].From(),
                                                    walls[w].To(),
                                                    ref DistToThisIP,
                                                    ref point))
                    {
                        //is this the closest found so far? If so keep a record
                        if (DistToThisIP < DistToClosestIP)
                        {
                            DistToClosestIP = DistToThisIP;

                            ClosestWall = w;

                            ClosestPoint = point;
                        }
                    }
                } //next wall


                //if an intersection point has been detected, calculate a force  
                //that will direct the agent away
                if (ClosestWall >= 0)
                {
                    //calculate by what distance the projected position of the agent
                    //will overshoot the wall
                    Vector2 OverShoot = m_Feelers[flr] - ClosestPoint;

                    //create a force in the direction of the wall normal, with a 
                    //magnitude of the overshoot
                    SteeringForce = walls[ClosestWall].Normal()*OverShoot.Length();
                }
            } //next feeler

            return SteeringForce;
        }


        protected virtual Vector2 Separation(List<AbstractBot> agents)
        {
            //iterate through all the neighbors and calculate the vector from the
            var SteeringForce = new Vector2();

            foreach (AbstractBot it in agents)
            {
                //make sure this agent isn't included in the calculations and that
                //the agent being examined is close enough. ***also make sure it doesn't
                //include the evade target ***
                if ((it != m_pRaven_Bot) && it.IsTagged() &&
                    (it != m_pTargetAgent1))
                {
                    Vector2 ToAgent = m_pRaven_Bot.Pos() - it.Pos();

                    //scale the force inversely proportional to the agents distance  
                    //from its neighbor.
                    if(ToAgent != Vector2.Zero)
                        SteeringForce += Vector2.Normalize(ToAgent)/ToAgent.Length();
                }
            }

            return SteeringForce;
        }


        /* .......................................................

                       END BEHAVIOR DECLARATIONS

      .......................................................*/

        //calculates and sums the steering forces from any active behaviors
        protected virtual Vector2 CalculatePrioritized()
        {
            Vector2 force;

            if (On(behavior_type.wall_avoidance))
            {
                force = WallAvoidance(m_pWorld.GetMap().GetWalls())*
                        m_dWeightWallAvoidance;
                Utils.TestVector(force);
                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }


            //these next three can be combined for flocking behavior (wander is
            //also a good behavior to add into this mix)

            if (On(behavior_type.separation))
            {
                force = Separation(m_pWorld.GetAllBots())*m_dWeightSeparation;
                Utils.TestVector(force);
                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }


            if (On(behavior_type.seek))
            {
                force = Seek(m_vTarget)*m_dWeightSeek;
                Utils.TestVector(force);
                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }


            if (On(behavior_type.arrive))
            {
                force = Arrive(m_vTarget, m_Deceleration)*m_dWeightArrive;
                Utils.TestVector(force);
                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }

            if (On(behavior_type.wander))
            {
                force = Wander()*m_dWeightWander;
                Utils.TestVector(force);
                if (!AccumulateForce(ref m_vSteeringForce, force)) return m_vSteeringForce;
            }


            return m_vSteeringForce;
        }


        //calculates and sums the steering forces from any active behaviors
        public virtual Vector2 Calculate()
        {
            //reset the steering force
            m_vSteeringForce = Vector2.Zero;

            //tag neighbors if any of the following 3 group behaviors are switched on
            if (On(behavior_type.separation))
            {
                m_pWorld.TagRaven_BotsWithinViewRange(m_pRaven_Bot, m_dViewDistance);
            }

            m_vSteeringForce = CalculatePrioritized();

            return m_vSteeringForce;
        }

        //calculates the component of the steering force that is parallel
        //with the Raven_Bot heading
        public float ForwardComponent()
        {
            return Vector2.Dot(m_pRaven_Bot.Heading(), m_vSteeringForce);
        }

        //calculates the component of the steering force that is perpendicuar
        //with the Raven_Bot heading
        public float SideComponent()
        {
            return Vector2.Dot(m_pRaven_Bot.Side(), m_vSteeringForce);
        }


        public void SetTarget(Vector2 t)
        {
            m_vTarget = t;
        }

        public Vector2 Target()
        {
            return m_vTarget;
        }

        public void SetTargetAgent1(AbstractBot Agent)
        {
            m_pTargetAgent1 = Agent;
        }

        public void SetTargetAgent2(AbstractBot Agent)
        {
            m_pTargetAgent2 = Agent;
        }


        public Vector2 Force()
        {
            return m_vSteeringForce;
        }

        public void SetSummingMethod(summing_method sm)
        {
            m_SummingMethod = sm;
        }


        public void SeekOn()
        {
            m_iFlags |= (int) behavior_type.seek;
        }

        public void ArriveOn()
        {
            m_iFlags |= (int) behavior_type.arrive;
        }

        public void WanderOn()
        {
            m_iFlags |= (int) behavior_type.wander;
        }

        public void SeparationOn()
        {
            m_iFlags |= (int) behavior_type.separation;
        }

        public void WallAvoidanceOn()
        {
            m_iFlags |= (int) behavior_type.wall_avoidance;
        }

        public void SeekOff()
        {
            if (On(behavior_type.seek)) m_iFlags ^= (int) behavior_type.seek;
        }

        public void ArriveOff()
        {
            if (On(behavior_type.arrive)) m_iFlags ^= (int) behavior_type.arrive;
        }

        public void WanderOff()
        {
            if (On(behavior_type.wander)) m_iFlags ^= (int) behavior_type.wander;
        }

        public void SeparationOff()
        {
            if (On(behavior_type.separation)) m_iFlags ^= (int) behavior_type.separation;
        }

        public void WallAvoidanceOff()
        {
            if (On(behavior_type.wall_avoidance)) m_iFlags ^= (int) behavior_type.wall_avoidance;
        }

        public bool SeekIsOn()
        {
            return On(behavior_type.seek);
        }

        public bool ArriveIsOn()
        {
            return On(behavior_type.arrive);
        }

        public bool WanderIsOn()
        {
            return On(behavior_type.wander);
        }

        public bool SeparationIsOn()
        {
            return On(behavior_type.separation);
        }

        public bool WallAvoidanceIsOn()
        {
            return On(behavior_type.wall_avoidance);
        }

        public List<Vector2> GetFeelers()
        {
            return m_Feelers;
        }

        public float WanderJitter()
        {
            return m_dWanderJitter;
        }

        public float WanderDistance()
        {
            return m_dWanderDistance;
        }

        public float WanderRadius()
        {
            return m_dWanderRadius;
        }

        public float SeparationWeight()
        {
            return m_dWeightSeparation;
        }

        #region Nested type: behavior_type

        protected enum behavior_type
        {
            none = 0x00000,
            seek = 0x00002,
            arrive = 0x00008,
            wander = 0x00010,
            separation = 0x00040,
            wall_avoidance = 0x00200,
        }

        #endregion

        #region Nested type: Deceleration

        protected enum Deceleration
        {
            slow = 3,
            normal = 2,
            fast = 1
        } ;

        #endregion
    }
}