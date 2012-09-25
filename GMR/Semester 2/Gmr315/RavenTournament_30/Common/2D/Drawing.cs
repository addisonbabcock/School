#region Using

using System;
using System.Collections.Generic;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Common._2D
{
    public class Drawing
    {
        public static void DrawRectangle(PrimitiveBatch batch, Color color, float left, float top, float width,
                                         float height)
        {
            Vector2 topLeft = new Vector2(left, top);
            Vector2 topRight = new Vector2(left + width, top);
            Vector2 bottomRight = new Vector2(left + width, top + height);
            Vector2 bottomLeft = new Vector2(left, top + height);
            // Top line
            batch.AddVertex(topLeft, color);
            batch.AddVertex(topRight, color);

            // Right Line
            batch.AddVertex(topRight, color);
            batch.AddVertex(bottomRight, color);

            // Bottom Line
            batch.AddVertex(bottomRight, color);
            batch.AddVertex(bottomLeft, color);

            // Left Line
            batch.AddVertex(bottomLeft, color);
            batch.AddVertex(topLeft, color);
        }

        public static void DrawLine(PrimitiveBatch batch, Vector2 vertex1, Vector2 vertex2, Color color)
        {
            
            batch.AddVertex(vertex1, color);
            batch.AddVertex(vertex2, color);
        }

        public static void DrawCircle(PrimitiveBatch batch, Vector2 center, float radius, Color color)
        {
            List<Vector2> vectors = CreateEllipseVectorsFor2DDrawing(center, radius, radius, 64);
            for (int i = 0; i < vectors.Count - 1; i++)
            {
                batch.AddVertex(vectors[i], color);
                batch.AddVertex(vectors[i + 1], color);
            }
        }

        public static void DrawFilledCircle(PrimitiveBatch batch, Vector2 center, float radius, Color color)
        {
            List<Vector2> vectors;
            for (float circleWidth = 0.1f; circleWidth < radius; circleWidth = circleWidth + 0.1f)
            {
                 vectors = CreateEllipseVectorsFor2DDrawing(center, circleWidth, circleWidth, 64);
                for (int i = 0; i < vectors.Count - 1; i++)
                {
                    batch.AddVertex(vectors[i], color);
                    batch.AddVertex(vectors[i + 1], color);
                }
            }
            vectors = CreateEllipseVectorsFor2DDrawing(center, radius, radius, 64);
            for (int i = 0; i < vectors.Count - 1; i++)
            {
                batch.AddVertex(vectors[i], color);
                batch.AddVertex(vectors[i + 1], color);
            }
        }

        private static List<Vector2> CreateEllipseVectorsFor2DDrawing(Vector2 position, float semimajor_axis,
                                                                      float semiminor_axis, int sides)
        {
            List<Vector2> vectors = new List<Vector2>();
            vectors.Clear();
            float max = 2.0f*(float) Math.PI;

            //Utils.TestFloat(sides, true);
            float step = max/sides;
            float h = position.X;
            float k = position.Y;

            for (float t = 0.0f; t < max; t += step)
            {
                // center point: (h,k); add as argument if you want (to circumvent modifying this.Position)
                // x = h + a*cos(t)  -- a is semimajor axis, b is semiminor axis
                // y = k + b*sin(t)
                vectors.Add(new Vector2((float) (h + semimajor_axis*Math.Cos(t)),
                                        (float) (k + semiminor_axis*Math.Sin(t))));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float) (h + semimajor_axis*Math.Cos(step)),
                                    (float) (k + semiminor_axis*Math.Sin(step))));

            return vectors;
        }

        public static void DrawClosedShaped(PrimitiveBatch batch, List<Vector2> points, Color color)
        {
            for (int p = 1; p < points.Count; ++p)
            {
                batch.AddVertex(points[p - 1], color);
                batch.AddVertex(points[p], color);
            }
            batch.AddVertex(points[0], color);
            batch.AddVertex(points[points.Count - 1], color);
        }

        public static void DrawFilledRectangle(SpriteBatch batch, Color fillColor, float left, float top, float width,
                                               float height)
        {
            Texture2D texture2D = new Texture2D(batch.GraphicsDevice, (int) width, (int) height);
            Color[] color = new Color[(int) width*(int) height];
            for (int i = 0; i < width*height; i++)
            {
                color[i] = fillColor;
            }

            texture2D.SetData(color);
        }

        public static void DrawLineWithArrow(PrimitiveBatch batch, Vector2 startPoint, Vector2 endPoint, int arrowSize, Color color)
        {
            Vector2 norm = Vector2.Normalize(startPoint - endPoint);

            //calculate where the arrow is attached
            Vector2 CrossingPoint = startPoint - (norm*arrowSize);

            //calculate the two extra points required to make the arrowhead
            Vector2 ArrowPoint1 = CrossingPoint + (norm.Perp()*0.4f*arrowSize);
            Vector2 ArrowPoint2 = CrossingPoint - (norm.Perp()*0.4f*arrowSize);

            DrawLine(batch, endPoint, CrossingPoint, color);
            DrawLine(batch, CrossingPoint, ArrowPoint1, color);
            DrawLine(batch, ArrowPoint1, ArrowPoint2, color);
            DrawLine(batch, ArrowPoint2, startPoint, color);
        }
    }
}