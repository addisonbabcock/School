using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Goals;

namespace Raven.goals
{
    public class Goal_AdjustRange : Goal
{

        private AbstractBot owner;
private   AbstractBot  m_pTarget;

private   double       m_dIdealRange;



public   Goal_AdjustRange(AbstractBot pBot):base(pBot, (int) Raven_Goal_Types.goal_adjust_range)
{
    owner = pBot;
    m_dIdealRange = 0;
}

        public override void Activate()
        {
            owner.GetSteering().SetTarget(owner.GetTargetBot().Pos());

        }

        public override int Process()
        {
            //if status is inactive, call Activate()
            ActivateIfInactive();

            /*
            if (m_pOwner.GetCurrentWeapon().isInIdealWeaponRange())
            {
              m_iStatus = completed;
            }
          */
            return (int) m_iStatus;
        }

        public override void Terminate()
        {
            
        }
}
    
}
