using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
    public interface IGraphNode
    {
        Vector2 Pos();
        int Index();
    }
}
