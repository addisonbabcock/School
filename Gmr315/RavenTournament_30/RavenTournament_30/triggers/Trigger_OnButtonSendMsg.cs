using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Game;
using Common.Messaging;
using Common.Misc;
using Common.Triggers;
using Microsoft.Xna.Framework.Graphics;
using Raven.lua;
using Microsoft.Xna.Framework;

namespace Raven.triggers
{
    public class Trigger_OnButtonSendMsg<T> : Trigger<T> where T : BaseGameEntity
    {
        public override void Dispose()
        {

        }
        private static Raven_Scriptor script = Raven_Scriptor.Instance();
        private static MessageDispatcher Dispatcher = MessageDispatcher.Instance();

        //when triggered a message is sent to the entity with the following ID
        private int m_iReceiver;

        //the message that is sent
        private int m_iMessageToSend;



        public Trigger_OnButtonSendMsg(StreamReader datafile) :

            base(StreamUtilities.GetIntFromStream(datafile))
        {
            Read(datafile);
        }

        public Trigger_OnButtonSendMsg(string[] tokens) :

            base(int.Parse(tokens[1]))
        {
            LoadData(tokens);
        }

        public override void Try(T pEnt)
        {

            if (isTouchingTrigger(pEnt.Pos(), pEnt.BRadius()))
            {
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                        this.ID(),
                                        m_iReceiver,
                                        m_iMessageToSend,
                                        MessageDispatcher.NO_ADDITIONAL_INFO);

            }
        }

        public override void Update()
        {
        }

        public override void Render(PrimitiveBatch batch)
        {
            float sz = BRadius();
           
               Drawing.DrawLine(batch, new Vector2(Pos().X - sz, Pos().Y - sz), new Vector2(Pos().X + sz, Pos().Y - sz), Color.Orange);
               Drawing.DrawLine(batch, new Vector2(Pos().X + sz, Pos().Y - sz), new Vector2(Pos().X + sz, Pos().Y + sz), Color.Orange);
               Drawing.DrawLine(batch, new Vector2(Pos().X + sz, Pos().Y + sz), new Vector2(Pos().X - sz, Pos().Y + sz), Color.Orange);
               Drawing.DrawLine(batch, new Vector2(Pos().X - sz, Pos().Y + sz), new Vector2(Pos().X - sz, Pos().Y - sz), Color.Orange);
        }

        public override void Write(StreamWriter writer)
        {
        }

        public override void Read(StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] tokens = line.Split(' ');
            LoadData(tokens);

        }

        private void LoadData(string[] tokens)
        {
            m_iReceiver = int.Parse(tokens[2]);
            //grab the message type
            m_iMessageToSend = int.Parse(tokens[3]);

            //grab the position and radius
            float x, y, r;
            x = float.Parse(tokens[4]);
            y = float.Parse(tokens[5]);
            r = float.Parse(tokens[6]);
            SetPos(new Vector2(x, y));
            SetBRadius(r);

            //create and set this trigger's region of fluence
            AddRectangularTriggerRegion(Pos() - new Vector2(BRadius(), BRadius()), //top left corner
                                        Pos() + new Vector2(BRadius(), BRadius())); //bottom right corner
        }


        public bool HandleMessage(Telegram msg)
        {
            return false;
        }
    }
}
