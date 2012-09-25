#region Using

using System;
using System.Collections.Generic;
using Common.Misc;
using Microsoft.Xna.Framework;

#endregion

namespace Common._2D
{
    public class Geometry
    {
        //given a plane and a ray this function determins how far along the ray 
        //an interestion occurs. Returns negative if the ray is parallel

        //------------------------- WhereIsPoint --------------------------------------

        #region span_type enum

        public enum span_type
        {
            plane_backside,
            plane_front,
            on_plane
        } ;

        #endregion

        public static float DistanceToRayPlaneIntersection(Vector2 RayOrigin,
                                                           Vector2 RayHeading,
                                                           Vector2 PlanePoint, //any point on the plane
                                                           Vector2 PlaneNormal)
        {
//            Utils.TestVector(RayOrigin, RayHeading, PlanePoint, PlaneNormal);
            float d = Vector2.Dot(-PlaneNormal, PlanePoint);
            float numer = Vector2.Dot(PlaneNormal, RayOrigin) + d;
            float denom = Vector2.Dot(PlaneNormal, RayHeading);
            
            // normal is parallel to vector
            if ((denom < 0.000001) && (denom > -0.000001))
            {
                return (-1.0f);
            }
            
//            Utils.TestFloat(denom, true);
            float result = -(numer/denom);
            return -(numer/denom);
        }

        public static span_type WhereIsPoint(Vector2 point,
                                             Vector2 PointOnPlane, //any point on the plane
                                             Vector2 PlaneNormal)
        {
//            Utils.TestVector(point, PointOnPlane, PlaneNormal);
            Vector2 dir = PointOnPlane - point;
            float d = Vector2.Dot(dir, PlaneNormal);
            if (d < -0.000001)
            {
                return span_type.plane_front;
            }

            if (d > 0.000001)
            {
                return span_type.plane_backside;
            }

            return span_type.on_plane;
        }

        //-------------------------- GetRayCircleIntersec -----------------------------
        public static float GetRayCircleIntersect(Vector2 RayOrigin,
                                                  Vector2 RayHeading,
                                                  Vector2 CircleOrigin,
                                                  float radius)
        {
//            Utils.TestVector(RayOrigin, RayHeading, CircleOrigin);
//            Utils.TestFloat(radius);
            Vector2 ToCircle = CircleOrigin - RayOrigin;
            float length = ToCircle.Length();
            float v = Vector2.Dot(ToCircle, RayHeading);
            float d = radius*radius - (length*length - v*v);
            
            // If there was no intersection, return -1
            if (d < 0.0) return (-1.0f);

            // Return the distance to the [first] intersecting point
            return (v - (float) Math.Sqrt(d));
        }

        //----------------------------- DoRayCircleIntersect --------------------------
        public static bool DoRayCircleIntersect(Vector2 RayOrigin,
                                                Vector2 RayHeading,
                                                Vector2 CircleOrigin,
                                                float radius)
        {
//            Utils.TestVector(RayOrigin, RayHeading, CircleOrigin);
//            Utils.TestFloat(radius);
            Vector2 ToCircle = CircleOrigin - RayOrigin;
            float length = ToCircle.Length();
            float v = Vector2.Dot(ToCircle, RayHeading);
            float d = radius*radius - (length*length - v*v);
            // If there was no intersection, return -1
            return (d < 0.0);
        }


        //------------------------------------------------------------------------
        //  Given a point P and a circle of radius R centered at C this function
        //  determines the two points on the circle that intersect with the 
        //  tangents from P to the circle. Returns false if P is within the circle.
        //
        //  thanks to Dave Eberly for this one.
        //------------------------------------------------------------------------
        public static bool GetTangentPoints(Vector2 C, float R, Vector2 P, ref Vector2 T1, ref Vector2 T2)
        {
//            Utils.TestVector(C, P, T1, T2);
//            Utils.TestFloat(R);
            Vector2 PmC = P - C;
            float SqrLen = PmC.LengthSquared();
            float RSqr = R*R;
            if (SqrLen <= RSqr)
            {
                // P is inside or on the circle
                return false;
            }

            float InvSqrLen = 1/SqrLen;
            float Root = (float) Math.Sqrt(Math.Abs(SqrLen - RSqr));
            float newT1x = C.X + R*(R*PmC.X - PmC.Y*Root)*InvSqrLen;
            float newT1y = C.Y + R*(R*PmC.Y + PmC.X*Root)*InvSqrLen;
            float newT2x = C.X + R*(R*PmC.X + PmC.Y*Root)*InvSqrLen;
            float newT2y = C.Y + R*(R*PmC.Y - PmC.X*Root)*InvSqrLen;
            T1 = new Vector2(newT1x, newT1y);
            T2 = new Vector2(newT2x, newT2y);
            return true;
        }


        //------------------------- DistToLineSegment ----------------------------
        //
        //  given a line segment AB and a point P, this function calculates the 
        //  perpendicular distance between them
        //------------------------------------------------------------------------
        public static float DistToLineSegment(Vector2 A,
                                              Vector2 B,
                                              Vector2 P)
        {
//            Utils.TestVector(A, B, P);
            //if the angle is obtuse between PA and AB is obtuse then the closest
            //vertex must be A
            float dotA = (P.X - A.X)*(B.X - A.X) + (P.Y - A.Y)*(B.Y - A.Y);
            if (dotA <= 0) return Vector2.Distance(A, P);

            //if the angle is obtuse between PB and AB is obtuse then the closest
            //vertex must be B
            float dotB = (P.X - B.X)*(A.X - B.X) + (P.Y - B.Y)*(A.Y - B.Y);
            if (dotB <= 0) return Vector2.Distance(B, P);

            //calculate the point along AB that is the closest to P
            Vector2 Point = A + ((B - A)*dotA)/(dotA + dotB);
            //calculate the distance P-Point
            float result = Vector2.Distance(P, Point);
            return result;
        }

        //------------------------- DistToLineSegmentSq ----------------------------
        //
        //  as above, but avoiding sqrt
        //------------------------------------------------------------------------
        public static float DistToLineSegmentSq(Vector2 A,
                                                Vector2 B,
                                                Vector2 P)
        {
//            Utils.TestVector(A, B, P);
            //if the angle is obtuse between PA and AB is obtuse then the closest
            //vertex must be A
            float dotA = (P.X - A.X)*(B.X - A.X) + (P.Y - A.Y)*(B.Y - A.Y);

            if (dotA <= 0) return Vector2.DistanceSquared(A, P);

            //if the angle is obtuse between PB and AB is obtuse then the closest
            //vertex must be B
            float dotB = (P.X - B.X)*(A.X - B.X) + (P.Y - B.Y)*(A.Y - B.Y);

            if (dotB <= 0) return Vector2.DistanceSquared(B, P);

//            Utils.TestFloat(dotA+dotB, true);
            //calculate the point along AB that is the closest to P
            Vector2 Point = A + ((B - A)*dotA)/(dotA + dotB);
            //calculate the distance P-Point
            return Vector2.DistanceSquared(P, Point);
        }


        //--------------------LineIntersection2D-------------------------
        //
        //	Given 2 lines in 2D space AB, CD this returns true if an 
        //	intersection occurs.
        //
        //----------------------------------------------------------------- 

        public static bool LineIntersection2D(Vector2 A,
                                              Vector2 B,
                                              Vector2 C,
                                              Vector2 D)
        {
          //  Utils.TestVector(A, B, C, D);
            float rTop = (A.Y - C.Y)*(D.X - C.X) - (A.X - C.X)*(D.Y - C.Y);
            float sTop = (A.Y - C.Y)*(B.X - A.X) - (A.X - C.X)*(B.Y - A.Y);

            float Bot = (B.X - A.X)*(D.Y - C.Y) - (B.Y - A.Y)*(D.X - C.X);

            if (Bot == 0) //parallel
            {
                return false;
            }

         //   Utils.TestFloat(Bot, true);
            float invBot = 1.0f/Bot;
            float r = rTop*invBot;
            float s = sTop*invBot;

            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                //lines intersect
                return true;
            }

            //lines do not intersect
            return false;
        }

        //--------------------LineIntersection2D-------------------------
        //
        //	Given 2 lines in 2D space AB, CD this returns true if an 
        //	intersection occurs and sets dist to the distance the intersection
        //  occurs along AB
        //
        //----------------------------------------------------------------- 

        public static bool LineIntersection2D(Vector2 A,
                                              Vector2 B,
                                              Vector2 C,
                                              Vector2 D,
                                              ref float dist)
        {
           // Utils.TestVector(A, B, C, D);
           // Utils.TestFloat(dist);
            float rTop = (A.Y - C.Y)*(D.X - C.X) - (A.X - C.X)*(D.Y - C.Y);
            float sTop = (A.Y - C.Y)*(B.X - A.X) - (A.X - C.X)*(B.Y - A.Y);

            float Bot = (B.X - A.X)*(D.Y - C.Y) - (B.Y - A.Y)*(D.X - C.X);


            if (Bot == 0) //parallel
            {
                if (Utils.isEqual(rTop, 0) && Utils.isEqual(sTop, 0))
                {
                    return true;
                }
                return false;
            }

//            Utils.TestFloat(Bot, true);
            float r = rTop/Bot;
            float s = sTop/Bot;

            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2.DistanceSquared(A, B)*r;

                return true;
            }

            dist = 0;

            return false;
        }

        //-------------------- LineIntersection2D-------------------------
        //
        //	Given 2 lines in 2D space AB, CD this returns true if an 
        //	intersection occurs and sets dist to the distance the intersection
        //  occurs along AB. Also sets the 2d vector point to the point of
        //  intersection
        //----------------------------------------------------------------- 
        public static bool LineIntersection2D(Vector2 A,
                                              Vector2 B,
                                              Vector2 C,
                                              Vector2 D,
                                              ref float dist,
                                              ref Vector2 point)
        {
            //Utils.TestVector(A, B, C, D);
            //Utils.TestVector(point);
            //Utils.TestFloat(dist);
            float calc1 = A.Y - C.Y;
            float calc2 = D.X - C.X;
            float calc3 = A.X - C.X;
            float calc4 = D.Y - C.Y;
            float calc5 = B.X - A.X;
            float calc6 = B.Y - A.Y;
            float calc7 = calc6*calc2;

            float rTop = calc1*calc2 - calc3*calc4;
            float rBot = calc5*calc4 - calc7;

            float sTop = calc1*calc5 - calc3*calc6;
            float sBot = rBot;

            if ((rBot == 0) || (sBot == 0))
            {
                //lines are parallel
                return false;
            }

            //Utils.TestFloat(rBot, true);
            //Utils.TestFloat(sBot, true);
            float r = rTop/rBot;
            float s = sTop/sBot;
            if ((r > 0) && (r < 1) && (s > 0) && (s < 1))
            {
                dist = Vector2.Distance(A, B)*r;

                point = A + r*(B - A);

                return true;
            }

            dist = 0;

            return false;
        }

        //----------------------- ObjectIntersection2D ---------------------------
        //
        //  tests two polygons for intersection. *Does not check for enclosure*
        //------------------------------------------------------------------------
        public static bool ObjectIntersection2D(ref List<Vector2> object1,
                                                ref List<Vector2> object2)
        {
            //Utils.TestVector(object1);
            //Utils.TestVector(object2);
            //test each line segment of object1 against each segment of object2
            for (int r = 0; r < object1.Count - 1; ++r)
            {
                for (int t = 0; t < object2.Count - 1; ++t)
                {
                    if (LineIntersection2D(object2[t],
                                           object2[t + 1],
                                           object1[r],
                                           object1[r + 1]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //----------------------- SegmentObjectIntersection2D --------------------
        //
        //  tests a line segment against a polygon for intersection
        //  *Does not check for enclosure*
        //------------------------------------------------------------------------
        public static bool SegmentObjectIntersection2D(ref Vector2 A,
                                                       ref Vector2 B,
                                                       ref List<Vector2> obj)
        {
//            Utils.TestVector(A, B);
//            Utils.TestVector(obj);
            //test AB against each segment of object
            for (int r = 0; r < obj.Count - 1; ++r)
            {
                if (LineIntersection2D(A, B, obj[r], obj[r + 1]))
                {
                    return true;
                }
            }

            return false;
        }


        //----------------------------- TwoCirclesOverlapped ---------------------
        //
        //  Returns true if the two circles overlap
        //------------------------------------------------------------------------
        public static bool TwoCirclesOverlapped(float x1, float y1, float r1,
                                                float x2, float y2, float r2)
        {
//            Utils.TestFloat(x1, y1, r1, x2, y2, r2);
            float DistBetweenCenters = (float) Math.Sqrt((x1 - x2)*(x1 - x2) +
                                                         (y1 - y2)*(y1 - y2));

            if ((DistBetweenCenters < (r1 + r2)) || (DistBetweenCenters < Math.Abs(r1 - r2)))
            {
                return true;
            }

            return false;
        }

        //----------------------------- TwoCirclesOverlapped ---------------------
        //
        //  Returns true if the two circles overlap
        //------------------------------------------------------------------------
        public static bool TwoCirclesOverlapped(Vector2 c1, float r1,
                                                Vector2 c2, float r2)
        {
            //Utils.TestVector(c1, c2);
            //Utils.TestFloat(r1, r2);
            float DistBetweenCenters = (float) Math.Sqrt((c1.X - c2.X)*(c1.X - c2.X) +
                                                         (c1.Y - c2.Y)*(c1.Y - c2.Y));

            if ((DistBetweenCenters < (r1 + r2)) || (DistBetweenCenters < Math.Abs(r1 - r2)))
            {
                return true;
            }

            return false;
        }

        //--------------------------- TwoCirclesEnclosed ---------------------------
        //
        //  returns true if one circle encloses the other
        //-------------------------------------------------------------------------
        public static bool TwoCirclesEnclosed(float x1, float y1, float r1,
                                              float x2, float y2, float r2)
        {
//            Utils.TestFloat(x1, y1, r1, x2, y2, r2);
            float DistBetweenCenters = (float) Math.Sqrt((x1 - x2)*(x1 - x2) +
                                                         (y1 - y2)*(y1 - y2));

            if (DistBetweenCenters < Math.Abs(r1 - r2))
            {
                return true;
            }

            return false;
        }

        //------------------------ TwoCirclesIntersectionPoints ------------------
        //
        //  Given two circles this function calculates the intersection points
        //  of any overlap.
        //
        //  returns false if no overlap found
        //
        // see http://astronomy.swin.edu.au/~pbourke/geometry/2circle/
        //------------------------------------------------------------------------ 
        public static bool TwoCirclesIntersectionPoints(float x1, float y1, float r1,
                                                        float x2, float y2, float r2,
                                                        ref float p3X, ref float p3Y,
                                                        ref float p4X, ref float p4Y)
        {
//            Utils.TestFloat(x1, y1, r1, x2, y2, r2);
  //          Utils.TestFloat(p3X, p3Y, p4X, p4Y);

            //first check to see if they overlap
            if (!TwoCirclesOverlapped(x1, y1, r1, x2, y2, r2))
            {
                return false;
            }

            //calculate the distance between the circle centers
            float d = (float) Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));

    //        Utils.TestFloat(d, true);
            //Now calculate the distance from the center of each circle to the center
            //of the line which connects the intersection points.
            float a = (r1 - r2 + (d*d))/(2*d);


            //MAYBE A TEST FOR EXACT OVERLAP? 

            //calculate the point P2 which is the center of the line which 
            //connects the intersection points
            float p2X, p2Y;

            p2X = x1 + a*(x2 - x1)/d;
            p2Y = y1 + a*(y2 - y1)/d;

            //calculate first point
            float h1 = (float) Math.Sqrt((r1*r1) - (a*a));

            p3X = p2X - h1*(y2 - y1)/d;
            p3Y = p2Y + h1*(x2 - x1)/d;


            //calculate second point
            float h2 = (float) Math.Sqrt((r2*r2) - (a*a));

            p4X = p2X + h2*(y2 - y1)/d;
            p4Y = p2Y - h2*(x2 - x1)/d;

            return true;
        }

        //------------------------ TwoCirclesIntersectionArea --------------------
        //
        //  Tests to see if two circles overlap and if so calculates the area
        //  defined by the union
        //
        // see http://mathforum.org/library/drmath/view/54785.html
        //-----------------------------------------------------------------------
        public static float TwoCirclesIntersectionArea(float x1, float y1, float r1,
                                                       float x2, float y2, float r2)
        {
//            Utils.TestFloat(x1,y1,x2,y2,r1,r2);
            float iX1=0;
            float iY1 = 0;
            float iX2 = 0;
            float iY2 = 0;

            //first calculate the intersection points
            if (!TwoCirclesIntersectionPoints(x1, y1, r1, x2, y2, r2, ref iX1, ref iY1, ref iX2, ref iY2))
            {
                return 0.0f; //no overlap
            }

            //calculate the distance between the circle centers
            float d = (float) Math.Sqrt((x1 - x2)*(x1 - x2) + (y1 - y2)*(y1 - y2));

//            Utils.TestFloat(r2, true);
  //          Utils.TestFloat(r1, true);
    //        Utils.TestFloat(d, true);
            //find the angles given that A and B are the two circle centers
            //and C and D are the intersection points
            float CBD = 2*(float) Math.Acos((r2*r2 + d*d - r1*r1)/(r2*d*2));

            float CAD = 2*(float) Math.Acos((r1*r1 + d*d - r2*r2)/(r1*d*2));


            //Then we find the segment of each of the circles cut off by the 
            //chord CD, by taking the area of the sector of the circle BCD and
            //subtracting the area of triangle BCD. Similarly we find the area
            //of the sector ACD and subtract the area of triangle ACD.

            float area = 0.5f*CBD*r2*r2 - 0.5f*r2*r2*(float) Math.Sin(CBD) +
                         0.5f*CAD*r1*r1 - 0.5f*r1*r1*(float) Math.Sin(CAD);

            return area;
        }

        //-------------------------------- CircleArea ---------------------------
        //
        //  given the radius, calculates the area of a circle
        //-----------------------------------------------------------------------
        public static float CircleArea(float radius)
        {
//            Utils.TestFloat(radius);
            return MathHelper.Pi*radius*radius;
        }


        //----------------------- PointInCircle ----------------------------------
        //
        //  returns true if the point p is within the radius of the given circle
        //------------------------------------------------------------------------
        public static bool PointInCircle(Vector2 Pos,
                                         float radius,
                                         Vector2 p)
        {
//            Utils.TestVector(Pos, p);
//            Utils.TestFloat(radius);
            float DistFromCenterSquared = (p - Pos).LengthSquared();

            if (DistFromCenterSquared < (radius*radius))
            {
                return true;
            }

            return false;
        }

        //--------------------- LineSegmentCircleIntersection ---------------------------
        //
        //  returns true if the line segemnt AB intersects with a circle at
        //  position P with radius radius
        //------------------------------------------------------------------------
        public static bool LineSegmentCircleIntersection(Vector2 A,
                                                         Vector2 B,
                                                         Vector2 P,
                                                         float radius)
        {
//            Utils.TestVector(A, B, P);
//            Utils.TestFloat(radius);
            //first determine the distance from the center of the circle to
            //the line segment (working in distance squared space)
            float DistToLineSq = DistToLineSegmentSq(A, B, P);

            if (DistToLineSq < radius*radius)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //------------------- GetLineSegmentCircleClosestIntersectionPoint ------------
        //
        //  given a line segment AB and a circle position and radius, this function
        //  determines if there is an intersection and stores the position of the 
        //  closest intersection in the reference IntersectionPoint
        //
        //  returns false if no intersection point is found
        //-----------------------------------------------------------------------------
        public static bool GetLineSegmentCircleClosestIntersectionPoint(Vector2 A,
                                                                        Vector2 B,
                                                                        Vector2 pos,
                                                                        float radius,
                                                                        ref Vector2 IntersectionPoint)
        {
//            Utils.TestVector(A, B, pos, IntersectionPoint);
//            Utils.TestFloat(radius);
//            Utils.TestVector(B-A, true);
            IntersectionPoint = new Vector2();
            Vector2 toBNorm = Vector2.Normalize(B - A);
            Vector2 toBNormPerp = toBNorm.Perp();
            //move the circle into the local space defined by the vector B-A with origin
            //at A
            Vector2 LocalPos = Transformations.PointToLocalSpace(ref pos, ref toBNorm, ref toBNormPerp, ref A);

            bool ipFound = false;

            //if the local position + the radius is negative then the circle lays behind
            //point A so there is no intersection possible. If the local x pos minus the 
            //radius is greater than length A-B then the circle cannot intersect the 
            //line segment
            if ((LocalPos.X + radius >= 0) &&
                ((LocalPos.X - radius)*(LocalPos.X - radius) <= Vector2.DistanceSquared(B, A)))
            {
                //if the distance from the x axis to the object's position is less
                //than its radius then there is a potential intersection.
                if (Math.Abs(LocalPos.Y) < radius)
                {
                    //now to do a line/circle intersection test. The center of the 
                    //circle is represented by A, B. The intersection points are 
                    //given by the formulae x = A +/-sqrt(r^2-B^2), y=0. We only 
                    //need to look at the smallest positive value of x.
                    float a = LocalPos.X;
                    float b = LocalPos.Y;

                    float ip = a - (float) Math.Sqrt(radius*radius - b*b);

                    if (ip <= 0)
                    {
                        ip = a + (float) Math.Sqrt(radius*radius - b*b);
                    }

                    ipFound = true;

                    IntersectionPoint = A + toBNorm*ip;
                }
            }

            return ipFound;
        }
    }
}