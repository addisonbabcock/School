#region Using

using System;
using System.IO;
using Common.Interfaces;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common._2D
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Wall2D : IDisposable
    {
        private static PrimitiveBatch primitiveBatch;

        protected
            Vector2 m_vA,
                    m_vB,
                    m_vN;

        public void Dispose()
        {
        }

        public
            Wall2D()
            : this(new Vector2(), new Vector2())
        {
        }

        public Wall2D(Vector2 A, Vector2 B)
        {
            SetFrom(A);
            SetTo(B);
            CalculateNormal();
        }

        public Wall2D(Vector2 A, Vector2 B, Vector2 N)
        {
            SetFrom(A);
            SetTo(B);
            SetNormal(N);
        }

        public Wall2D(StreamReader inputStream)
        {
            Read(inputStream);
        }

        public Wall2D(String[] tokens)
        {
            LoadData(tokens);
        }


        protected void CalculateNormal()
        {
            Vector2 temp = Vector2.Normalize(m_vB - m_vA);

            m_vN = new Vector2(-temp.Y, temp.X);
        }

        public void Draw(PrimitiveBatch batch)
        {
            Draw(batch, false, Color.White);
        }

        public void Draw(PrimitiveBatch batch, Color color)
        {
            Draw(batch, false, color);
        }

        public void Draw(PrimitiveBatch batch, bool RenderNormals, Color color)
        {
            Drawing.DrawLine(batch, m_vA, m_vB, color);

            //render the normals if rqd
            if (RenderNormals)
            {
                int MidX = (int) ((m_vA.X + m_vB.X)/2);
                int MidY = (int) ((m_vA.Y + m_vB.Y)/2);

                Drawing.DrawLine(batch, new Vector2(MidX, MidY),
                                 new Vector2(MidX + (m_vN.X*5f), MidY + (m_vN.Y*5f)), color);
            }
        }

        public Vector2 From()
        {
            return m_vA;
        }

        public void SetFrom(Vector2 v)
        {
//            Utils.TestVector(v);
            m_vA = v;
            CalculateNormal();
        }

        public Vector2 To()
        {
            return m_vB;
        }

        public void SetTo(Vector2 v)
        {
//            Utils.TestVector(v);
            m_vB = v;
            CalculateNormal();
        }

        public Vector2 Normal()
        {
            return m_vN;
        }

        public void SetNormal(Vector2 n)
        {
//            Utils.TestVector(n);
            m_vN = n;
        }

        public Vector2 Center()
        {
            return (m_vA + m_vB)/2.0f;
        }

        public void Write(StreamWriter outputStream)
        {
            outputStream.Write(From().X);
            outputStream.Write(",");
            outputStream.Write(From().Y);
            outputStream.Write(",");
            outputStream.Write(To().X);
            outputStream.Write(",");
            outputStream.Write(To().Y);
            outputStream.Write(",");
            outputStream.Write(Normal().X);
            outputStream.Write(",");
            outputStream.WriteLine(Normal().Y);
        }

        private void Read(StreamReader inputStream)
        {
            String line = inputStream.ReadLine();
            string[] items = line.Split(',');
            LoadData(items);
            
        }

        private void LoadData(string[] items)
        {
            SetFrom(new Vector2(float.Parse(items[2]), float.Parse(items[3])));
            SetTo(new Vector2(float.Parse(items[5]), float.Parse(items[6])));
            SetNormal(new Vector2(float.Parse(items[8]), float.Parse(items[9])));
        }
    }
}
