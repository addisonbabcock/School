using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven
{
    public enum Raven_Objects
    {
        type_wall,
        type_bot,
        type_unused,
        type_waypoint,
        type_health,
        type_spawn_point,
        type_rail_gun,
        type_rocket_launcher,
        type_shotgun,
        type_blaster,
        type_obstacle,
        type_sliding_door,
        type_door_trigger
    };

    public class Raven_ObjectEnumerations
    {

        public static string GetNameOfType(int w)
        {
            string s;

            switch (w)
            {
                case (int)Raven_Objects.type_wall:

                    s = "Wall"; break;

                case (int)Raven_Objects.type_waypoint:

                    s = "Waypoint"; break;

                case (int)Raven_Objects.type_obstacle:

                    s = "Obstacle"; break;

                case (int)Raven_Objects.type_health:

                    s = "Health"; break;

                case (int)Raven_Objects.type_spawn_point:

                    s = "Spawn Point"; break;

                case (int)Raven_Objects.type_rail_gun:

                    s = "Railgun"; break;

                case (int)Raven_Objects.type_blaster:

                    s = "Blaster"; break;

                case (int)Raven_Objects.type_rocket_launcher:

                    s = "rocket_launcher"; break;

                case (int)Raven_Objects.type_shotgun:

                    s = "shotgun"; break;

                case (int)Raven_Objects.type_unused:

                    s = "knife"; break;

                case (int)Raven_Objects.type_bot:

                    s = "bot"; break;

                case (int)Raven_Objects.type_sliding_door:

                    s = "sliding_door"; break;

                case (int)Raven_Objects.type_door_trigger:

                    s = "door_trigger"; break;

                default:

                    s = "UNKNOWN OBJECT TYPE"; break;

            }

            return s;
        }
    }
}
