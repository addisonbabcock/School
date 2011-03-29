using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;

namespace Common.Triggers
{
    public class TriggerSystem<T> where T : BaseGameEntity, ITriggerSystem
    {
        public TriggerSystem()
        {
            m_Triggers = new List<Trigger<T>>();
        }
        
private   List<Trigger<T>>  m_Triggers; 


  //this method iterates through all the triggers present in the system and
  //calls their Update method in order that their protected state can be
  //updated if necessary. It also removes any triggers from the system that
  //have their m_bRemoveFromGame field set to true.
  private void UpdateTriggers()
  {
      int i = 0;
      while(i < m_Triggers.Count())
      {
          Trigger<T> curTrg = m_Triggers[i];
          //remove trigger if dead
          if (curTrg.isToBeRemoved())
          {
              curTrg.Dispose();

              m_Triggers.RemoveAt(i);
          }
          else
          {
              //update this trigger
              curTrg.Update();

              ++i;
          }
      }
  }


  private void TryTriggers(IEnumerable<T> entities)
  {
      foreach (T curEnt in entities)
      {
          if (curEnt.isReadyForTriggerUpdate() && curEnt.isAlive())
          {
              foreach (Trigger<T> curTrg in m_Triggers)
              {
                  curTrg.Try(curEnt);
              }
          }
      }


  }



        ~TriggerSystem()
  {
    Clear();
  }

  //this deletes any current triggers and empties the trigger list
public   void Clear()
  {
    foreach(Trigger<T> curTrg in m_Triggers)
    {
        curTrg.Dispose();
    }
    
    m_Triggers.Clear();
  }

  public void Update(IEnumerable<T> entities)
  {
    UpdateTriggers();
    TryTriggers(entities);
  }

  //this is used to register triggers with the TriggerSystem (the TriggerSystem
  //will take care of tidying up memory used by a trigger)
  public void Register(Trigger<T> trigger)
  {
    m_Triggers.Add(trigger);
  }

  //some triggers are required to be rendered (like giver-triggers for example)
  public void Render(PrimitiveBatch batch)
  {
      foreach(Trigger<T> curTrg in m_Triggers)
      {
          curTrg.Render(batch);
      }
  }

  public List<Trigger<T>> GetTriggers(){return m_Triggers;}


    }
}
