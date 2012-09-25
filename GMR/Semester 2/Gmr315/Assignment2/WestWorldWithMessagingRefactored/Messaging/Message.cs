#region Using

using System;

#endregion

namespace WestWorldWithMessagingRefactored.Messaging
{
    public class Message
    {
        /// <summary>
        /// The minimum amount of time a message must wait until dispatched
        /// if it has not been sent immediately.
        /// </summary>
        public const double SmallestDelay = 0.25;

        /// <summary>
        /// The time which this message should be dispatched.
        /// </summary>
        public double DispatchTime;

        /// <summary>
        /// A message to be sent
        /// </summary>
        /// <param name="time">The time at which this message should be sent</param>
        /// <param name="sender">Sender's unique id</param>
        /// <param name="receiver">Receiver's unique id</param>
        /// <param name="message">Message to be sent</param>
        /// <param name="additionalInfo">Any additional information</param>
        public Message(double time,
                       int sender,
                       int receiver,
                       MessageType message,
                       object additionalInfo)
        {
            DispatchTime = time;
            SenderId = sender;
            ReceiverId = receiver;
            MessageType = message;
            AdditionalInfo = additionalInfo;
        }

        /// <summary>
        /// Any additional info supplied with the message
        /// </summary>
        public object AdditionalInfo { get; set; }

        /// <summary>
        /// The actual message to be sent
        /// </summary>
        public MessageType MessageType { get; set; }

        /// <summary>
        /// The intended receivers unique id
        /// </summary>
        public int ReceiverId { get; set; }

        /// <summary>
        ///  The senders unique id
        /// </summary>
        public int SenderId { get; set; }

        /// <summary>
        /// Messages will be stored in a priority queue. Therefore the boolean
        /// operators need to be overloaded so that the priority queue
        /// can later sort the messages by time priority.  Note how the times
        /// must be smaller than SmallestDelay apart before two messages are
        /// considered unique.
        /// </summary>
        /// <param name="message1">Message 1</param>
        /// <param name="message2">Message 2</param>
        /// <returns>true if these messages are the same</returns>
        public static bool operator ==(Message message1, Message message2)
        {
            return (Math.Abs(message1.DispatchTime - message2.DispatchTime) < SmallestDelay) &&
                   (message1.SenderId == message2.SenderId) &&
                   (message1.ReceiverId == message2.ReceiverId) &&
                   (message1.MessageType == message2.MessageType);
        }

        /// <summary>
        /// Messages will be stored in a priority queue. Therefore the boolean
        /// operators need to be overloaded so that the priority queue
        /// can later sort the messages by time priority.  Note how the times
        /// must be smaller than SmallestDelay apart before two messages are
        /// considered unique.
        /// </summary>
        /// <param name="message1">Message 1</param>
        /// <param name="message2">Message 2</param>
        /// <returns>true if these messages are not equal</returns>
        public static bool operator !=(Message message1, Message message2)
        {
            return !(message1 == message2);
        }

        /// <summary>
        /// Override of the equals operator to compare two messages
        /// </summary>
        /// <param name="obj">Potential message to be compared</param>
        /// <returns>true if both messages are the same</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Message message = obj as Message;
            if (message == null)
            {
                return false;
            }
            return this == message;
        }

        public override int GetHashCode()
        {
            return DispatchTime.GetHashCode() ^ MessageType.GetHashCode() ^ ReceiverId.GetHashCode() ^
                   SenderId.GetHashCode();
        }

        /// <summary>
        /// Comapares messages dispatch time to see which should be sent
        /// sooner
        /// </summary>
        /// <param name="message1">Message 1</param>
        /// <param name="message2">Message 2</param>
        /// <returns>true if message 1 dispatch time is less than message 2's dispatch time</returns>
        public static bool operator <(Message message1, Message message2)
        {
            if (message1 == message2)
            {
                return false;
            }

            return (message1.DispatchTime < message2.DispatchTime);
        }

        /// <summary>
        /// Comapares messages dispatch time to see which should be sent
        /// later
        /// </summary>
        /// <param name="message1">Message 1</param>
        /// <param name="message2">Message 2</param>
        /// <returns>true if message 1 dispatch time is greater than message 2's dispatch time</returns>
        public static bool operator >(Message message1, Message message2)
        {
            if (message1 == message2)
            {
                return false;
            }

            return (message1.DispatchTime > message2.DispatchTime);
        }

        /// <summary>
        /// Generate a string comprising the messages details
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            {
                return
                    string.Format(
                        "Dispatch time: {0} Sender's Unique ID: {1} Receiver's Unique Id:{2} MessageType: {3}",
                        DispatchTime, SenderId, ReceiverId, MessageType);
            }
        }
    }
}