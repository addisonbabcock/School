using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;
using Common.Messaging;
using Microsoft.Xna.Framework;

namespace Raven.goals
{
    public class Goal_Explore : Goal_Composite
{

        private AbstractBot owner;
private   Vector2  m_CurrentDestination;

  //set to true when the destination for the exploration has been established
 private  bool      m_bDestinationIsSet;



public   Goal_Explore(AbstractBot pOwner):base(pOwner,
                                                            (int) Raven_Goal_Types.goal_explore)
{
    owner = pOwner;
    m_bDestinationIsSet = false;
  }

        public override void Activate()
        {
            m_iStatus = (int) GoalStatus.active;

            //if this goal is reactivated then there may be some existing subgoals that
            //must be removed
            RemoveAllSubgoals();

            if (!m_bDestinationIsSet)
            {
                //grab a random location
                m_CurrentDestination = owner.GetWorld().GetMap().GetRandomNodeLocation();

                m_bDestinationIsSet = true;
            }

            //and request a path to that position
            owner.GetPathPlanner().RequestPathToPosition(m_CurrentDestination);

            //the bot may have to wait a few update cycles before a path is calculated
            //so for appearances sake it simple ARRIVES at the destination until a path
            //has been found
            AddSubgoal(new Goal_SeekToPosition(owner, m_CurrentDestination));
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //process the subgoals
            m_iStatus = ProcessSubgoals();

            return m_iStatus;
        }

        public override void Terminate()
        {
            
        }

        public override bool HandleMessage(Telegram msg)
        {
            //first, pass the message down the goal hierarchy
            bool bHandled = ForwardMessageToFrontMostSubgoal(msg);

            //if the msg was not handled, test to see if this goal can handle it
            if (bHandled == false)
            {
                switch (msg.Msg)
                {
                    case (int) message_type.Msg_PathReady:

                        //clear any existing goals
                        RemoveAllSubgoals();

                        AddSubgoal(new Goal_FollowPath(owner,
                                                       owner.GetPathPlanner().GetPath()));

                        return true; //msg handled


                    case (int)message_type.Msg_NoPathAvailable:

                        m_iStatus = (int) GoalStatus.failed;

                        return true; //msg handled

                    default: return false;
                }
            }

            //handled by subgoals
            return true;
        }
}
}
