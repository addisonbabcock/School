#region Using

using System.Collections.Generic;
using Assignment_3;
using Microsoft.Xna.Framework.Input;

#endregion

namespace BucklandXNA2
{
    public static class GameKeys
    {
        public const Keys Arrive = Keys.D1;
        public const Keys Flee = Keys.D2;
        public const Keys Flocking = Keys.D3;
        public const Keys Hide = Keys.D4;
        public const Keys Interpose = Keys.D5;
        public const Keys None = Keys.OemTilde;
        public const Keys NonPenetrationConstraint = Keys.D6;
        public const Keys ObstacleAvoidance = Keys.D7;
        public const Keys OffsetPursuit = Keys.D8;
        public const Keys PathFollowing = Keys.D9;
        public const Keys Pursuit = Keys.D0;
        public const Keys Seek = Keys.OemMinus;
        public const Keys Wander = Keys.OemPlus;

        public static Dictionary<Keys, BehaviorMode> BehaviorKeyMap = new Dictionary<Keys, BehaviorMode>();

        static GameKeys()
        {
            BehaviorKeyMap.Add(Wander, BehaviorMode.Wander);
            BehaviorKeyMap.Add(Seek, BehaviorMode.Seek);
            BehaviorKeyMap.Add(Pursuit, BehaviorMode.Pursuit);
            BehaviorKeyMap.Add(PathFollowing, BehaviorMode.PathFollowing);
            BehaviorKeyMap.Add(OffsetPursuit, BehaviorMode.OffsetPursuit);
            BehaviorKeyMap.Add(ObstacleAvoidance, BehaviorMode.ObstacleAvoidance);
            BehaviorKeyMap.Add(NonPenetrationConstraint, BehaviorMode.NonPenetrationConstraint);
            BehaviorKeyMap.Add(Interpose, BehaviorMode.Interpose);
            BehaviorKeyMap.Add(Hide, BehaviorMode.Hide);
            BehaviorKeyMap.Add(Flocking, BehaviorMode.Flocking);
            BehaviorKeyMap.Add(Flee, BehaviorMode.Flee);
            BehaviorKeyMap.Add(Arrive, BehaviorMode.Arrive);
            BehaviorKeyMap.Add(None, BehaviorMode.None);
        }

        public static bool IsBehaviorKeyPressed(Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (IsBehaviorKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsBehaviorKey(Keys key)
        {
            if ((key == Wander) || (key == Seek) || (key == Pursuit) || (key == PathFollowing)
                || (key == OffsetPursuit) || (key == ObstacleAvoidance) || (key == NonPenetrationConstraint)
                || (key == Interpose) || (key == Hide) || (key == Flocking) || (key == Flee) || (key == Arrive) ||
                (key == None))
            {
                return true;
            }
            return false;
        }

        public static BehaviorMode GetBehavior(Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (IsBehaviorKey(key))
                {
                    return BehaviorKeyMap[key];
                }
            }
            return BehaviorMode.None;
        }
    }
}