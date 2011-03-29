using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common._2D;
using Common.Messaging;
using Common.Triggers;
using Raven.lua;

namespace Raven.triggers
{
    public class Trigger_SoundNotify : Trigger_LimitedLifeTime<AbstractBot>
{

  //a pointer to the bot that has made the sound
private  AbstractBot  m_pSoundSource;


  public Trigger_SoundNotify(AbstractBot source, float range):base(Constants.FrameRate /script.GetInt("Bot_TriggerUpdateFreq"))
  {
      m_pSoundSource = source;
  //set position and range
  SetPos(m_pSoundSource.Pos());

  SetBRadius(range);

  //create and set this trigger's region of fluence
  AddCircularTriggerRegion(Pos(), BRadius());
}


        public override void Dispose()
        {
            m_pSoundSource = null;
        }

        //------------------------------ Try ------------------------------------------
        //
        //  when triggered this trigger adds the bot that made the source of the sound 
        //  to the triggering bot's perception.
        //-----------------------------------------------------------------------------
        public override void Try(AbstractBot pBot)
        {
            //is this bot within range of this sound
            if (isTouchingTrigger(pBot.Pos(), pBot.BRadius()))
            {
                Dispatcher.DispatchMsg(MessageDispatcher.SEND_MSG_IMMEDIATELY,
                                        MessageDispatcher.SENDER_ID_IRRELEVANT,
                                        pBot.ID(),
                                        (int)message_type.Msg_GunshotSound,
                                        m_pSoundSource);
            }
        }

  public override void  Render(PrimitiveBatch batch){}
  private static MessageDispatcher Dispatcher = MessageDispatcher.Instance();
        private static Raven_Scriptor script = Raven_Scriptor.Instance();
    }
}
