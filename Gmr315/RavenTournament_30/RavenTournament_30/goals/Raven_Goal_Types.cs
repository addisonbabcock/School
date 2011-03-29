using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raven.goals
{
    public enum Raven_Goal_Types
    {
        goal_think,
        goal_explore,
        goal_arrive_at_position,
        goal_seek_to_position,
        goal_follow_path,
        goal_traverse_edge,
        goal_move_to_position,
        goal_get_health,
        goal_get_shotgun,
        goal_get_rocket_launcher,
        goal_get_railgun,
        goal_wander,
        goal_negotiate_door,
        goal_attack_target,
        goal_hunt_target,
        goal_strafe,
        goal_adjust_range,
        goal_say_phrase

    }

    public class GoalTypeToString
    {


        public static string Convert(int gt)
        {
            switch (gt)
            {
                case (int)Raven_Goal_Types.goal_explore:

                    return "explore";

                case (int)Raven_Goal_Types.goal_think:

                    return "think";

                case (int)Raven_Goal_Types.goal_arrive_at_position:

                    return "arrive_at_position";

                case (int)Raven_Goal_Types.goal_seek_to_position:

                    return "seek_to_position";

                case (int)Raven_Goal_Types.goal_follow_path:

                    return "follow_path";

                case (int)Raven_Goal_Types.goal_traverse_edge:

                    return "traverse_edge";

                case (int)Raven_Goal_Types.goal_move_to_position:

                    return "move_to_position";

                case (int)Raven_Goal_Types.goal_get_health:

                    return "get_health";

                case (int)Raven_Goal_Types.goal_get_shotgun:

                    return "get_shotgun";

                case (int)Raven_Goal_Types.goal_get_railgun:

                    return "get_railgun";

                case (int)Raven_Goal_Types.goal_get_rocket_launcher:

                    return "get_rocket_launcher";

                case (int)Raven_Goal_Types.goal_wander:

                    return "wander";

                case (int)Raven_Goal_Types.goal_negotiate_door:

                    return "negotiate_door";

                case (int)Raven_Goal_Types.goal_attack_target:

                    return "attack_target";

                case (int)Raven_Goal_Types.goal_hunt_target:

                    return "hunt_target";

                case (int)Raven_Goal_Types.goal_strafe:

                    return "strafe";

                case (int)Raven_Goal_Types.goal_adjust_range:

                    return "adjust_range";

                case (int)Raven_Goal_Types.goal_say_phrase:

                    return "say_phrase";

                default:

                    return "UNKNOWN GOAL TYPE!";

            } //end switch
        }
    }
}

