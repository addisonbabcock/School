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
    public class Transformations
    {
        private static List<Vector2> CopyList(List<Vector2> points)
        {
            List<Vector2> newList = new List<Vector2>(points.Count());
            foreach (Vector2 point in points)
            {
                newList.Add(new Vector2(point.X, point.Y));
            }
            return newList;
        }
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
            //copy the original vertices into the buffer about to be transformed
            List<Vector2> TranVector2s = CopyList(points);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //scale
            if ((scale.X != 1.0) || (scale.Y != 1.0))
            {
                matTransform.Scale(scale.X, scale.Y);
            }

            //rotate
            matTransform.Rotate(forward, side);

            //and translate
            matTransform.Translate(pos.X, pos.Y);

            //now transform the object's vertices
            matTransform.TransformVector2s(ref TranVector2s);

            return TranVector2s;
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
            //copy the original vertices into the buffer about to be transformed
            List<Vector2> TranVector2s = CopyList(points);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //rotate
            matTransform.Rotate(forward, side);

            //and translate
            matTransform.Translate(pos.X, pos.Y);

            //now transform the object's vertices
            matTransform.TransformVector2s(ref TranVector2s);

            return TranVector2s;
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
            //make a copy of the point
            Vector2 TransPoint = new Vector2(point.X, point.Y);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);

            //and translate
            matTransform.Translate(AgentPosition.X, AgentPosition.Y);

            //now transform the vertices
            matTransform.TransformVector2s(ref TransPoint);

            return TransPoint;
        }

        //--------------------- VectorToWorldSpace --------------------------------
        //
        //  Transforms a vector from the agent's local space into world space
        //------------------------------------------------------------------------
        public static Vector2 VectorToWorldSpace(Vector2 vec,
                                             Vector2 AgentHeading,
                                             Vector2 AgentSide)
        {
            //make a copy of the point
            Vector2 TransVec = new Vector2(vec.X, vec.Y);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //rotate
            matTransform.Rotate(AgentHeading, AgentSide);

            //now transform the vertices
            matTransform.TransformVector2s(ref TransVec);

            return TransVec;
        }


        //--------------------- PointToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2 PointToLocalSpace(Vector2 point,
                                     Vector2 AgentHeading,
                                     Vector2 AgentSide,
                                      Vector2 AgentPosition)
        {

            //make a copy of the point
            Vector2 TransPoint = new Vector2(point.X, point.Y);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();


            float Tx = -Microsoft.Xna.Framework.Vector2.Dot(AgentPosition, AgentHeading);
            float Ty = -Microsoft.Xna.Framework.Vector2.Dot(AgentPosition, AgentSide);

            //create the transformation matrix
            matTransform._11(AgentHeading.X); matTransform._12(AgentSide.X);
            matTransform._21(AgentHeading.Y); matTransform._22(AgentSide.Y);
            matTransform._31(Tx); matTransform._32(Ty);

            //now transform the vertices
            matTransform.TransformVector2s(ref TransPoint);

            return TransPoint;
        }

        //--------------------- VectorToLocalSpace --------------------------------
        //
        //------------------------------------------------------------------------
        public static Vector2 VectorToLocalSpace(Vector2 vec,
                                     Vector2 AgentHeading,
                                     Vector2 AgentSide)
        {

            //make a copy of the point
            Vector2 TransPoint = new Vector2(vec.X, vec.Y);

            //create a transformation matrix
            C2DMatrix matTransform = new C2DMatrix();

            //create the transformation matrix
            matTransform._11(AgentHeading.X); matTransform._12(AgentSide.X);
            matTransform._21(AgentHeading.Y); matTransform._22(AgentSide.Y);

            //now transform the vertices
            matTransform.TransformVector2s(ref TransPoint);

            return TransPoint;
        }

        //-------------------------- Vec2DRotateAroundOrigin --------------------------
        //
        //  rotates a vector ang rads around the origin
        //-----------------------------------------------------------------------------
        public static Vector2 Vec2DRotateAroundOrigin(Vector2 v, float ang)
        {
            //create a transformation matrix
            C2DMatrix mat = new C2DMatrix();

            //rotate
            mat.Rotate(ang);

            //now transform the object's vertices
            mat.TransformVector2s(ref v);
            return v;
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
            //this is the magnitude of the angle separating each whisker
            float SectorSize = fov / (float)(NumWhiskers - 1);

            List<Vector2> whiskers = new List<Vector2>();
            Vector2 temp;
            float angle = -fov * 0.5f;

            for (int w = 0; w < NumWhiskers; ++w)
            {
                //create the whisker extending outwards at this angle
                temp = facing;
                Vec2DRotateAroundOrigin(temp, angle);
                whiskers.Add(origin + WhiskerLength * temp);

                angle += SectorSize;
            }

            return whiskers;
        }
    }
}