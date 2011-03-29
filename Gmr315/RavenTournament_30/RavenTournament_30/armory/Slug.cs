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
    public class Slug : Raven_Projectile
{


  //when this projectile hits something it's trajectory is rendered
  //for this amount of time
private   float   m_dTimeShotIsVisible;

  //tests the trajectory of the shell for an impact
  private void  TestForImpact(){
  // a rail gun slug travels VERY fast. It only gets the chance to update once 
  m_bImpacted = true;

  //first find the closest wall that this ray intersects with. Then we
  //can test against all entities within this range.
  float DistToClosestImpact = 0.0f;
  IEnumerable<Wall2D> walls = m_pWorld.GetMap().GetWalls();
  WallIntersectionTests.FindClosestPointOfIntersectionWithWalls(m_vOrigin,
                                          Pos(),
                                          ref DistToClosestImpact,
                                          ref m_vImpactPoint,
                                          ref walls);

  //test to see if the ray between the current position of the slug and 
  //the start position intersects with any bots.
  List<AbstractBot> hits = GetListOfIntersectingBots(m_vOrigin, Pos());

  //if no bots hit just return;
  if (hits.Count == 0) return;

  //give some damage to the hit bots
      foreach(AbstractBot bot in hits)
      {
          
    //send a message to the bot to let it know it's been hit, and who the
    //shot came from
    Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                            m_iShooterID,
                            bot.ID(),
                            (int) message_type.Msg_TakeThatMF,
                            m_iDamageInflicted);
    
  }
}

    //returns true if the shot is still to be rendered
  private bool  isVisibleToPlayer()
  {return DateTime.Now < m_dTimeOfCreation.AddSeconds(m_dTimeShotIsVisible);}



  public Slug(AbstractBot shooter, Vector2 target):base(target,
                         shooter.GetWorld(),
                         shooter.ID(),
                         shooter.Pos(),
                         shooter.Facing(),
                         script.GetInt("Slug_Damage"),
                         script.GetDouble("Slug_Scale"),
                         script.GetDouble("Slug_MaxSpeed"),
                         script.GetDouble("Slug_Mass"),
                         script.GetDouble("Slug_MaxForce"))
    {
        m_dTimeShotIsVisible = script.GetDouble("Slug_Persistance");

  
}

  public override void Render(PrimitiveBatch batch)
  {
      if (isVisibleToPlayer() && m_bImpacted)
      {
          Drawing.DrawLine(batch, m_vOrigin, m_vImpactPoint, Color.Green);
      }
  }

  public override void Update()
  {
      if (!HasImpacted())
      {
          //calculate the steering force
          Vector2 DesiredVelocity = Vector2.Normalize(m_vTarget - Pos())*MaxSpeed();

          Vector2 sf = DesiredVelocity - Velocity();

          //update the position
          Vector2 accel = sf/Mass();

          SetVelocity(Velocity()+ accel);

          //make sure the slug does not exceed maximum velocity
          SetVelocity(Velocity().Truncate(MaxSpeed()));

          //update the position
          SetPos(Pos() + Velocity());

          TestForImpact();
      }
      else if (!isVisibleToPlayer())
      {
          m_bDead = true;
      }
  }
}
}
