using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven
{
    public class Constants
    {
        public const int WindowWidth = 500;
        public const int WindowHeight = 500;
        public const int FrameRate = 60;

        //--------------------------- Constants ----------------------------------

        //the radius of the constraining circle for the wander behavior
        public const float WanderRad = 1.2f;
        //distance the wander circle is projected in front of the agent
        public const float WanderDist = 2.0f;
        //the maximum amount of displacement along the circle each frame
        public const float WanderJitterPerSec = 80.0f;

        //used in path following
        public const float WaypointSeekDist = 20;   
    }
}
