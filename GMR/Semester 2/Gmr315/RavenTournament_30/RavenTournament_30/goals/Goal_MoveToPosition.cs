using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Goals;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven.goals
{
    public class Goal_MoveToPosition : Goal_Composite
    {


        //the position the bot wants to reach
        private Vector2 m_vDestination;
        private AbstractBot owner;

        public

          Goal_MoveToPosition(AbstractBot pBot,
                              Vector2 pos) :

            base(pBot,
                                             (int)Raven_Goal_Types.goal_move_to_position)
        {
            owner = pBot;
            m_vDestination = pos;
        }


        //this goal is able to accept messages
        public override void Activate()
        {
            m_iStatus = (int)GoalStatus.active;

            //make sure the subgoal list is clear.
            RemoveAllSubgoals();

            //requests a path to the target position from the path planner. Because, for
            //demonstration purposes, the Raven path planner uses time-slicing when 
            //processing the path requests the bot may have to wait a few update cycles
            //before a path is calculated. Consequently, for appearances sake, it just
            //seeks directly to the target position whilst it's awaiting notification
            //that the path planning request has succeeded/failed
            if (owner.GetPathPlanner().RequestPathToPosition(m_vDestination))
            {
                AddSubgoal(new Goal_SeekToPosition(owner, m_vDestination));
            }
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //process the subgoals
            m_iStatus = ProcessSubgoals();

            //if any of the subgoals have failed then this goal re-plans
            ReactivateIfFailed();

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
                    case (int)message_type.Msg_PathReady:

                        //clear any existing goals
                        RemoveAllSubgoals();

                        AddSubgoal(new Goal_FollowPath(owner,
                                                       owner.GetPathPlanner().GetPath()));

                        return true; //msg handled


                    case (int)message_type.Msg_NoPathAvailable:

                        m_iStatus = (int)GoalStatus.failed;

                        return true; //msg handled

                    default:
                        return false;
                }
            }

            //handled by subgoals
            return true;
        }


        public override void Render(PrimitiveBatch batch)
        {
            //forward the request to the subgoals
            base.Render(batch);

            //draw a bullseye
            Drawing.DrawCircle(batch, m_vDestination, 6, Color.DarkSlateBlue);
            Drawing.DrawCircle(batch, m_vDestination, 4, Color.Red);
            Drawing.DrawCircle(batch, m_vDestination, 2, Color.Yellow);

        }
    }
}
