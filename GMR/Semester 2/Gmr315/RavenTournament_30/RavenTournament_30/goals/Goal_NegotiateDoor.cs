using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;
using Microsoft.Xna.Framework;
using Raven.navigation;

namespace Raven.goals
{
    public class Goal_NegotiateDoor: Goal_Composite
{

        private AbstractBot owner;
private   PathEdge m_PathEdge;

  bool     m_bLastEdgeInPath;



  public Goal_NegotiateDoor(AbstractBot pBot, PathEdge edge, bool LastEdge):base(pBot, (int) Raven_Goal_Types.goal_negotiate_door)
  {
      owner = pBot;
      m_PathEdge = edge;
      m_bLastEdgeInPath = LastEdge;


}

 //the usual suspects

        public override void Activate()
  {
      m_iStatus = (int) GoalStatus.active;

      //if this goal is reactivated then there may be some existing subgoals that
      //must be removed
      RemoveAllSubgoals();

      //get the position of the closest navigable switch
      Vector2 posSw = owner.GetWorld().GetPosOfClosestSwitch(m_pOwner.Pos(),
                                                              m_PathEdge.DoorID());

      //because goals are *pushed* onto the front of the subgoal list they must
      //be added in reverse order.

      //first the goal to traverse the edge that passes through the door
      AddSubgoal(new Goal_TraverseEdge(owner, m_PathEdge, m_bLastEdgeInPath));

      //next, the goal that will move the bot to the beginning of the edge that
      //passes through the door
      AddSubgoal(new Goal_MoveToPosition(owner, m_PathEdge.Source()));

      //finally, the Goal that will direct the bot to the location of the switch
      AddSubgoal(new Goal_MoveToPosition(owner, posSw));
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
}
}
