#region Using

using System.Collections.Generic;
using Common.Game;
using Common.Misc;

#endregion

namespace Common.Messaging
{
    public class MessageDispatcher
    {
        //to make code easier to read
        public const int NO_ADDITIONAL_INFO = 0;
        public const double SEND_MSG_IMMEDIATELY = 0.0;
        public const int SENDER_ID_IRRELEVANT = -1;


        //a std::set is used as the container for the delayed messages
        //because of the benefit of automatic sorting and avoidance
        //of duplicates. Messages are sorted by their dispatch time.


        private static MessageDispatcher instance;

        private readonly EntityManager EntityMgr = EntityManager.Instance();
        private readonly FrameCounter TickCounter = FrameCounter.Instance();
        private SortedDictionary<double, Telegram> PriorityQ;

        private MessageDispatcher()
        {
        }

        private void Discharge(BaseGameEntity pReceiver, Telegram telegram)
        {
            if (!pReceiver.HandleMessage(telegram))
            {
                //telegram could not be handled
#if SHOW_MESSAGING_INFO
    debug.WriteLine("Message not handled");
#endif
            }
        }

        public static MessageDispatcher Instance()
        {
            if (instance == null)
            {
                instance = new MessageDispatcher();
            }
            return instance;
        }

        //send a message to another agent. Receiving agent is referenced by ID.
        public void DispatchMsg(double delay,
                                int sender,
                                int receiver,
                                int msg,
                                object AdditionalInfo)
        {
            //get a pointer to the receiver
            BaseGameEntity pReceiver = EntityMgr.GetEntityFromID(receiver);

            //make sure the receiver is valid
            if (pReceiver == null)
            {
                Debug.WriteLine("Warning! No Receiver with ID of "+receiver+" found");


                return;
            }

            //create the telegram
            Telegram telegram = new Telegram(0, sender, receiver, msg, AdditionalInfo);

            //if there is no delay, route telegram immediately                       
            if (delay <= 0.0)
            {
                Debug.WriteLine("Telegram dispatched at time: " + TickCounter.GetCurrentFrame()
                                + " by " + sender + " for " + receiver
                                + ". Msg is " + msg);

                //send the telegram to the recipient
                Discharge(pReceiver, telegram);
            }

                //else calculate the time when the telegram should be dispatched
            else
            {
                double CurrentTime = TickCounter.GetCurrentFrame();

                telegram.DispatchTime = CurrentTime + delay;

                //and put it in the queue
                PriorityQ.Add(telegram.DispatchTime, telegram);

                Debug.WriteLine("Delayed telegram from " + sender + " recorded at time "
                                + TickCounter.GetCurrentFrame() + " for " + receiver
                                + ". Msg is " + msg);

            }
        }

        //send out any delayed messages. This method is called each time through   
        //the main game loop.
        //---------------------- DispatchDelayedMessages -------------------------
//
//  This function dispatches any telegrams with a timestamp that has
//  expired. Any dispatched telegrams are removed from the queue
//------------------------------------------------------------------------

        public void DispatchDelayedMessages()
        {
            //first get current time
            double CurrentTime = TickCounter.GetCurrentFrame();

            //now peek at the queue to see if any telegrams need dispatching.
            //remove all telegrams from the front of the queue that have gone
            //past their sell by date

            while (PriorityQ.Count != 0 &&
                   (PriorityQ[0].DispatchTime < CurrentTime) &&
                   (PriorityQ[0].DispatchTime > 0))
            {
                //read the telegram from the front of the queue
                Telegram telegram = PriorityQ[0];

                //find the recipient
                BaseGameEntity pReceiver = EntityMgr.GetEntityFromID(telegram.Receiver);


                Debug.WriteLine("Queued telegram ready for dispatch: Sent to "
                                + pReceiver.ID() + ". Msg is " + telegram.Msg + "");

                //send the telegram to the recipient
                Discharge(pReceiver, telegram);

                //remove it from the queue
                PriorityQ.Remove(telegram.DispatchTime);
            }
        }
    }
}