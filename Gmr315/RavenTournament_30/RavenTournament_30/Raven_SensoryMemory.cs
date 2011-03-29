using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MemoryMap = System.Collections.Generic.Dictionary<Raven.AbstractBot, Raven.MemoryRecord>;

namespace Raven
{
    public class Raven_SensoryMemory : IDisposable
    {
        public void Dispose()
        {
        }




        //the owner of this instance
        private AbstractBot m_pOwner;

        //this container is used to simulate memory of sensory events. A MemoryRecord
        //is created for each opponent in the environment. Each record is updated 
        //whenever the opponent is encountered. (when it is seen or heard)
        private MemoryMap m_MemoryMap;

        //a bot has a memory span equivalent to this value. When a bot requests a 
        //list of all recently sensed opponents this value is used to determine if 
        //the bot is able to remember an opponent or not.
        private float m_dMemorySpan;

        //this methods checks to see if there is an existing record for pBot. If
        //not a new MemoryRecord record is made and added to the memory map.(called
        //by UpdateWithSoundSource & UpdateVision)
        private void MakeNewRecordIfNotAlreadyPresent(AbstractBot pBot)
        {
            //else check to see if this Opponent already exists in the memory. If it doesn't,
            //create a new record
            if (!m_MemoryMap.ContainsKey(pBot))
            {
                m_MemoryMap.Add(pBot, new MemoryRecord());
            }

        }



        public Raven_SensoryMemory(AbstractBot owner, float MemorySpan)
        {
            m_pOwner = owner;
            m_dMemorySpan = MemorySpan;
            m_MemoryMap = new MemoryMap();

        }

        //this method is used to update the memory map whenever an opponent makes
        //a noise
        public void UpdateWithSoundSource(AbstractBot pNoiseMaker)
        {
            //make sure the bot being examined is not this bot
            if (m_pOwner != pNoiseMaker)
            {
                //if the bot is already part of the memory then update its data, else
                //create a new memory record and add it to the memory
                MakeNewRecordIfNotAlreadyPresent(pNoiseMaker);

                MemoryRecord info = m_MemoryMap[pNoiseMaker];

                //test if there is LOS between bots 
                if (m_pOwner.GetWorld().isLOSOkay(m_pOwner.Pos(), pNoiseMaker.Pos()))
                {
                    info.bShootable = true;

                    //record the position of the bot
                    info.vLastSensedPosition = pNoiseMaker.Pos();
                }
                else
                {
                    info.bShootable = false;
                }

                //record the time it was sensed
                info.fTimeLastSensed = DateTime.Now;
            }
        }

        //this removes a bot's record from memory
        public void RemoveBotFromMemory(AbstractBot pBot)
        {
            if (pBot == null) return;
            if (m_MemoryMap.ContainsKey(pBot))
            {
                m_MemoryMap.Remove(pBot);
            }
        }

        //this method iterates through all the opponents in the game world and 
        //updates the records of those that are in the owner's FOV
        public void UpdateVision()
        {
            //for each bot in the world test to see if it is visible to the owner of
            //this class
            foreach (AbstractBot curBot in m_pOwner.GetWorld().GetAllBots())
            {
                //make sure the bot being examined is not this bot
                if (m_pOwner != curBot)
                {
                    //make sure it is part of the memory map
                    MakeNewRecordIfNotAlreadyPresent(curBot);

                    //get a reference to this bot's data
                    MemoryRecord info = m_MemoryMap[curBot];

                    //test if there is LOS between bots 
                    if (m_pOwner.GetWorld().isLOSOkay(m_pOwner.Pos(), curBot.Pos()))
                    {
                        info.bShootable = true;

                        //test if the bot is within FOV
                        if (Raven_Game.isSecondInFOVOfFirst(m_pOwner.Pos(),
                                                 m_pOwner.Facing(),
                                                 curBot.Pos(),
                                                  m_pOwner.FieldOfView()))
                        {
                            info.fTimeLastSensed = DateTime.Now;
                            info.vLastSensedPosition = curBot.Pos();
                            info.fTimeLastVisible = DateTime.Now;

                            if (info.bWithinFOV == false)
                            {
                                info.bWithinFOV = true;
                                info.fTimeBecameVisible = info.fTimeLastSensed;

                            }
                        }

                        else
                        {
                            info.bWithinFOV = false;
                        }
                    }

                    else
                    {
                        info.bShootable = false;
                        info.bWithinFOV = false;
                    }
                }
            }//next bot
        }

        public bool isOpponentShootable(AbstractBot pOpponent)
        {
            if (pOpponent == null) return false;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return m_MemoryMap[pOpponent].bShootable;
            }

            return false;
        }
        public bool isOpponentWithinFOV(AbstractBot pOpponent)
        {
            if (pOpponent == null) return false;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return m_MemoryMap[pOpponent].bWithinFOV;
            }

            return false;
        }
        public Vector2 GetLastRecordedPositionOfOpponent(AbstractBot pOpponent)
        {
            if (pOpponent == null) return Vector2.Zero;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return m_MemoryMap[pOpponent].vLastSensedPosition;
            }

            throw new Exception("< Raven_SensoryMemory::GetLastRecordedPositionOfOpponent>: Attempting to get position of unrecorded bot");
        }
        public TimeSpan GetTimeOpponentHasBeenVisible(AbstractBot pOpponent)
        {
            if (pOpponent == null) return TimeSpan.MinValue;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return DateTime.Now - m_MemoryMap[pOpponent].fTimeBecameVisible;
            }

            return TimeSpan.MinValue;

        }
        public TimeSpan GetTimeSinceLastSensed(AbstractBot pOpponent)
        {
            if (pOpponent == null) return TimeSpan.MinValue;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return DateTime.Now - m_MemoryMap[pOpponent].fTimeLastSensed;
            }

            return TimeSpan.MinValue;
        }

        public TimeSpan GetTimeOpponentHasBeenOutOfView(AbstractBot pOpponent)
        {
            if (pOpponent == null) return TimeSpan.MaxValue;
            if (m_MemoryMap.ContainsKey(pOpponent))
            {
                return DateTime.Now - m_MemoryMap[pOpponent].fTimeLastVisible;
            }

            return TimeSpan.MaxValue;
        }

        //this method returns a list of all the opponents that have had their
        //records updated within the last m_dMemorySpan seconds.
        public List<AbstractBot> GetListOfRecentlySensedOpponents()
        {
            //this will store all the opponents the bot can remember
            List<AbstractBot> opponents = new List<AbstractBot>();

            DateTime CurrentTime = DateTime.Now;

            foreach (AbstractBot opponent in m_MemoryMap.Keys)
            {
                MemoryRecord curRecord = m_MemoryMap[opponent];
                if (((CurrentTime - curRecord.fTimeLastSensed).Seconds <= m_dMemorySpan))
                {
                    opponents.Add(opponent);
                }
            }

            return opponents;
        }

        public void RenderBoxesAroundRecentlySensed(PrimitiveBatch batch)
        {
            List<AbstractBot> opponents = GetListOfRecentlySensedOpponents();
            foreach (AbstractBot it in opponents)
            {
                Vector2 p = it.Pos();
                float b = it.BRadius();

                Drawing.DrawLine(batch, new Vector2(p.X - b, p.Y - b), new Vector2(p.X + b, p.Y - b), Color.Orange);
                Drawing.DrawLine(batch, new Vector2(p.X + b, p.Y - b), new Vector2(p.X + b, p.Y + b), Color.Orange);
                Drawing.DrawLine(batch, new Vector2(p.X + b, p.Y + b), new Vector2(p.X - b, p.Y + b), Color.Orange);
                Drawing.DrawLine(batch, new Vector2(p.X - b, p.Y + b), new Vector2(p.X - b, p.Y - b), Color.Orange);
            }

        }

        //----------------------- UpdateWithDamageSource -------------------------------
//
// this updates the record for an individual opponent. Note, there is no need to
// test if the opponent is within the FOV because that test will be done when the
// UpdateVision method is called
//-----------------------------------------------------------------------------
public void UpdateWithDamageSource(AbstractBot pShooter, int damage)
{
  //make sure the bot being examined is not this bot
  if (m_pOwner != pShooter) // probably not possible
  {
    //if the bot is already part of the memory then update its data, else
    //create a new memory record and add it to the memory
    MakeNewRecordIfNotAlreadyPresent(pShooter);

    MemoryRecord info = m_MemoryMap[pShooter];

	info.iRecentDamage += damage;

    //test if there is LOS between bots 
    if (m_pOwner.GetWorld().isLOSOkay(m_pOwner.Pos(), pShooter.Pos()))
    {
      info.bShootable = true;
      
     //record the position of the bot
      info.vLastSensedPosition = pShooter.Pos();
    }
    else
    {
      info.bShootable = false;
    }
    
    //record the time it was sensed
      info.fTimeLastSensed = DateTime.Now;
  }
}
        public int GetDamage(AbstractBot pOpponent)
        {
            if (!m_MemoryMap.ContainsKey(pOpponent))
            {
                return 0;
            }
            return m_MemoryMap[pOpponent].iRecentDamage;
        }

        public bool IsUnderAttack()
        {
            foreach (MemoryRecord record in m_MemoryMap.Values)
            {
                if (record.iRecentDamage > 0)
                {
                    return true;
                }

            }
            return false;

        }
    }
}
