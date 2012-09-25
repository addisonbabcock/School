using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Game;
using Microsoft.Xna.Framework;

namespace Common.Graph
{
    public class Heuristic_Euclid
    {

        public static float Calculate(SparseGraph G, int nd1, int nd2)
        {
            return Vector2.Distance(G.GetNode(nd1).Pos(), G.GetNode(nd2).Pos());
        }
    }
}
