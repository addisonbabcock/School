using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Goals
{
    public abstract class Goal
    {
        public delegate string Convert(int gt);

        public void Dispose()
        {
        }

        public enum GoalStatus { active, inactive, completed, failed };

        //an enumerated type specifying the type of goal

        protected int m_iType;

        //a pointer to the entity that owns this goal
        protected BaseGameEntity m_pOwner;

        //an enumerated value indicating the goal's status (active, inactive,
        //completed, failed)
        protected int m_iStatus;


        /* the following methods were created to factor out some of the commonality
           in the implementations of the Process method() */

        //if m_iStatus = inactive this method sets it to active and calls Activate()
        protected void ActivateIfInactive()
        {
            if (isInactive())
            {
                Activate();
            }
        }

        //if m_iStatus is failed this method sets it to inactive so that the goal
        //will be reactivated (and therefore re-planned) on the next update-step.
        protected void ReactivateIfFailed()
        {
            if (hasFailed())
            {
                m_iStatus = (int) GoalStatus.inactive;
            }
        }




        //note how goals start off in the inactive state
        public Goal(BaseGameEntity pE, int type)
        {
            m_iType = type;
            m_pOwner = pE;
            m_iStatus = (int) GoalStatus.inactive;
        }


        //logic to run when the goal is activated.
        public abstract void Activate();

        //logic to run each update-step
        public abstract int Process();

        //logic to run when the goal is satisfied. (typically used to switch
        //off any active steering behaviors)
        public abstract void Terminate();

        //goals can handle messages. Many don't though, so this defines a default
        //behavior
        public virtual bool HandleMessage(Telegram msg) { return false; }


        //a Goal is atomic and cannot aggregate subgoals yet we must implement
        //this method to provide the uniform interface required for the goal
        //hierarchy.
        public virtual void AddSubgoal(Goal g)
        { throw new Exception("Cannot add goals to atomic goals"); }


        public bool isComplete() { return m_iStatus == (int) GoalStatus.completed; }
        public bool isActive() { return m_iStatus == (int)GoalStatus.active; }
        public bool isInactive() { return m_iStatus == (int)GoalStatus.inactive; }
        public bool hasFailed() { return m_iStatus == (int)GoalStatus.failed; }
        public int GetGoalType() { return m_iType; }



        //this is used to draw the name of the goal at the specific position
        //used for debugging
        public virtual void RenderAtPos(SpriteBatch batch, SpriteFont font, ref Vector2 pos, Convert tts)
        {

            pos.Y += 15;
            Color color = Color.Blue;

            if (isComplete()) color = new Color(0, 255, 0);
            if (isInactive()) color = new Color(0, 0, 0);
            if (hasFailed()) color = new Color(255, 0, 0);
            if (isActive()) color = new Color(0, 0, 255);

            batch.DrawString(font, tts(GetGoalType()), pos, color);
        }

        //used to render any goal specific information
        public virtual void Render(PrimitiveBatch batch) { }
    }
}
