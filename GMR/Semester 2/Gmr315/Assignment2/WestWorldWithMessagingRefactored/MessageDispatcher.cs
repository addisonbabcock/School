#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.Misc;

#endregion

namespace WestWorldWithMessagingRefactored
{
    public class MessageDispatcher
    {
        /// <summary>
        /// Value to be sent when no additional information is attached.
        /// </summary>
        public const int NoAdditionalInfo = 0;

        /// <summary>
        /// Time frame for sending messages immediately.
        /// </summary>
        public const double SendMessageImmediately = 0.0;

        /// <summary>
        /// Singleton instance of the dispatch manager
        /// </summary>
        private static MessageDispatcher instance;

        /// <summary>
        /// Using a sorted list for automatically sorting the delayed
        /// messags and avoiding duplicates.  Messages are sorted by
        /// their dispatch time.
        /// </summary>
        private readonly SortedList<double, Message> _priorityQueue;

        /// <summary>
        /// Private constructor for the message dispatcher responsible
        /// for intializing any of the structures it needs to deal with
        /// messages.
        /// </summary>
        private MessageDispatcher()
        {
            _priorityQueue = new SortedList<double, Message>();
        }

        /// <summary>
        /// this method is utilized by DispatchMessage or DispatchDelayedMessages.
        ///This method calls the message handling member function of the receiving
        ///entity, messageRecepient, with the newly created telegram
        /// </summary>
        /// <param name="messageRecepient">Intended receipient of this message</param>
        /// <param name="message">The message</param>
        private static void Discharge(BaseGameEntity messageRecepient, Message message)
        {
            if (!messageRecepient.HandleMessage(message))
            {
                //telegram could not be handled
                Console.WriteLine("Message not handled");
            }
        }


        /// <summary>
        /// Accessor for the singleton instance.
        /// </summary>
        /// <returns>The message dispatcher object</returns>
        public static MessageDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageDispatcher();
                }
                return instance;
            }
        }

        /// <summary>
        /// Send a message to the receiptient
        /// </summary>
        /// <param name="delay">The amount of time to wait before sending the message</param>
        /// <param name="senderId">The sender's unique id.</param>
        /// <param name="receiverId">The recepient's unique id.</param>
        /// <param name="messageType">The type of message to send</param>
        /// <param name="extraInfo">Any additional extra information</param>
        public void DispatchMessage(double delay,
                                    int senderId,
                                    int receiverId,
                                    MessageType messageType,
                                    object extraInfo)
        {
          
            BaseGameEntity sender = EntityManager.Instance.GetEntityFromID(senderId);
            BaseGameEntity receiver = EntityManager.Instance.GetEntityFromID(receiverId);

            //make sure the receiver is valid
            if (receiver == null)
            {
                OutputMessage(string.Format("Warning! No receiver with ID of {0} found", receiverId));
                return;
            }

            //create the telegram
            Message message = new Message(0, senderId, receiverId, messageType, extraInfo);

            //if there is no delay, route telegram immediately                       
            if (delay <= 0.0f)
            {
                OutputMessage(string.Format("Instant telegram dispatched at time: {0} by {1} for {2}.  Msg is {3}",
                                  DateTime.Now.Ticks, EntityManager.GetNameOfEntity(senderId),
                                  EntityManager.GetNameOfEntity(receiverId),
                                  messageType));

                //send the telegram to the recipient
                Discharge(receiver, message);
            }

                //else calculate the time when the telegram should be dispatched
            else
            {
                double CurrentTime = DateTime.Now.Ticks;

                message.DispatchTime = CurrentTime + delay;

                //and put it in the queue
                _priorityQueue.Add(message.DispatchTime, message);

                OutputMessage(string.Format("Delayed telegram from {0} recorded at time {1} for {2}. Msg is {3}",
                                 EntityManager.GetNameOfEntity(senderId), DateTime.Now.Ticks,
                                  EntityManager.GetNameOfEntity(receiverId), message.MessageType));
            }
        }

        //send out any delayed messages. This method is called each time through   
        //the main game loop.
        public void DispatchDelayedMessages()
        {
            //get current time
            double currentTime = DateTime.Now.Ticks;

            //now peek at the queue to see if any telegrams need dispatching.
            //remove all telegrams from the front of the queue that have gone
            //past their sell by date
            while (_priorityQueue.Count != 0 &&
                   (_priorityQueue.ElementAt(0).Value.DispatchTime < currentTime) &&
                   (_priorityQueue.ElementAt(0).Value.DispatchTime > 0))
            {
                //read the telegram from the front of the queue
                Message message = _priorityQueue.ElementAt(0).Value;

                //find the recipient
                BaseGameEntity pReceiver = EntityManager.Instance.GetEntityFromID(message.ReceiverId);

                OutputMessage(string.Format("Queued telegram ready for dispatch: Sent to {0}. Msg is {1}"
                                  , EntityManager.GetNameOfEntity(pReceiver.ID),
                                  message.MessageType));

                //send the telegram to the recipient
                Discharge(pReceiver, message);

                //remove it from the queue
                _priorityQueue.RemoveAt(0);
            }
        }

        private static void OutputMessage(string message)
        {
            ConsoleManager.SetOutputColor(ConsoleColor.White, ConsoleColor.Red);
            Console.WriteLine(message);
        }

        public static void OutputHandledMessage(int id)
        {

            OutputMessage(string.Format("Message handled by {0} at time: {1}", EntityManager.GetNameOfEntity(id),
                                        DateTime.Now.Ticks));
        }
    }
}