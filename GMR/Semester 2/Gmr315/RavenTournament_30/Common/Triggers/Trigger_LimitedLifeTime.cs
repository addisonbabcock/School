using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Game;

namespace Common.Triggers
{
    public abstract class Trigger_LimitedLifeTime<T> : Trigger<T> where T : BaseGameEntity
    {


        //the lifetime of this trigger in update-steps
        protected int m_iLifetime;



        public Trigger_LimitedLifeTime(int lifetime)
            : base(BaseGameEntity.GetNextValidID())
        {
            m_iLifetime = lifetime;
        }

        //children of this class should always make sure this is called from within
        //their own update method
        public override void Update()
        {
            //if the lifetime counter expires set this trigger to be removed from
            //the game
            if (--m_iLifetime <= 0)
            {
                SetToBeRemovedFromGame();
            }
        }
    }
}
