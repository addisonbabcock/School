using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven
{
    public class GraveMarkers : IDisposable
    {
        public void Dispose()
        {
        }


        private struct GraveRecord
        {
            public Vector2 Position;
            public DateTime TimeCreated;

            public GraveRecord(Vector2 pos)
            {

                Position = pos;
                TimeCreated = DateTime.Now;
            }
        }


        //how long a grave remains on screen
        private double m_dLifeTime;

        //when a bot dies, a grave is rendered to mark the spot.
        private List<Vector2> m_vecRIPVB;
        private List<Vector2> m_vecRIPVBTrans;
        private List<GraveRecord> m_GraveList;




        public GraveMarkers(float lifetime)
        {
            m_dLifeTime = lifetime;
            m_GraveList = new List<GraveRecord>();
            if (m_vecRIPVB == null)
            {
                m_vecRIPVB = new List<Vector2>();
                m_vecRIPVB.Add(new Vector2(-4, -5));
                m_vecRIPVB.Add(new Vector2(-4, 3));
                m_vecRIPVB.Add(new Vector2(-3, 5));
                m_vecRIPVB.Add(new Vector2(-1, 6));
                m_vecRIPVB.Add(new Vector2(1, 6));
                m_vecRIPVB.Add(new Vector2(3, 5));
                m_vecRIPVB.Add(new Vector2(4, 3));
                m_vecRIPVB.Add(new Vector2(4, -5));
                m_vecRIPVB.Add(new Vector2(-4, -5));
            }
        }

        public void Update()
        {
            List<GraveRecord> recordsForRemvoal = new List<GraveRecord>();
            foreach (GraveRecord it in m_GraveList)
            {
                if (DateTime.Now > it.TimeCreated.AddSeconds(m_dLifeTime))
                {
                    recordsForRemvoal.Add(it);
                }
            }
            foreach (GraveRecord it in recordsForRemvoal)
            {
                m_GraveList.Remove(it);
            }

        }


        public void Render(PrimitiveBatch primitiveBatch, SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 facing = new Vector2(-1, 0);
            Vector2 facingPerp = facing.Perp();
            Vector2 vec1 = new Vector2(1, 1);
            foreach (GraveRecord it in m_GraveList)
            {
                Vector2 pos = it.Position;
                m_vecRIPVBTrans = Transformations.WorldTransform(m_vecRIPVB, ref pos, ref facing, ref facingPerp,
                                                                 ref vec1);
                Drawing.DrawClosedShaped(primitiveBatch, m_vecRIPVBTrans, Color.Brown);
                spriteBatch.DrawString(font, "RIP", new Vector2(it.Position.X, it.Position.Y), new Color(133, 90, 0));
            }
        }

        public void AddGrave(Vector2 pos)
        {
            m_GraveList.Add(new GraveRecord(pos));
        }

    }
}
