using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Common.Misc;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raven
{
    public class Raven_Door : BaseGameEntity, IDisposable
    {
        public void Dispose()
        {
            m_pWall2 = null;
            m_pWall1 = null;
        }
        protected enum door_status { open, opening, closed, closing };

        protected door_status m_Status;

        //a sliding door is created from two walls, back to back.These walls must
        //be added to a map's geometry in order for an agent to detect them
        protected Wall2D m_pWall1;
        protected Wall2D m_pWall2;

        //a container of the id's of the triggers able to open this door
        protected List<int> m_Switches;

        //how long the door remains open before it starts to shut again
        protected int m_iNumTicksStayOpen;

        //how long the door has been open (0 if status is not open)
        protected int m_iNumTicksCurrentlyOpen;

        //the door's position and size when in the open position
        protected Vector2 m_vP1;
        protected Vector2 m_vP2;
        protected float m_dSize;

        //a normalized vector facing along the door. This is used frequently
        //by the other methods so we might as well just calculate it once in the
        //ctor
        protected Vector2 m_vtoP2Norm;

        //the door's current size
        protected float m_dCurrentSize;

        protected void Open()
        {
            if (m_Status == door_status.opening)
            {
                if (m_dCurrentSize < 2)
                {
                    m_Status = door_status.open;

                    m_iNumTicksCurrentlyOpen = m_iNumTicksStayOpen;

                    return;

                }

                //reduce the current size
                m_dCurrentSize -= 1;

                Utils.Clamp(ref m_dCurrentSize, 0, m_dSize);

                ChangePosition(m_vP1, m_vP1 + m_vtoP2Norm * m_dCurrentSize);

            }
        }

        protected void Close()
        {
            if (m_Status == door_status.closing)
            {
                if (m_dCurrentSize == m_dSize)
                {
                    m_Status = door_status.closed;
                    return;

                }

                //reduce the current size
                m_dCurrentSize += 1;

                Utils.Clamp(ref m_dCurrentSize, 0, m_dSize);

                ChangePosition(m_vP1, m_vP1 + m_vtoP2Norm * m_dCurrentSize);

            }
        }

        protected void ChangePosition(Vector2 newP1, Vector2 newP2)
        {

            m_vP1 = newP1;
            m_vP2 = newP2;

            m_pWall1.SetFrom(m_vP1 + m_vtoP2Norm.Perp());
            m_pWall1.SetTo(m_vP2 + m_vtoP2Norm.Perp());

            m_pWall2.SetFrom(m_vP2 - m_vtoP2Norm.Perp());
            m_pWall2.SetTo(m_vP1 - m_vtoP2Norm.Perp());

        }



        public Raven_Door(ref Raven_Map pMap, StreamReader inputStream)
            : base(Common.Misc.StreamUtilities.GetIntFromStream(inputStream))
        {
            m_Status = door_status.closed;
            m_iNumTicksStayOpen = 60;

            Read(inputStream);

            m_vtoP2Norm = Vector2.Normalize(m_vP2 - m_vP1);
            m_dCurrentSize = m_dSize = Vector2.Distance(m_vP2, m_vP1);

            Vector2 perp = m_vtoP2Norm.Perp();

            //create the walls that make up the door's geometry
            m_pWall1 = pMap.AddWall(m_vP1 + perp, m_vP2 + perp);
            m_pWall2 = pMap.AddWall(m_vP2 - perp, m_vP1 - perp);

        }
        public Raven_Door(ref Raven_Map pMap, string[] tokens)
            : base(int.Parse(tokens[1]))
        {
            m_Status = door_status.closed;
            m_iNumTicksStayOpen = 60;

            LoadData(tokens);

            m_vtoP2Norm = Vector2.Normalize(m_vP2 - m_vP1);
            m_dCurrentSize = m_dSize = Vector2.Distance(m_vP2, m_vP1);

            Vector2 perp = m_vtoP2Norm.Perp();

            //create the walls that make up the door's geometry
            m_pWall1 = pMap.AddWall(m_vP1 + perp, m_vP2 + perp);
            m_pWall2 = pMap.AddWall(m_vP2 - perp, m_vP1 - perp);

        }

        //the usual suspects
        public void Render(PrimitiveBatch batch)
        {
            Drawing.DrawLine(batch, m_vP1, m_vP2, Color.DarkBlue);
        }

        public override void Update()
        {
            switch (m_Status)
            {
                case door_status.opening:

                    Open();
                    break;

                case door_status.closing:

                    Close();
                    break;

                case door_status.open:

                    if (m_iNumTicksCurrentlyOpen-- < 0)
                    {
                        m_Status = door_status.closing;
                    }

                    break;
            }


        }

        public override bool HandleMessage(Telegram msg)
        {
            if (msg.Msg == (int) message_type.Msg_OpenSesame)
            {
                if (m_Status != door_status.open)
                {
                    m_Status = door_status.opening;
                }

                return true;
            }

            return false;
        }

        public override void Read(StreamReader inputStream)
        {
            float x, y;
            string line = inputStream.ReadLine();
            string[] tokens = line.Split(',');

            LoadData(tokens);
        }

        private void LoadData(string[] tokens)
        {
            //grab the hinge points
            m_vP1 = new Vector2(float.Parse(tokens[2]), float.Parse(tokens[3]));
            m_vP2 = new Vector2(float.Parse(tokens[4]), float.Parse(tokens[5]));


            //grab the number of triggers
            int num;

            num = int.Parse(tokens[6]);
            //save the trigger IDs
            for (int i = 1; i <= num; ++i)
            {

                m_Switches.Add(int.Parse(tokens[6+i]));
            }
        }


        //adds the ID of a switch
        public void AddSwitch(int id)
        {
            //only add the trigger if it isn't already present
            if (!m_Switches.Contains(id))
            {
                m_Switches.Add(id);
            }
        }

        public List<int> GetSwitchIDs()
        {
            return m_Switches;
        }
    }
}
