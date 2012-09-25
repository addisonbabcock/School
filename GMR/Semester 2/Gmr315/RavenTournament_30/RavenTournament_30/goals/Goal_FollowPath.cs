using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Graph;
using Microsoft.Xna.Framework.Graphics;
using Raven.navigation;

namespace Raven.goals
{
    public class Goal_FollowPath : Goal_Composite
    {


        //a local copy of the path returned by the path planner
        private List<PathEdge> m_Path;


        private AbstractBot owner;
        public Goal_FollowPath(AbstractBot pBot, List<PathEdge> path)
            : base(pBot, (int)Raven_Goal_Types.goal_follow_path)
        {
            owner = pBot;
            m_Path = path;

        }

        //the usual suspects

        public override void Activate()
        {
              m_iStatus = (int) GoalStatus.active;
  
  //get a reference to the next edge
  PathEdge edge = m_Path[0];

  //remove the edge from the path
            m_Path.RemoveAt(0);

  //some edges specify that the bot should use a specific behavior when
  //following them. This switch statement queries the edge behavior flag and
  //adds the appropriate goals/s to the subgoal list.
  switch(edge.Behavior())
  {
  case (int) NavGraphEdge.GraphEdgeFlags.normal:
    {
        AddSubgoal(new Goal_TraverseEdge(owner, edge, m_Path.Count() == 0));
    }

    break;

  case (int) NavGraphEdge.GraphEdgeFlags.goes_through_door:
    {

      //also add a goal that is able to handle opening the door
        AddSubgoal(new Goal_NegotiateDoor(owner, edge, m_Path.Count() == 0));
    }

    break;

  case (int) NavGraphEdge.GraphEdgeFlags.jump:
    {
      //add subgoal to jump along the edge
    }

    break;

  case (int)NavGraphEdge.GraphEdgeFlags.grapple:
    {
      //add subgoal to grapple along the edge
    }

    break;

  default:

    throw new Exception("<Goal_FollowPath::Activate>: Unrecognized edge type");
  }
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            m_iStatus = ProcessSubgoals();

            //if there are no subgoals present check to see if the path still has edges.
            //remaining. If it does then call activate to grab the next edge.
            if (m_iStatus == (int)GoalStatus.completed && m_Path.Count != 0)
            {
                Activate();
            }

            return m_iStatus;
        }

        public override void Terminate()
        {
        }

        public override void Render(PrimitiveBatch batch)
        {
            //render all the path waypoints remaining on the path list
            foreach (PathEdge it in m_Path)
            {
                Drawing.DrawLineWithArrow(batch, it.Source(), it.Destination(), 5, Color.Black);

                Drawing.DrawCircle(batch, it.Destination(), 3, Color.DarkOrchid);
            }
            base.Render(batch);
        }



    }
}
