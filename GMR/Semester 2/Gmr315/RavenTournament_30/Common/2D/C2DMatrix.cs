using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Misc;
using Microsoft.Xna.Framework;

namespace Common._2D
{
    public class C2DMatrix
    {



        private struct Matrix
        {

            internal float _11, _12, _13;
            internal float _21, _22, _23;
            internal float _31, _32, _33;


        }

        private Matrix m_Matrix;

        //multiplies m_Matrix with mIn
        private void MatrixMultiply(ref Matrix mIn)
        {
            C2DMatrix.Matrix mat_temp;

            //first row
            mat_temp._11 = (m_Matrix._11 * mIn._11) + (m_Matrix._12 * mIn._21) + (m_Matrix._13 * mIn._31);
            mat_temp._12 = (m_Matrix._11 * mIn._12) + (m_Matrix._12 * mIn._22) + (m_Matrix._13 * mIn._32);
            mat_temp._13 = (m_Matrix._11 * mIn._13) + (m_Matrix._12 * mIn._23) + (m_Matrix._13 * mIn._33);

            //second
            mat_temp._21 = (m_Matrix._21 * mIn._11) + (m_Matrix._22 * mIn._21) + (m_Matrix._23 * mIn._31);
            mat_temp._22 = (m_Matrix._21 * mIn._12) + (m_Matrix._22 * mIn._22) + (m_Matrix._23 * mIn._32);
            mat_temp._23 = (m_Matrix._21 * mIn._13) + (m_Matrix._22 * mIn._23) + (m_Matrix._23 * mIn._33);

            //third
            mat_temp._31 = (m_Matrix._31 * mIn._11) + (m_Matrix._32 * mIn._21) + (m_Matrix._33 * mIn._31);
            mat_temp._32 = (m_Matrix._31 * mIn._12) + (m_Matrix._32 * mIn._22) + (m_Matrix._33 * mIn._32);
            mat_temp._33 = (m_Matrix._31 * mIn._13) + (m_Matrix._32 * mIn._23) + (m_Matrix._33 * mIn._33);

            m_Matrix = mat_temp;
        }



        public C2DMatrix()
        {
            //initialize the matrix to an identity matrix
            Identity();
        }

        //create an identity matrix
        public void Identity()
        {
            m_Matrix._11 = 1; m_Matrix._12 = 0; m_Matrix._13 = 0;

            m_Matrix._21 = 0; m_Matrix._22 = 1; m_Matrix._23 = 0;

            m_Matrix._31 = 0; m_Matrix._32 = 0; m_Matrix._33 = 1;

        }

        //create a transformation matrix
        public void Translate(float x, float y)
        {
            //Utils.TestFloat(x, y);
            
            Matrix mat;

            mat._11 = 1; mat._12 = 0; mat._13 = 0;

            mat._21 = 0; mat._22 = 1; mat._23 = 0;

            mat._31 = x; mat._32 = y; mat._33 = 1;

            //and multiply
            MatrixMultiply(ref mat);
        }

        //create a scale matrix
        public void Scale(float xScale, float yScale)
        {
            //Utils.TestFloat(xScale, yScale);
            C2DMatrix.Matrix mat;

            mat._11 = xScale; mat._12 = 0; mat._13 = 0;

            mat._21 = 0; mat._22 = yScale; mat._23 = 0;

            mat._31 = 0; mat._32 = 0; mat._33 = 1;

            //and multiply
            MatrixMultiply(ref mat);
        }

        //create a rotation matrix
        public void Rotate(float rotation)
        {
//            Utils.TestFloat(rotation);
            C2DMatrix.Matrix mat;

            float Sin = (float)Math.Sin(rotation);
            float Cos = (float)Math.Cos(rotation);

            mat._11 = Cos; mat._12 = Sin; mat._13 = 0;

            mat._21 = -Sin; mat._22 = Cos; mat._23 = 0;

            mat._31 = 0; mat._32 = 0; mat._33 = 1;

            //and multiply
            MatrixMultiply(ref mat);
        }

        //create a rotation matrix from a fwd and side 2D vector
        public void Rotate(ref Vector2 fwd, ref Vector2 side)
        {
//            Utils.TestVector(fwd, side);
            C2DMatrix.Matrix mat;

            mat._11 = fwd.X; mat._12 = fwd.Y; mat._13 = 0;

            mat._21 = side.X; mat._22 = side.Y; mat._23 = 0;

            mat._31 = 0; mat._32 = 0; mat._33 = 1;

            //and multiply
            MatrixMultiply(ref mat);
        }

        //applys a transformation matrix to a List of points
        public void TransformVector2s(ref List<Vector2> vPoints)
        {
//            Utils.TestVector(vPoints);
            for (int i = 0; i < vPoints.Count(); ++i)
            {
                float tempX = (m_Matrix._11 * vPoints[i].X) + (m_Matrix._21 * vPoints[i].Y) + (m_Matrix._31);

                float tempY = (m_Matrix._12 * vPoints[i].X) + (m_Matrix._22 * vPoints[i].Y) + (m_Matrix._32);

                vPoints[i] = new Vector2(tempX, tempY);
//                Utils.TestVector(vPoints[i]);
            }
        }
        //applys a transformation matrix to a point
        public void TransformVector2s(ref Vector2 vPoint)
        {
//            Utils.TestVector(vPoint);

            float tempX = (m_Matrix._11 * vPoint.X) + (m_Matrix._21 * vPoint.Y) + (m_Matrix._31);

            float tempY = (m_Matrix._12 * vPoint.X) + (m_Matrix._22 * vPoint.Y) + (m_Matrix._32);

            vPoint = new Vector2(tempX, tempY);
//            Utils.TestVector(vPoint);
        }


        //accessors to the matrix elements
        public void _11(float val)
        {
//            Utils.TestFloat(val);
            m_Matrix._11 = val;
        }
        public void _12(float val) { //Utils.TestFloat(val); 
            m_Matrix._12 = val; }
        public void _13(float val) { //Utils.TestFloat(val); 
            m_Matrix._13 = val; }

        public void _21(float val) { //Utils.TestFloat(val); 
            m_Matrix._21 = val; }
        public void _22(float val) { //Utils.TestFloat(val); 
            m_Matrix._22 = val; }
        public void _23(float val) { //Utils.TestFloat(val); 
            m_Matrix._23 = val; }

        public void _31(float val) { //Utils.TestFloat(val); 
            m_Matrix._31 = val; }
        public void _32(float val) { //Utils.TestFloat(val); 
            m_Matrix._32 = val; }
        public void _33(float val) { //Utils.TestFloat(val); 
            m_Matrix._33 = val; }

    }


}

