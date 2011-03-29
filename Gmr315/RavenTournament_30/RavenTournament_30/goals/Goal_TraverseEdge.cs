using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Graph;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;
using Raven.navigation;

namespace Raven.goals
{
    public class Goal_TraverseEdge: Goal
{
        private AbstractBot owner;

        protected static Raven_Scriptor script = Raven_Scriptor.Instance();
  //the edge the bot will follow
private   PathEdge  m_Edge;

  //true if m_Edge is the last in the path.
private   bool      m_bLastEdgeInPath;

  //the estimated time the bot should take to traverse the edge
private   float     m_dTimeExpected;
  
  //this records the time this goal was activated
private   DateTime     m_dStartTime;

  //returns true if the bot gets stuck
private bool isStuck()
{
    if (DateTime.Now > m_dStartTime.AddSeconds(m_dTimeExpected))
    {
        Console.WriteLine("BOT " + m_pOwner.ID() + " IS STUCK!!");

        return true;
    }

    return false;
}



public Goal_TraverseEdge(AbstractBot pBot,
                    PathEdge   edge,
                    bool       LastEdge):

                                base(pBot, (int) Raven_Goal_Types.goal_traverse_edge)
{
    owner = pBot;
    m_Edge = edge;
    m_dTimeExpected = 0.0f;
    m_bLastEdgeInPath = LastEdge;
   
}




public override void Render(PrimitiveBatch batch)
{
    if (m_iStatus == (int) GoalStatus.active)
    {
        Drawing.DrawLine(batch, m_pOwner.Pos(), m_Edge.Destination(), Color.Blue);
        Drawing.DrawCircle(batch, m_Edge.Destination(), 3, Color.DarkOliveGreen);
    }
}
        public override void Activate()
       {
  m_iStatus = (int) GoalStatus.active;
  
  //the edge behavior flag may specify a type of movement that necessitates a 
  //change in the bot's max possible speed as it follows this edge
  switch(m_Edge.Behavior())
  {
    case (int) NavGraphEdge.GraphEdgeFlags.swim:
    {
        owner.SetMaxSpeed(script.GetDouble("Bot_MaxSwimmingSpeed"));
    }
   
    break;
   
    case (int) NavGraphEdge.GraphEdgeFlags.crawl:
    {
        owner.SetMaxSpeed(script.GetDouble("Bot_MaxCrawlingSpeed"));
    }
   
    break;
  }
  

  //record the time the bot starts this goal
            m_dStartTime = DateTime.Now;
  
  //calculate the expected time required to reach the this waypoint. This value
  //is used to determine if the bot becomes stuck 
            m_dTimeExpected = owner.CalculateTimeToReachPosition(m_Edge.Destination());
  
  //factor in a margin of error for any reactive behavior
  const float MarginOfError = 2.0f;

  m_dTimeExpected += MarginOfError;


  //set the steering target
  owner.GetSteering().SetTarget(m_Edge.Destination());

  //Set the appropriate steering behavior. If this is the last edge in the path
  //the bot should arrive at the position it points to, else it should seek
  if (m_bLastEdgeInPath)
  {
      owner.GetSteering().ArriveOn();
  }

  else
  {
      owner.GetSteering().SeekOn();
  }
}

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //if the bot has become stuck return failure
            if (isStuck())
            {
                m_iStatus = (int) GoalStatus.failed;
            }

            //if the bot has reached the end of the edge return completed
            else
            {
                if (owner.isAtPosition(m_Edge.Destination()))
                {
                    m_iStatus = (int)GoalStatus.completed;
                }
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
            //turn off steering behaviors.
            owner.GetSteering().SeekOff();
            owner.GetSteering().ArriveOff();

            //return max speed back to normal
            owner.SetMaxSpeed(script.GetDouble("Bot_MaxSpeed"));
        }
}
}
