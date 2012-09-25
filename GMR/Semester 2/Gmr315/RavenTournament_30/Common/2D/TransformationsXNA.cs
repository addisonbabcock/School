#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.Misc;
using Microsoft.Xna.Framework;

#endregion

namespace Common._2D
{
    public class TransformationsXNA
    {
        private static bool debug = true;
        //--------------------------- WorldTransform -----------------------------
        //
        //  given a List of 2D vectors, a position, orientation and scale,
        //  this function transforms the 2D vectors into the object's world space
        //------------------------------------------------------------------------
        public static List<Vector2> WorldTransform(List<Vector2> points,
                                                   Vector2 pos,
                                                   Vector2 forward,
                                                   Vector2 side,
                                                   Vector2 scale)
        {
            //Trace.WriteLineIf(debug, "Start WorldTransform 1", "Transformations");
            //Trace.WriteLineIf(debug, "Points in");
            foreach(Vector2 point in points)
            {
              //  Trace.WriteLineIf(debug, string.Format("\t\tPoint: ({0}, {1})", point.X, point.Y), "Transformations");
            }
            //Trace.WriteLineIf(debug, string.Format("\tpos = ({0}, {1})", pos.X, pos.Y), "Transformations");
            //Trace.WriteLineIf(debug, string.Format("\tforward = ({0}, {1})", forward.X, forward.Y), "Transformations");
            //Trace.WriteLineIf(debug, string.Format("\tside = ({0}, {1})", side.X, side.Y), "Transformations");
            //Trace.WriteLineIf(debug, string.Format("\tscale = ({0}, {1})", scale.X, scale.Y), "Transformations");

            //copy the original vertices into the buffer about to be transformed
            Vector2[] TranVector2s = points.ToArray();

            //create a transformation matrix
            Matrix matTransform = Matrix.Identity;

            //scale
            if ((scale.X != 1.0) || (scale.Y != 1.0))
            {
                Matrix.CreateScale(scale.X, scale.Y, 1.0f, out matTransform);
            }

            Vector2[] transformedVectors = new Vector2[points.Count()];
            matTransform *= new Matrix().MakeRotation(forward, side);
            matTransform *= Matrix.CreateTranslation(pos.X, pos.Y, 0);

            
            Vector2.Transform(TranVector2s, ref matTransform, transformedVectors);
            List<Vector2> vectors = new List<Vector2>(transformedVectors);
            foreach(Vector2 vector in vectors)
            {
//                Trace.WriteLineIf(debug, "Resulting Points ");
//                Trace.WriteLineIf(debug, string.Format("\t\tPoint: ({0}, {1})", vector.X, vector.Y), "Transformations");
                //Utils.TestVector(vector);
                
            }
//            Trace.WriteLineIf(debug, "End WorldTransform 1", "Transformations");
            return vectors;
        }

        

        //--------------------------- WorldTransform -----------------------------
        //
        //  given a List of 2D vectors, a position and  orientation
        //  this function transforms the 2D vectors into the object's world space
        //------------------------------------------------------------------------
        public static List<Vector2> WorldTransform(List<Vector2> points,
                                                   Vector2 pos,
                                                   Vector2 forward,
                                                   Vector2 side)
        {
//            Trace.WriteLineIf(debug, "Start WorldTransform 2", "Transformations");
            //copy the original vertices into the buffer about to be transformed
            Vector2[] TranVector2s = points.ToArray();

            //create a transformation matrix
            Matrix matTransform = new Matrix().MakeRotation(forward, side);

            //and translate
            matTransform *= Matrix.CreateTranslation(pos.X, pos.Y, 0);

            //now transform the object's vertices
            Vector2[] transformedVectors = new Vector2[points.Count()];
            Vector2.Transform(TranVector2s, ref matTransform, transformedVectors);
            List<Vector2> vectors = new List<Vector2>(transformedVectors);
            foreach (Vector2 vector in vectors)
            {
//                Utils.TestVector(vector);

            }
//            Trace.WriteLineIf(debug, "End WorldTransform 2", "Transformations");
            return vectors;
        }


        //--------------------- PointToWorldSpace --------------------------------
        //
        //  Transforms a point from the agent's local space into world space
        //------------------------------------------------------------------------
        public static Vector2 PointToWorldSpace(Vector2 point,
                                                Vector2 AgentHeading,
                                                Vector2 AgentSide,
                                                Vector2 AgentPosition)
        {
//            Trace.WriteLineIf(debug, "Start PointToWorldSpace", "Transformations");
            //create a transformation matrix
            Matrix matTransform = new Matrix().MakeRotation(AgentHeading, AgentSide);

            //and translate
            matTransform *= Matrix.CreateTranslation(AgentPosition.X, AgentPosition.Y, 1);

            //now transform the vertices
            Vector2 vector = Vector2.Transform(point, matTransform);
//            Utils.TestVector(vector);
//            Trace.WriteLineIf(debug, "End PointToWorldSpace", "Transformations");
            return vector;
        }

        //--------------------- VectorToWorldSpace --------------------------------
        //
        //  Transforms a vector from the agent's local space into world space
        //------------------------------------------------------------------------
        public static Vector2 VectorToWorldSpace(Vector2 vec,
                                                 Vector2 AgentHeading,
                                                 Vector2 AgentSide)
        {
//            Trace.WriteLineIf(debug, "Start VectorToWorldSpace", "Transformations");
            //create a transformation matrix
            Matrix matTransform = new Matrix().MakeRotation(AgentHeading, AgentSide);

            //now transform the vertices
            Vector2 vector = Vector2.Transform(vec, matTransform);
//            Utils.TestVector(vector);
//            Trace.WriteLineIf(debug, "End VectorToWorldSpace", "Transformations");
            return vector;
        }


        //--------------------- PointToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2 PointToLocalSpace(Vector2 point,
                                                Vector2 AgentHeading,
                                                Vector2 AgentSide,
                                                Vector2 AgentPosition)
        {
//            Trace.WriteLineIf(debug, "Start PointToLocalSpace", "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tpoint = ({0}, {1})", point.X, point.Y), "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tAgentHeading = ({0}, {1})", AgentHeading.X, AgentHeading.Y), "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tAgentSide = ({0}, {1})", AgentSide.X, AgentSide.Y), "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tAgentPosition = ({0}, {1})", AgentPosition.X, AgentPosition.Y), "Transformations");

            //create a transformation matrix
            Matrix matTransform = Matrix.Identity;

            float Tx = Vector2.Dot(-AgentPosition, AgentHeading);
            float Ty = Vector2.Dot(-AgentPosition, AgentSide);

            //create the transformation matrix
            matTransform.M11 = AgentHeading.X;
            matTransform.M12 = AgentSide.X;
            matTransform.M21 = AgentHeading.Y;
            matTransform.M22 = AgentSide.Y;
            matTransform.M13 = Tx;
            matTransform.M23 = Ty;

            //now transform the vertices
            Vector2 vector = Vector2.Transform(point, matTransform);
//            Utils.TestVector(vector);
//            Trace.WriteLineIf(debug, "End PointToLocalSpace", "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tvector = ({0}, {1})", vector.X, vector.Y), "Transformations");

            return vector;
        }


        //--------------------- VectorToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2 VectorToLocalSpace(Vector2 vec,
                                                 Vector2 AgentHeading,
                                                 Vector2 AgentSide)
        {
//            Trace.WriteLineIf(debug, "Start VectorToLocalSpace", "Transformations");
            //create a transformation matrix
            Matrix matTransform = Matrix.Identity;

            //create the transformation matrix
            matTransform.M11 = AgentHeading.X;
            matTransform.M12 = AgentSide.X;
            matTransform.M21 = AgentHeading.Y;
            matTransform.M22 = AgentSide.Y;


            //now transform the vertices
            Vector2 vector = Vector2.Transform(vec, matTransform);
//            Utils.TestVector(vector);
//            Trace.WriteLineIf(debug, "End VectorToLocalSpace", "Transformations");
            return vector;
        }

        //-------------------------- Vec2DRotateAroundOrigin --------------------------
        //
        //  rotates a vector ang rads around the origin
        //-----------------------------------------------------------------------------
        public static Vector2 Vec2DRotateAroundOrigin(Vector2 v, float ang)
        {
            if(ang == 0)
            {
                return v;
            }
//            Trace.WriteLineIf(debug, "Start Vec2DRotateAroundOrigin", "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tv = ({0}, {1})", v.X, v.Y), "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tang = {0}", ang), "Transformations");
            
            float newX = (float) (v.X*Math.Cos(ang) + v.Y*-Math.Sin(ang));
            float newY = (float) (v.X*Math.Sin(ang) + v.Y*Math.Cos(ang));

            //create a transformation matrix
           // Matrix mat = new Matrix().Make2DRotation(ang);

            //now transform the object's vertices
            //return Vector2.Transform(v, mat);
            Vector2 vector = new Vector2(newX, newY);
//            Utils.TestVector(vector);
//            Trace.WriteLineIf(debug, "End Vec2DRotateAroundOrigin", "Transformations");
//            Trace.WriteLineIf(debug, string.Format("\tvector = ({0}, {1})", vector.X, vector.Y), "Transformations");
            return vector;
        }

        //------------------------ CreateWhiskers ------------------------------------
        //
        //  given an origin, a facing direction, a 'field of view' describing the 
        //  limit of the outer whiskers, a whisker length and the number of whiskers
        //  this method returns a vector containing the end positions of a series
        //  of whiskers radiating away from the origin and with equal distance between
        //  them. (like the spokes of a wheel clipped to a specific segment size)
        //----------------------------------------------------------------------------
        public static List<Vector2> CreateWhiskers(int NumWhiskers,
                                                   float WhiskerLength,
                                                   float fov,
                                                   Vector2 facing,
                                                   Vector2 origin)
        {
//            Trace.WriteLineIf(debug, "Start CreateWhiskers", "Transformations");
            
            //this is the magnitude of the angle separating each whisker
            float SectorSize = fov/(NumWhiskers - 1);

            List<Vector2> whiskers = new List<Vector2>(NumWhiskers);
            Vector2 temp;
            float angle = -fov*0.5f;

            for (int w = 0; w < NumWhiskers; ++w)
            {
                //create the whisker extending outwards at this angle
                temp = facing;
                temp = Vec2DRotateAroundOrigin(temp, angle);
                whiskers.Add(origin + WhiskerLength*temp);

                angle += SectorSize;
            }

//            Trace.WriteLineIf(debug, "End CreateWhiskers", "Transformations");
            return whiskers;
        }
    }
}