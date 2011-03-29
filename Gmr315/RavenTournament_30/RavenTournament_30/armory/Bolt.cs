using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.armory
{
    public class Bolt : Raven_Projectile
    {


        //tests the trajectory of the shell for an impact
        private void TestForImpact()
        {
            if (!m_bImpacted)
            {
                SetVelocity(MaxSpeed() * Heading());

                //make sure vehicle does not exceed maximum velocity
                SetVelocity(Velocity().Truncate(MaxSpeed()));

                //update the position
                SetPos(Pos() + Velocity());


                //if the projectile has reached the target position or it hits an entity
                //or wall it should explode/inflict damage/whatever and then mark itself
                //as dead


                //test to see if the line segment connecting the bolt's current position
                //and previous position intersects with any bots.
                AbstractBot hit = GetClosestIntersectingBot(Pos() - Velocity(),
                                                           Pos());

                //if hit
                if (hit != null)
                {
                    m_bImpacted = true;
                    m_bDead = true;

                    //send a message to the bot to let it know it's been hit, and who the
                    //shot came from
                    Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                            m_iShooterID,
                                            hit.ID(),
                                            (int)message_type.Msg_TakeThatMF,
                                            m_iDamageInflicted);
                }

                //test for impact with a wall
                float dist = 0.0f;
                IEnumerable<Wall2D> walls = m_pWorld.GetMap().GetWalls();

                if (WallIntersectionTests.FindClosestPointOfIntersectionWithWalls(Pos() - Velocity(),
                                                            Pos(),
                                                            ref dist,
                                                            ref m_vImpactPoint,
                                                            ref walls))
                {
                    m_bDead = true;
                    m_bImpacted = true;

                    SetPos(m_vImpactPoint);

                    return;
                }
            }
        }



        public Bolt(AbstractBot shooter, Vector2 target)
            : base(target,
                shooter.GetWorld(),
                shooter.ID(),
                shooter.Pos(),
                shooter.Facing(),
                script.GetInt("Bolt_Damage"),
                script.GetDouble("Bolt_Scale"),
                script.GetDouble("Bolt_MaxSpeed"),
                script.GetDouble("Bolt_Mass"),
                script.GetDouble("Bolt_MaxForce"))
        {
            if (target == new Vector2())
            {
                throw new Exception("Original Code Assertion");
            }
        }

        public override void Render(PrimitiveBatch batch)
        {
            Drawing.DrawLine(batch, Pos(), Pos() - Velocity(), Color.DarkGreen);
        }

        public override void Update()
        {
            TestForImpact();
        }

    }
}
