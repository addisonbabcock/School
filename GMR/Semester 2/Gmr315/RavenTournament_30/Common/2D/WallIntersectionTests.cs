using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Common._2D
{
    public class WallIntersectionTests
    {
        public static bool doWallsObstructLineSegment(Vector2 from,
                                               Vector2 to,
                                               ref IEnumerable<Wall2D> walls)
        {
            foreach (Wall2D curWall in walls)
            {
                if (Geometry.LineIntersection2D(from, to, curWall.From(), curWall.To()))
                {
                    return true;
                }
            }
            return false;
        }


        //----------------------- doWallsObstructCylinderSides -------------------------
        //
        //  similar to above except this version checks to see if the sides described
        //  by the cylinder of length |AB| with the given radius intersect any walls.
        //  (this enables the trace to take into account any the bounding radii of
        //  entity objects)
        //-----------------------------------------------------------------------------
        public static bool doWallsObstructCylinderSides(Vector2 A,
                                                 Vector2 B,
                                                 float BoundingRadius,
                                                 ref IEnumerable<Wall2D> walls)
        {
            //the line segments that make up the sides of the cylinder must be created
            Vector2 toB = Vector2.Normalize(B - A);

            //A1B1 will be one side of the cylinder, A2B2 the other.
            Vector2 A1, B1, A2, B2;

            Vector2 radialEdge = toB.Perp() * BoundingRadius;

            //create the two sides of the cylinder
            A1 = A + radialEdge;
            B1 = B + radialEdge;

            A2 = A - radialEdge;
            B2 = B - radialEdge;

            //now test against them
            if (!doWallsObstructLineSegment(A1, B1, ref walls))
            {
                return doWallsObstructLineSegment(A2, B2, ref walls);
            }

            return true;
        }

        //------------------ FindClosestPointOfIntersectionWithWalls ------------------
        //
        //  tests a line segment against the container of walls  to calculate
        //  the closest intersection point, which is stored in the reference 'ip'. The
        //  distance to the point is assigned to the reference 'distance'
        //
        //  returns false if no intersection point found
        //-----------------------------------------------------------------------------

        public static bool FindClosestPointOfIntersectionWithWalls(Vector2 A,
                                                            Vector2 B,
                                                            ref float distance,
                                                            ref Vector2 ip,
                                                            ref IEnumerable<Wall2D> walls)
        {
            distance = float.MaxValue;

            foreach (Wall2D curWall in walls)
            {
                float dist = 0.0f;
                Vector2 point = new Vector2();

                if (Geometry.LineIntersection2D(A, B, curWall.From(), curWall.To(), ref dist, ref point))
                {
                    if (dist < distance)
                    {
                        distance = dist;
                        ip = point;
                    }
                }
            }

            if (distance < float.MaxValue) return true;

            return false;
        }

        //------------------------ doWallsIntersectCircle -----------------------------
        //
        //  returns true if any walls intersect the circle of radius at point p
        //-----------------------------------------------------------------------------
        public static bool doWallsIntersectCircle(ref IEnumerable<Wall2D> walls, Vector2 p, float r)
        {
            foreach (Wall2D curWall in walls)
            {
                if (Geometry.LineSegmentCircleIntersection(curWall.From(), curWall.To(), p, r))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
