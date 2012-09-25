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


        public MovingEntity(Microsoft.Xna.Framework.Game game,
                            Vector2 position,
                            float radius,
                            Vector2 velocity,
                            float max_speed,
                            Vector2 heading,
                            float mass,
                            Vector2 scale,
                            float turn_rate,
                            float max_force)
            : base(game, GetNextValidID())
        {
            m_vHeading = heading;
            m_vVelocity = velocity;
            m_dMass = mass;
            m_vSide = m_vHeading.Perp();
            m_dMaxSpeed = max_speed;
            m_dMaxTurnRate = turn_rate;
            m_dMaxForce = max_force;

            m_vPosition = position;
            m_dBoundingRadius = radius;
            m_vScale = scale;
        }


        //accessors
        public Vector2 Velocity()
        {
            return m_vVelocity;
        }

        public void SetVelocity(Vector2 NewVel)
        {
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
            m_dMaxSpeed = new_speed;
        }

        public float MaxForce()
        {
            return m_dMaxForce;
        }

        public void SetMaxForce(float mf)
        {
            m_dMaxForce = mf;
        }

        public bool IsSpeedMaxedOut()
        {
            return m_dMaxSpeed*m_dMaxSpeed >= m_vVelocity.LengthSquared();
        }

        public float Speed()
        {
            return m_vVelocity.Length();
        }

        public float SpeedSq()
        {
            return m_vVelocity.LengthSquared();
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
            m_dMaxTurnRate = val;
        }


        //--------------------------- RotateHeadingToFacePosition ---------------------
        //
        //  given a target position, this method rotates the entity's heading and
        //  side vectors by an amount not greater than m_dMaxTurnRate until it
        //  directly faces the target.
        //
        //  returns true when the heading is facing in the desired direction
        //-----------------------------------------------------------------------------
        public bool RotateHeadingToFacePosition(Vector2 target)
        {
            Vector2 toTarget = Vector2.Normalize(target - m_vPosition);

            float dot = Vector2.Dot(m_vHeading, toTarget);

            ////some compilers lose acurracy so the value is clamped to ensure it
            ////remains valid for the acos
            dot = Utils.Clamp(dot, -1, 1);

            ////first determine the angle between the heading vector and the target
            float angle = (float) Math.Acos(dot);

            ////return true if the player is facing the target
            if (angle < 0.00001) return true;

            ////clamp the amount to turn to the max turn rate
            if (angle > m_dMaxTurnRate) angle = m_dMaxTurnRate;

            ////The next few lines use a rotation matrix to rotate the player's heading
            ////vector accordingly
            //Matrix RotationMatrix = Matrix.Identity;
//            Matrix RotationMatrix = Matrix.CreateRotationY(angle);

            ////notice how the direction of rotation has to be determined when creating
            ////the rotation matrix
            m_vHeading = Transformations.Vec2DRotateAroundOrigin(m_vHeading, angle);
            // m_vPosition = Transformations.Vec2DRotateAroundOrigin(m_vPosition, angle);

            ////finally recreate m_vSide
            m_vSide = m_vHeading.Perp();

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
            if (new_heading.LengthSquared() - 1.0 >= 0.00001)
            {
                throw new Exception("Original code assertion!");
            }

            m_vHeading = new_heading;

            //the side vector must always be perpendicular to the heading
            m_vSide = m_vHeading.Perp();
        }
    }
}