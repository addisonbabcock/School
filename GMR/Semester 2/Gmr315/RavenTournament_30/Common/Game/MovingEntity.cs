#region Using

using System;
using Common._2D;
using Common.Misc;
using Microsoft.Xna.Framework;

#endregion

namespace Common.Game
{
    public abstract class MovingEntity : BaseGameEntity
    {
        protected float m_dMass;

        //the maximum speed this entity may travel at.

        //the maximum force this entity can produce to power itself 
        //(think rockets and thrust)
        protected float m_dMaxForce;
        protected float m_dMaxSpeed;

        //the maximum rate (radians per second)this vehicle can rotate         
        protected float m_dMaxTurnRate;
        protected Vector2 m_vHeading;

        //a vector perpendicular to the heading vector
        protected Vector2 m_vSide;
        protected Vector2 m_vVelocity;


        public MovingEntity(
                            Vector2 position,
                            float radius,
                            Vector2 velocity,
                            float max_speed,
                            Vector2 heading,
                            float mass,
                            Vector2 scale,
                            float turn_rate,
                            float max_force)
            : base(GetNextValidID())
        {
            SetHeading(heading);
            SetVelocity(velocity);
            m_dMass = mass;
            SetMaxSpeed(max_speed);
            SetMaxTurnRate(turn_rate);
            SetMaxForce(max_force);

            SetPos(position);
            SetBRadius(radius);
            SetScale(scale);
        }


        //accessors
        public Vector2 Velocity()
        {
            return m_vVelocity;
        }

        public void SetVelocity(Vector2 NewVel)
        {
            Utils.TestVector(NewVel);
            m_vVelocity = NewVel;
        }

        public float Mass()
        {
            return m_dMass;
        }

        public Vector2 Side()
        {
            return m_vSide;
        }

        public float MaxSpeed()
        {
            return m_dMaxSpeed;
        }

        public void SetMaxSpeed(float new_speed)
        {
//            Utils.TestFloat(new_speed);
            m_dMaxSpeed = new_speed;
        }

        public float MaxForce()
        {
            return m_dMaxForce;
        }

        public void SetMaxForce(float mf)
        {
//            Utils.TestFloat(mf);
            m_dMaxForce = mf;
        }

        public bool IsSpeedMaxedOut()
        {
            return MaxSpeed()*MaxSpeed() >= SpeedSq();
        }

        public float Speed()
        {
            return Velocity().Length();
        }

        public float SpeedSq()
        {
            return Velocity().LengthSquared();
        }

        public Vector2 Heading()
        {
            return m_vHeading;
        }

        public float MaxTurnRate()
        {
            return m_dMaxTurnRate;
        }

        public void SetMaxTurnRate(float val)
        {
//            Utils.TestFloat(val);
            m_dMaxTurnRate = val;
        }


        //--------------------------- RotateHeadingToFacePosition ---------------------
        //
        //  given a target position, this method rotates the entity's heading and
        //  side vectors by an amount not greater than MaxTurnRate until it
        //  directly faces the target.
        //
        //  returns true when the heading is facing in the desired direction
        //-----------------------------------------------------------------------------
        public bool RotateHeadingToFacePosition(Vector2 target)
        {
//            Utils.TestVector(target);
//            Utils.TestVector(target - Pos(), true);
            Vector2 toTarget = Vector2.Normalize(target - Pos());

            float dot = Vector2.Dot(Heading(), toTarget);

            ////some compilers lose acurracy so the value is clamped to ensure it
            ////remains valid for the acos
            Utils.Clamp(ref dot, -1, 1);

            ////first determine the angle between the heading vector and the target
            float angle = (float) Math.Acos(dot);

            ////return true if the player is facing the target
            if (angle < 0.00001) return true;

            ////clamp the amount to turn to the max turn rate
            if (angle > MaxTurnRate()) angle = MaxTurnRate();

            ////The next few lines use a rotation matrix to rotate the player's heading
            ////vector accordingly
            ////notice how the direction of rotation has to be determined when creating
            ////the rotation matrix
            Vector2 heading = Heading();
            SetHeading(Transformations.Vec2DRotateAroundOrigin(ref heading, angle));
          //  SetPos(Transformations.Vec2DRotateAroundOrigin(Pos(), angle));

            return false;
        }


        //------------------------- SetHeading ----------------------------------------
        //
        //  first checks that the given heading is not a vector of zero length. If the
        //  new heading is valid this fumction sets the entity's heading and side 
        //  vectors accordingly
        //-----------------------------------------------------------------------------
        public void SetHeading(Vector2 new_heading)
        {
            if(new_heading == Vector2.Zero)
            {
                throw new Exception("Heading must have directionality!");
            }
            if (float.IsInfinity(new_heading.X) || float.IsInfinity(new_heading.Y))
            {
                // Do not set heading
                return;
            }
                Utils.TestVector(new_heading);
            m_vHeading = new_heading;

            //the side vector must always be perpendicular to the heading
            m_vSide = Heading().Perp();
        }
    }
}