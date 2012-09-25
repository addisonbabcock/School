using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Graph
{
    public interface IHeuristic
    {
        float Calculate(ref SparseGraph G, int nd1, int nd2);
    }
}
