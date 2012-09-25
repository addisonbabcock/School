using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Raven
{
    public class MemoryRecord
    {
        
  
  //records the time the opponent was last sensed (seen or heard). This
  //is used to determine if a bot can 'remember' this record or not. 
  //(if CurrentTime() - m_dTimeLastSensed is greater than the bot's
  //memory span, the data in this record is made unavailable to clients)
  public DateTime       fTimeLastSensed;

  //it can be useful to know how long an opponent has been visible. This 
  //variable is tagged with the current time whenever an opponent first becomes
  //visible. It's then a simple matter to calculate how long the opponent has
  //been in view (CurrentTime - fTimeBecameVisible)
  public DateTime fTimeBecameVisible;

  //it can also be useful to know the last time an opponent was seen
  public DateTime fTimeLastVisible;

  //a vector marking the position where the opponent was last sensed. This can
  // be used to help hunt down an opponent if it goes out of view
  public Vector2    vLastSensedPosition;

  //set to true if opponent is within the field of view of the owner
  public bool        bWithinFOV;

  //set to true if there is no obstruction between the opponent and the owner, 
  //permitting a shot.
  public bool        bShootable;
        public int iRecentDamage;

  public MemoryRecord()
  {
      fTimeLastSensed = DateTime.MinValue;
      fTimeBecameVisible = DateTime.MinValue;
      fTimeLastVisible = DateTime.MinValue;
      bWithinFOV = false;
      bShootable = false;
      iRecentDamage = 0;
  }
    }
}
