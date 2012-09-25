using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace Raven.goals
{
    public class Goal_DodgeSideToSide: Goal
{


private   Vector2    m_vStrafeTarget;
        private AbstractBot owner;
  private   bool        m_bClockwise;

  //private   Vector2  GetStrafeTarget();




  public Goal_DodgeSideToSide(AbstractBot pBot)
      : base(pBot, (int)Raven_Goal_Types.goal_strafe)
  {
      owner = pBot;
      m_bClockwise = Utils.RandBool();
  }


        public override void Activate()
  {
      m_iStatus = (int) GoalStatus.active;

      owner.GetSteering().SeekOn();


      if (m_bClockwise)
      {
          if (owner.canStepRight(ref m_vStrafeTarget))
          {
              owner.GetSteering().SetTarget(m_vStrafeTarget);
          }
          else
          {
              //debug_con << "changing" << "";
              m_bClockwise = !m_bClockwise;
              m_iStatus = (int)GoalStatus.inactive;
          }
      }

      else
      {
          if (owner.canStepLeft(ref m_vStrafeTarget))
          {
              owner.GetSteering().SetTarget(m_vStrafeTarget);
          }
          else
          {
              // debug_con << "changing" << "";
              m_bClockwise = !m_bClockwise;
              m_iStatus = (int)GoalStatus.inactive;
          }
      }


  }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //if target goes out of view terminate
            if (!owner.GetTargetSys().isTargetWithinFOV())
            {
                m_iStatus = (int)GoalStatus.completed;
            }

            //else if bot reaches the target position set status to inactive so the goal 
            //is reactivated on the next update-step
            else if (owner.isAtPosition(m_vStrafeTarget))
            {
                m_iStatus = (int)GoalStatus.inactive;
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
            owner.GetSteering().SeekOff();
        }


        public override void Render(PrimitiveBatch batch){
//#define SHOW_TARGET
#if SHOW_TARGET
  gdi.OrangePen();
  gdi.HollowBrush();

  gdi.Line(m_pOwner.Pos(), m_vStrafeTarget);
  gdi.Circle(m_vStrafeTarget, 3);
#endif
  
}
 
}
}
