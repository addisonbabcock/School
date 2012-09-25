#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using WestWorldWithMessaging.Messaging;

#endregion

namespace WestWorldWithMessaging
{
    public class MessageDispatcher
    {
        public const int NO_ADDITIONAL_INFO = 0;
        public const double SEND_MSG_IMMEDIATELY = 0.0;
        private static MessageDispatcher instance;


        //a std::set is used as the container for the delayed messages
        //because of the benefit of automatic sorting and avoidance
        //of duplicates. Messages are sorted by their dispatch time.
        private readonly SortedList<double, Telegram> PriorityQ;


        //this method is utilized by DispatchMessage or DispatchDelayedMessages.
        //This method calls the message handling member function of the receiving
        //entity, pReceiver, with the newly created telegram

        private MessageDispatcher()
        {
            PriorityQ = new SortedList<double, Telegram>();
        }

        private void Discharge(BaseGameEntity pReceiver, Telegram msg)
        {
            if (!pReceiver.HandleMessage(msg))
            {
                //telegram could not be handled
                Console.WriteLine("Message not handled");
            }
        }


        //this class is a singleton
        public static MessageDispatcher Instance()
        {
            if (instance == null)
            {
                instance = new MessageDispatcher();
            }
            return instance;
        }

        //send a message to another agent. Receiving agent is referenced by ID.
        public void DispatchMessage(double delay,
                                    int sender,
                                    int receiver,
                                    int msg,
                                    object ExtraInfo)
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            //get pointers to the sender and receiver
            BaseGameEntity pSender = EntityManager.Instance().GetEntityFromID(sender);
            BaseGameEntity pReceiver = EntityManager.Instance().GetEntityFromID(receiver);

            //make sure the receiver is valid
            if (pReceiver == null)
            {
                Console.WriteLine("{0}Warning! No receiver with ID of {1} found", Environment.NewLine, receiver);
                Console.BackgroundColor = currentBackgroundColor;
                Console.ForegroundColor = currentForegroundColor;
                return;
            }

            //create the telegram
            Telegram telegram = new Telegram(0, sender, receiver, msg, ExtraInfo);

            //if there is no delay, route telegram immediately                       
            if (delay <= 0.0f)
            {
                Console.WriteLine("{0}Instant telegram dispatched at time: {1} by {2} for {3}.  Msg is {4}",
                                  Environment.NewLine,
                                  DateTime.Now, EntityNames.GetNameOfEntity(pSender.ID()),
                                  EntityNames.GetNameOfEntity(pReceiver.ID()),
                                  MessageTypes.MsgToStr(msg));

                //send the telegram to the recipient
                Discharge(pReceiver, telegram);
            }

                //else calculate the time when the telegram should be dispatched
            else
            {
                double CurrentTime = DateTime.Now.Ticks;

                telegram.DispatchTime = CurrentTime + delay;

                //and put it in the queue
                PriorityQ.Add(telegram.DispatchTime, telegram);

                Console.WriteLine("{0}Delayed telegram from {1} recorded at time {2} for {3}. Msg is {4}",
                                  Environment.NewLine, EntityNames.GetNameOfEntity(pSender.ID()), DateTime.Now,
                                  EntityNames.GetNameOfEntity(pReceiver.ID()), MessageTypes.MsgToStr(msg));
            }
            Console.ForegroundColor = currentForegroundColor;
            Console.BackgroundColor = currentBackgroundColor;
        }

        //send out any delayed messages. This method is called each time through   
        //the main game loop.
        public void DispatchDelayedMessages()
        {
            ConsoleColor currentBackgroundColor = Console.BackgroundColor;
            ConsoleColor currentForegroundColor = Console.ForegroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;

            //get current time
            double CurrentTime = DateTime.Now.Ticks;

            //now peek at the queue to see if any telegrams need dispatching.
            //remove all telegrams from the front of the queue that have gone
            //past their sell by date
            while (PriorityQ.Count != 0 &&
                   (PriorityQ.ElementAt(0).Value.DispatchTime < CurrentTime) &&
                   (PriorityQ.ElementAt(0).Value.DispatchTime > 0))
            {
                //read the telegram from the front of the queue
                Telegram telegram = PriorityQ.ElementAt(0).Value;

                //find the recipient
                BaseGameEntity pReceiver = EntityManager.Instance().GetEntityFromID(telegram.Receiver);

                Console.WriteLine("{0}Queued telegram ready for dispatch: Sent to {1}. Msg is {2}"
                                  , Environment.NewLine, EntityNames.GetNameOfEntity(pReceiver.ID()),
                                  MessageTypes.MsgToStr(telegram.Msg));

                //send the telegram to the recipient
                Discharge(pReceiver, telegram);

                //remove it from the queue
                PriorityQ.RemoveAt(0);
            }

            Console.ForegroundColor = currentForegroundColor;
            Console.BackgroundColor = currentBackgroundColor;
        }
    }
}