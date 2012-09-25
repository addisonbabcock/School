using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Raven.lua;

namespace Raven.armory
{
    public abstract class Raven_Projectile : MovingEntity
{
        protected static Raven_Scriptor script = Raven_Scriptor.Instance();
        
  //the ID of the entity that fired this
protected   int           m_iShooterID;

  //the place the projectile is aimed at
protected   Vector2      m_vTarget;

  //a pointer to the world data
  protected Raven_Game   m_pWorld;

  //where the projectile was fired from
  protected Vector2      m_vOrigin;

  //how much damage the projectile inflicts
  protected int           m_iDamageInflicted;

  //is it dead? A dead projectile is one that has come to the end of its
  //trajectory and cycled through any explosion sequence. A dead projectile
  //can be removed from the world environment and deleted.
  protected bool          m_bDead;

  //this is set to true as soon as a projectile hits something
  protected bool          m_bImpacted;

  //the position where this projectile impacts an object
  protected Vector2      m_vImpactPoint;

  //this is stamped with the time this projectile was instantiated. This is
  //to enable the shot to be rendered for a specific length of time
  protected DateTime       m_dTimeOfCreation;

  protected AbstractBot            GetClosestIntersectingBot(Vector2 From,
                                                  Vector2 To){
  AbstractBot ClosestIntersectingBot = null;
  float ClosestSoFar = float.MaxValue;

  //iterate through all entities checking against the line segment FromTo
  foreach(AbstractBot curBot in m_pWorld.GetAllBots())
  {
    //make sure we don't check against the shooter of the projectile
    if ( (curBot.ID() != m_iShooterID))
    {
      //if the distance to FromTo is less than the entity's bounding radius then
      //there is an intersection
      if (Geometry.DistToLineSegment(From, To, curBot.Pos()) < curBot.BRadius())
      {
        //test to see if this is the closest so far
        double Dist = Vector2.DistanceSquared(curBot.Pos(), m_vOrigin);

        if (Dist < ClosestSoFar)
        {
          Dist = ClosestSoFar;
          ClosestIntersectingBot = curBot;
        }
      }
    }

  }

  return ClosestIntersectingBot;
}

  protected List<AbstractBot> GetListOfIntersectingBots(Vector2 From,
                                                  Vector2 To){
  //this will hold any bots that are intersecting with the line segment
      List<AbstractBot> hits = new List<AbstractBot>();

  //iterate through all entities checking against the line segment FromTo
      foreach(AbstractBot curBot in m_pWorld.GetAllBots())
      {
    //make sure we don't check against the shooter of the projectile
    if ( (curBot.ID() != m_iShooterID))
    {
      //if the distance to FromTo is less than the entities bounding radius then
      //there is an intersection so add it to hits
      if (Geometry.DistToLineSegment(From, To, curBot.Pos()) < curBot.BRadius())
      {
        hits.Add(curBot);
      }
    }

  }

  return hits;
}




public   Raven_Projectile(Vector2  target,   //the target's position
                   Raven_Game world,  //a pointer to the world data
                   int      ShooterID, //the ID of the bot that fired this shot
                   Vector2 origin,  //the start position of the projectile
                   Vector2 heading,   //the heading of the projectile
                   int      damage,    //how much damage it inflicts
                   float    scale,    
                   float    MaxSpeed, 
                   float    mass,
                   float    MaxForce):  base(origin,
                                                     scale,
                                                    new Vector2(0,0),
                                                     MaxSpeed,
                                                     heading,
                                                     mass,
                                                     new Vector2(scale, scale),
                                                     0, //max turn rate irrelevant here, all shots go straight
                                                     MaxForce)
{

    m_vTarget = target;
    m_bDead = false;
    m_bImpacted = false;
    m_pWorld = world;
    m_iDamageInflicted = damage;
    m_vOrigin = origin;
    m_iShooterID = ShooterID;
                

  m_dTimeOfCreation = DateTime.Now;}

  //unimportant for this class unless you want to implement a full state 
  //save/restore (which can be useful for debugging purposes)
public override  void Write(StreamWriter writer)
{
}

        public  override void Read (StreamReader reader){}

  //must be implemented
        public abstract void Render(PrimitiveBatch batch);
  
  //set to true if the projectile has impacted and has finished any explosion 
  //sequence. When true the projectile will be removed from the game
  public bool isDead(){return m_bDead;}
  
  //true if the projectile has impacted but is not yet dead (because it
  //may be exploding outwards from the point of impact for example)
  public bool HasImpacted(){return m_bImpacted;}



}
}
