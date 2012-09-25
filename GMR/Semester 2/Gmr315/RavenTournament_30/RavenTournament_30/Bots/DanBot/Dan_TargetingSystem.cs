using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Raven.Bots.DanBot
{
    public class Dan_TargetingSystem : AbstractTargetingSystem
    {
        public Dan_TargetingSystem(AbstractBot owner)
            : base(owner)
        {
        }


        //each time this method is called the opponents in the owner's sensory 
        //memory are examined and the closest  is assigned to m_pCurrentTarget.
        //if there are no opponents that have had their memory records updated
        //within the memory span of the owner then the current target is set
        //to null
        public override void Update()
        {
            float ClosestDistSoFar = float.MaxValue;
            m_pCurrentTarget = null;

            //grab a list of all the opponents the owner can sense
            List<AbstractBot> SensedBots = m_pOwner.GetSensoryMem().GetListOfRecentlySensedOpponents();

            foreach (AbstractBot curBot in SensedBots)
            {
                //make sure the bot is alive and that it is not the owner
                if (curBot.isAlive() && (curBot != m_pOwner))
                {
                    float dist = Vector2.DistanceSquared(curBot.Pos(), m_pOwner.Pos());

                    if (dist < ClosestDistSoFar)
                    {
                        ClosestDistSoFar = dist;
                        m_pCurrentTarget = curBot;
                    }
                }
            }
        }
    }
}