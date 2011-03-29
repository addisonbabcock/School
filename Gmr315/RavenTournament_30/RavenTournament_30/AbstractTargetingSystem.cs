using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Raven
{
    public abstract class AbstractTargetingSystem : IDisposable
    {
        
        public void Dispose()
        {
        }

        //the owner of this system
  protected AbstractBot  m_pOwner;

  //the current target (this will be null if there is no target assigned)
  protected AbstractBot  m_pCurrentTarget;




public   AbstractTargetingSystem(AbstractBot owner){
    m_pOwner = owner;
    m_pCurrentTarget = null;
}


        public abstract void Update();

  //returns true if there is a currently assigned target
  public bool       isTargetPresent(){return m_pCurrentTarget != null;}

  //returns true if the target is within the field of view of the owner
  public virtual bool       isTargetWithinFOV(){
  return m_pOwner.GetSensoryMem().isOpponentWithinFOV(m_pCurrentTarget);
}


  //returns true if there is unobstructed line of sight between the target
  //and the owner
  public virtual bool isTargetShootable()
  {
      if (m_pCurrentTarget == null) return false;
  return m_pOwner.GetSensoryMem().isOpponentShootable(m_pCurrentTarget);
}

  //returns the position the target was last seen. Throws an exception if
  //there is no target currently assigned
  public virtual Vector2 GetLastRecordedPosition()
  {
  return m_pOwner.GetSensoryMem().GetLastRecordedPositionOfOpponent(m_pCurrentTarget);
}

  //returns the amount of time the target has been in the field of view
  public virtual TimeSpan GetTimeTargetHasBeenVisible()
  {
  return m_pOwner.GetSensoryMem().GetTimeOpponentHasBeenVisible(m_pCurrentTarget);
}

  //returns the amount of time the target has been out of view
  public virtual TimeSpan GetTimeTargetHasBeenOutOfView()
  {
  return m_pOwner.GetSensoryMem().GetTimeOpponentHasBeenOutOfView(m_pCurrentTarget);
}
  
  //returns a pointer to the target. null if no target current.
  public virtual AbstractBot GetTarget() { return m_pCurrentTarget; }

  //sets the target pointer to null
  public void ClearTarget() { m_pCurrentTarget = null; }
    }
}
