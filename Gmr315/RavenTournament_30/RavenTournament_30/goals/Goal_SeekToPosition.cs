using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class Goal_SeekToPosition: Goal
{
        private AbstractBot owner;

  //the position the bot is moving to
private   Vector2  m_vPosition;

  //the approximate time the bot should take to travel the target location
  private  float     m_dTimeToReachPos;
  
  //this records the time this goal was activated
  private DateTime     m_dStartTime;

  //returns true if a bot gets stuck
  private bool      isStuck()
  {
     
      if (DateTime.Now > m_dStartTime.AddSeconds(m_dTimeToReachPos))
      {
          Debug.WriteLine("BOT " + m_pOwner.ID() + " IS STUCK!!");

          return true;
      }

      return false;
  }

        private void SetPos(Vector2 position)
        {
            m_vPosition = position;
            if(m_vPosition.X < 0 || m_vPosition.Y < 0)
            {
                throw new Exception();
            }
        }

        private Vector2 Pos()
        {
            return m_vPosition;
        }
public   Goal_SeekToPosition(AbstractBot pBot, Vector2 target): base(pBot, (int) Raven_Goal_Types.goal_seek_to_position)
{
    owner = pBot;
    SetPos(target);
    m_dTimeToReachPos = 0.0f;
}

  //the usual suspects

        public override void Activate()
        {
            m_iStatus = (int) GoalStatus.active;

            //record the time the bot starts this goal
            m_dStartTime = DateTime.Now;

            //This value is used to determine if the bot becomes stuck 
            m_dTimeToReachPos = owner.CalculateTimeToReachPosition(Pos());

            //factor in a margin of error for any reactive behavior
            const float MarginOfError = 1.0f;

            m_dTimeToReachPos += MarginOfError;


            owner.GetSteering().SetTarget(Pos());

            owner.GetSteering().SeekOn();
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //test to see if the bot has become stuck
            if (isStuck())
            {
                m_iStatus = (int) GoalStatus.failed;
            }

            //test to see if the bot has reached the waypoint. If so terminate the goal
            else
            {
                if (owner.isAtPosition(Pos()))
                {
                    m_iStatus = (int) GoalStatus.completed;
                }
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
            owner.GetSteering().SeekOff();
            owner.GetSteering().ArriveOff();

            m_iStatus = (int) GoalStatus.completed;
        }

        public override void Render(PrimitiveBatch batch)
        {
            if(m_iStatus == (int) GoalStatus.active)
            {
                Drawing.DrawCircle(batch, Pos(), 3, Color.DarkOliveGreen);
            } else if (m_iStatus == (int) GoalStatus.inactive)
            {
                Drawing.DrawCircle(batch, Pos(), 3, Color.DarkOrchid);
            }
        }
}
}
