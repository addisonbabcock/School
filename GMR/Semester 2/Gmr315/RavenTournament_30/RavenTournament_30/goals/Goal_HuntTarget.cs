using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Microsoft.Xna.Framework;

namespace Raven.goals
{
    public class Goal_HuntTarget :  Goal_Composite
{


  //this value is set to true if the last visible position of the target
  //bot has been searched without success
        private bool m_bLVPTried;
        private AbstractBot owner;


public   Goal_HuntTarget(AbstractBot pBot):base(pBot, (int) Raven_Goal_Types.goal_hunt_target)
  {
    owner = pBot;
      m_bLVPTried = false;
  }


public override  void Render(PrimitiveBatch batch)
{
    //#define SHOW_LAST_RECORDED_POSITION
#if SHOW_LAST_RECORDED_POSITION
  //render last recorded position as a green circle
  if (m_pOwner.GetTargetSys().isTargetPresent())
  {
    gdi.GreenPen();
    gdi.RedBrush();
    gdi.Circle(m_pOwner.GetTargetSys().GetLastRecordedPosition(), 3);
  }
#endif

 //forward the request to the subgoals
  base.Render(batch);
}


        public override void Activate()
        {
            m_iStatus = (int) GoalStatus.active;

            //if this goal is reactivated then there may be some existing subgoals that
            //must be removed
            RemoveAllSubgoals();

            //it is possible for the target to die whilst this goal is active so we
            //must test to make sure the bot always has an active target
            if (owner.GetTargetSys().isTargetPresent())
            {
                //grab a local copy of the last recorded position (LRP) of the target
                Vector2 lrp = owner.GetTargetSys().GetLastRecordedPosition();

                //if the bot has reached the LRP and it still hasn't found the target
                //it starts to search by using the explore goal to move to random
                //map locations
                if (lrp == Vector2.Zero || owner.isAtPosition(lrp))
                {
                    AddSubgoal(new Goal_Explore(owner));
                }

                //else move to the LRP
                else
                {
                    AddSubgoal(new Goal_MoveToPosition(owner, lrp));
                }
            }

            //if their is no active target then this goal can be removed from the queue
            else
            {
                m_iStatus = (int) GoalStatus.completed;
            }
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            m_iStatus = ProcessSubgoals();

            //if target is in view this goal is satisfied
            if (owner.GetTargetSys().isTargetWithinFOV())
            {
                m_iStatus = (int) GoalStatus.completed;
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
            
        }
}
}
