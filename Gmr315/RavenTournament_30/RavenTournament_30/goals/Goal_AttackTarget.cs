using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;
using Microsoft.Xna.Framework;

namespace Raven.goals
{
    public class Goal_AttackTarget : Goal_Composite
    {
        private AbstractBot owner;

        public Goal_AttackTarget(AbstractBot pOwner)
            : base(pOwner, (int) Raven_Goal_Types.goal_attack_target)
  {
            owner = pOwner;}

        
        public override void Activate()
        {
            m_iStatus = (int) GoalStatus.active;

            //if this goal is reactivated then there may be some existing subgoals that
            //must be removed
            RemoveAllSubgoals();

            //it is possible for a bot's target to die whilst this goal is active so we
            //must test to make sure the bot always has an active target
            if (!owner.GetTargetSys().isTargetPresent())
            {
                m_iStatus = (int)GoalStatus.completed;

                return;
            }

            //if the bot is able to shoot the target (there is LOS between bot and
            //target), then select a tactic to follow while shooting
            if (owner.GetTargetSys().isTargetShootable())
            {
                //if the bot has space to strafe then do so
                Vector2 dummy = new Vector2();
                if (owner.canStepLeft(ref dummy) || owner.canStepRight(ref dummy))
                {
                    AddSubgoal(new Goal_DodgeSideToSide(owner));
                }

                //if not able to strafe, head directly at the target's position 
                else
                {
                    AddSubgoal(new Goal_SeekToPosition(owner, owner.GetTargetBot().Pos()));
                }
            }

            //if the target is not visible, go hunt it.
            else
            {
                AddSubgoal(new Goal_HuntTarget(owner));
            }
        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            //process the subgoals
            m_iStatus = ProcessSubgoals();

            ReactivateIfFailed();

            return m_iStatus;
        }

        public override void Terminate()
        {
            m_iStatus = (int) GoalStatus.completed;
        }
    }
}
