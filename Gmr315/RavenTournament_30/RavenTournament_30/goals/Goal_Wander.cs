using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;

namespace Raven.goals
{
    public class Goal_Wander: Goal
{
        private AbstractBot owner;


public   Goal_Wander(AbstractBot pBot):base(pBot, (int) Raven_Goal_Types.goal_wander)
{
    owner = pBot;
}

        public override void Activate()
{
    m_iStatus = (int) GoalStatus.active;

    owner.GetSteering().WanderOn();
}

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            return m_iStatus;
        }

        public override void Terminate()
        {
            owner.GetSteering().WanderOff();
        }
}
}
