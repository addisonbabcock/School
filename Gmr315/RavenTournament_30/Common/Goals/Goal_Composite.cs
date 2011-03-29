using System.Collections.Generic;
using System.Linq;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Goals
{
    public abstract class Goal_Composite : Goal
    {
        //composite goals may have any number of subgoals
        protected List<Goal> m_SubGoals;

        public Goal_Composite(BaseGameEntity pE, int type) : base(pE, type)
        {
            m_SubGoals = new List<Goal>();
        }

        public new void Dispose()
        {
            base.Dispose();
            RemoveAllSubgoals();
            m_SubGoals = null;
        }


        //processes any subgoals that may be present
        protected int ProcessSubgoals()
        {
            //remove all completed and failed goals from the front of the subgoal list
            while (m_SubGoals.Count != 0 && (m_SubGoals[0].isComplete() || m_SubGoals[0].hasFailed()))
            {
                m_SubGoals[0].Terminate();
                m_SubGoals[0].Dispose();
                m_SubGoals.RemoveAt(0);
            }


            //if any subgoals remain, process the one at the front of the list
            if (m_SubGoals.Count != 0)
            {
                //grab the status of the front-most subgoal
                int StatusOfSubGoals = m_SubGoals[0].Process();

                //we have to test for the special case where the front-most subgoal
                //reports 'completed' *and* the subgoal list contains additional goals.When
                //this is the case, to ensure the parent keeps processing its subgoal list
                //we must return the 'active' status.
                if (StatusOfSubGoals == (int) GoalStatus.completed && m_SubGoals.Count() > 1)
                {
                    return (int) GoalStatus.active;
                }

                return StatusOfSubGoals;
            }
  
                //no more subgoals to process - return 'completed'
            else
            {
                return (int) GoalStatus.completed;
            }
        }

        //passes the message to the front-most subgoal
        protected bool ForwardMessageToFrontMostSubgoal(Telegram msg)
        {
            if (m_SubGoals.Count != 0)
            {
                return m_SubGoals[0].HandleMessage(msg);
            }

            //return false if the message has not been handled
            return false;
        }


        //when this object is destroyed make sure any subgoals are terminated
        //and destroyed.
        ~Goal_Composite()
        {
            RemoveAllSubgoals();
        }

        //if a child class of Goal_Composite does not define a message handler
        //the default behavior is to forward the message to the front-most
        //subgoal
        public override bool HandleMessage(Telegram msg)
        {
            return ForwardMessageToFrontMostSubgoal(msg);
        }

        //adds a subgoal to the front of the subgoal list
        public override void AddSubgoal(Goal g)
        {
            //add the new goal to the front of the list
            m_SubGoals.Insert(0, g);
        }

        //this method iterates through the subgoals and calls each one's Terminate
        //method before deleting the subgoal and removing it from the subgoal list
        public void RemoveAllSubgoals()
        {
            foreach (Goal it in m_SubGoals)
            {
                it.Terminate();
                it.Dispose();
            }
            m_SubGoals.Clear();
        }


        public override void RenderAtPos(SpriteBatch batch, SpriteFont font, ref Vector2 pos, Convert tts)
        {
            base.RenderAtPos(batch, font, ref pos, tts);

            pos.X += 10;

            foreach (Goal it in m_SubGoals)
            {
                it.RenderAtPos(batch, font, ref pos, tts);
            }

            pos.X -= 10;
        }

        //this is only used to render information for debugging purposes
        public override void Render(PrimitiveBatch batch)
        {
            if (m_SubGoals.Count != 0)
            {
                m_SubGoals[0].Render(batch);
            }
        }
    }
}