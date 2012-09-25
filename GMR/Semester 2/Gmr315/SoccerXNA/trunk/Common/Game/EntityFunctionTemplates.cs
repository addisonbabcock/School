#region Using

using System.Collections.Generic;
using Common._2D;
using Microsoft.Xna.Framework;

#endregion

namespace Common.Game
{
    public class EntityFunctionTemplates<T> where T : IEntity
    {
        public static bool Overlapped(T ob, List<T> conOb)
        {
            return Overlapped(ob, conOb, 40.0f);
        }

        public static bool Overlapped(T ob, List<T> conOb, float MinDistBetweenObstacles)
        {
            foreach (T it in conOb)
            {
                if (Geometry.TwoCirclesOverlapped(ob.Pos(),
                                                  ob.BRadius() + MinDistBetweenObstacles,
                                                  (it).Pos(),
                                                  (it).BRadius()))
                {
                    return true;
                }
            }

            return false;
        }

        public static void TagNeighbors(T entity, List<T> others, float radius)
        {
            foreach (T it in others)
            {
                //first clear any current tag
                (it).UnTag();

                //work in distance squared to avoid sqrts
                Vector2 to = (it).Pos() - entity.Pos();

                //the bounding radius of the other is taken into account by adding it 
                //to the range
                float range = radius + (it).BRadius();

                //if entity within range, tag for further consideration
                if ((!it.Equals(entity)) && (to.LengthSquared() < range*range))
                {
                    (it).Tag();
                }
            } //next entity
        }

        public static void EnforceNonPenetrationContraint(T entity, List<T> others)
        {
            foreach (T it in others)
            {
                //make sure we don't check against this entity
                if (it.Equals(entity)) continue;

                //calculate the distance between the positions of the entities
                Vector2 ToEntity = entity.Pos() - (it).Pos();

                float DistFromEachOther = ToEntity.Length();

                //if this distance is smaller than the sum of their radii then this
                //entity must be moved away in the direction parallel to the
                //ToEntity vector   
                float AmountOfOverLap = (it).BRadius() + entity.BRadius() -
                                        DistFromEachOther;

                if (AmountOfOverLap >= 0)
                {
                    //move the entity a distance away equivalent to the amount of overlap.
                    entity.SetPos(entity.Pos() + (ToEntity/DistFromEachOther)*
                                                 AmountOfOverLap);
                }
            } //next entity
        }
    }
}