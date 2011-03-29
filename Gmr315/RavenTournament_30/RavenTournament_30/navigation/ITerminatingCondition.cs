using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Game;
using Common.Graph;
using Common.Triggers;

namespace Raven.navigation
{
    public interface ITerminatingCondition
    {
        bool isSatisfied(SparseGraph graph, int target, int CurrentNodeIdx);
    }
}
