using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;

namespace Raven.armory
{
    public class Rocket : Raven_Projectile
    {


        //the radius of damage, once the rocket has impacted
        private float m_dBlastRadius;

        //this is used to render the splash when the rocket impacts
        private float m_dCurrentBlastRadius;

        //If the rocket has impacted we test all bots to see if they are within the 
        //blast radius and reduce their health accordingly
        private void InflictDamageOnBotsWithinBlastRadius()
        {
            foreach (AbstractBot curBot in m_pWorld.GetAllBots())
            {
                if (Vector2.Distance(Pos(), curBot.Pos()) < m_dBlastRadius + curBot.BRadius())
                {
                    //send a message to the bot to let it know it's been hit, and who the
                    //shot came from
                    Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                            m_iShooterID,
                                            curBot.ID(),
                                            (int)message_type.Msg_TakeThatMF,
                                            m_iDamageInflicted);

                }
            }
        }

        //tests the trajectory of the shell for an impact
        private void TestForImpact()
        {

            //if the projectile has reached the target position or it hits an entity
            //or wall it should explode/inflict damage/whatever and then mark itself
            //as dead


            //test to see if the line segment connecting the rocket's current position
            //and previous position intersects with any bots.
            AbstractBot hit = GetClosestIntersectingBot(Pos() - Velocity(), Pos());

            //if hit
            if (hit != null)
            {
                m_bImpacted = true;

                //send a message to the bot to let it know it's been hit, and who the
                //shot came from
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                        m_iShooterID,
                                        hit.ID(),
                                        (int)message_type.Msg_TakeThatMF,
                                        m_iDamageInflicted);

                //test for bots within the blast radius and inflict damage
                InflictDamageOnBotsWithinBlastRadius();
            }

            //test for impact with a wall
            float dist = 0;
            IEnumerable<Wall2D> walls = m_pWorld.GetMap().GetWalls();
            if (WallIntersectionTests.FindClosestPointOfIntersectionWithWalls(Pos() - Velocity(),
                                                        Pos(),
                                                        ref dist,
                                                        ref m_vImpactPoint,
                                                       ref walls))
            {
                m_bImpacted = true;

                //test for bots within the blast radius and inflict damage
                InflictDamageOnBotsWithinBlastRadius();

                SetPos(m_vImpactPoint);

                return;
            }

            //test to see if rocket has reached target position. If so, test for
            //all bots in vicinity
            const double tolerance = 5.0;
            if (Vector2.DistanceSquared(Pos(), m_vTarget) < tolerance * tolerance)
            {
                m_bImpacted = true;

                InflictDamageOnBotsWithinBlastRadius();
            }
        }



        public Rocket(AbstractBot shooter, Vector2 target) :
            base(target,
                 shooter.GetWorld(),
                 shooter.ID(),
                 shooter.Pos(),
                 shooter.Facing(),
                 script.GetInt("Rocket_Damage"),
                 script.GetDouble("Rocket_Scale"),
                 script.GetDouble("Rocket_MaxSpeed"),
                 script.GetDouble("Rocket_Mass"),
                 script.GetDouble("Rocket_MaxForce"))
        {

            m_dCurrentBlastRadius = 0.0f;
            m_dBlastRadius = script.GetDouble("Rocket_BlastRadius");
            if (target == new Vector2())
            {
                throw new Exception("Original Code Assertion");
            }
        }

        public override void Render(PrimitiveBatch batch)
        {
            Drawing.DrawCircle(batch, Pos(), 2, Color.OrangeRed);
            if (m_bImpacted)
            {
                Drawing.DrawCircle(batch, Pos(), m_dCurrentBlastRadius, Color.Red);
            }

        }

        public override void Update()
        {
            if (!m_bImpacted)
            {
                SetVelocity(MaxSpeed() * Heading());

                //make sure vehicle does not exceed maximum velocity
                SetVelocity(Velocity().Truncate(MaxSpeed()));

                //update the position
                SetPos(Pos() + Velocity());

                TestForImpact();
            }

            else
            {
                m_dCurrentBlastRadius += script.GetDouble("Rocket_ExplosionDecayRate");

                //when the rendered blast circle becomes equal in size to the blast radius
                //the rocket can be removed from the game
                if (m_dCurrentBlastRadius > m_dBlastRadius)
                {
                    m_bDead = true;
                }
            }
        }
    }
}
