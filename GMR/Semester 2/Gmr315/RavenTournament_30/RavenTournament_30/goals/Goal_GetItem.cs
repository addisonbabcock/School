using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;
using Common.Messaging;
using Common.Triggers;

namespace Raven.goals
{
    public class Goal_GetItem : Goal_Composite
    {

        private AbstractBot owner;
        public static int ItemTypeToGoalType(int gt)
{
  switch(gt)
  {
  case (int) Raven_Objects.type_health:

    return (int) Raven_Goal_Types.goal_get_health;

  case (int) Raven_Objects.type_shotgun:

    return (int) Raven_Goal_Types.goal_get_shotgun;

  case (int) Raven_Objects.type_rail_gun:

    return (int) Raven_Goal_Types.goal_get_railgun;

  case (int) Raven_Objects.type_rocket_launcher:

    return (int) Raven_Goal_Types.goal_get_rocket_launcher;

  default: throw new Exception("Goal_GetItem cannot determine item type");

  }//end switch
}

        private int m_iItemToGet;

        private Trigger<AbstractBot> m_pGiverTrigger;

        //true if a path to the item has been formulated
        private bool m_bFollowingPath;

        //returns true if the bot sees that the item it is heading for has been
        //picked up by an opponent
        private bool hasItemBeenStolen()
        {
            if (m_pGiverTrigger != null &&
                !m_pGiverTrigger.isActive() &&
                owner.hasLOSto(m_pGiverTrigger.Pos()))
            {
                return true;
            }

            return false;
        }



        public Goal_GetItem(AbstractBot pBot,
                       int item)
            : base(pBot,
                                ItemTypeToGoalType(item))
        {
            owner = pBot;
            m_iItemToGet = item;
            m_pGiverTrigger = null;
            m_bFollowingPath = false;
        }


        public override void Activate()
        {
            m_iStatus = (int) GoalStatus.active;

            m_pGiverTrigger = null;

            //request a path to the item
            owner.GetPathPlanner().RequestPathToItem(m_iItemToGet);

            //the bot may have to wait a few update cycles before a path is calculated
            //so for appearances sake it just wanders
            AddSubgoal(new Goal_Wander(owner));

        }

        public override int Process()
        {
            ActivateIfInactive();

            if (hasItemBeenStolen())
            {
                Terminate();
            }

            else
            {
                //process the subgoals
                m_iStatus = ProcessSubgoals();
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
            m_iStatus = (int)GoalStatus.completed;
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

                        //get the pointer to the item
                        m_pGiverTrigger = (Trigger<AbstractBot>) (msg.ExtraInfo);

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
