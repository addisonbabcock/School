using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Triggers
{
    public interface ITriggerSystem
    {
        bool          isReadyForTriggerUpdate();
        bool isAlive();
    }
}
