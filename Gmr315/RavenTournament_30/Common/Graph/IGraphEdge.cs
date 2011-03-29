using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
    public interface IGraphEdge
    {
        int Flags();
        int IDofIntersectingEntity();
        int From();
        int To();
        float Cost();
    }
}
