#region Using

using System;

#endregion

namespace WestWorldWithMessaging.Messaging
{
    public class Telegram
    {
        //the entity that sent this telegram

        //messages can be dispatched immediately or delayed for a specified amount
        //of time. If a delay is necessary this field is stamped with the time 
        //the message should be dispatched.
        public const double SmallestDelay = 0.25;
        public double DispatchTime;

        //any additional information that may accompany the message
        public object ExtraInfo;
        public int Msg;
        public int Receiver;
        public int Sender;


        public Telegram()
            : this(-1, -1, -1, -1, null)
        {
        }

        public Telegram(double time,
                        int sender,
                        int receiver,
                        int msg)
            : this(time, sender, receiver, msg, null)
        {
        }

        public Telegram(double time,
                        int sender,
                        int receiver,
                        int msg,
                        object info)
        {
            DispatchTime = time;
            Sender = sender;
            Receiver = receiver;
            Msg = msg;
            ExtraInfo = info;
        }


        //these telegrams will be stored in a priority queue. Therefore the >
        //operator needs to be overloaded so that the PQ can sort the telegrams
        //by time priority. Note how the times must be smaller than
        //SmallestDelay apart before two Telegrams are considered unique.


        public static bool operator ==(Telegram t1, Telegram t2)
        {
            return (Math.Abs(t1.DispatchTime - t2.DispatchTime) < SmallestDelay) &&
                   (t1.Sender == t2.Sender) &&
                   (t1.Receiver == t2.Receiver) &&
                   (t1.Msg == t2.Msg);
        }

        public static bool operator !=(Telegram t1, Telegram t2)
        {
            return !(t1 == t2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Telegram telegram = obj as Telegram;
            if (telegram == null)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return DispatchTime.GetHashCode() ^ Msg.GetHashCode() ^ Receiver.GetHashCode() ^ Sender.GetHashCode();
        }

        public static bool operator <(Telegram t1, Telegram t2)
        {
            if (t1 == t2)
            {
                return false;
            }

            return (t1.DispatchTime < t2.DispatchTime);
        }

        public static bool operator >(Telegram t1, Telegram t2)
        {
            if (t1 == t2)
            {
                return false;
            }

            return (t1.DispatchTime > t2.DispatchTime);
        }

        public override string ToString()
        {
            {
                return "time: " + DispatchTime + "  Sender: " + Sender
                       + "   Receiver: " + Receiver + "   Msg: " + Msg;
            }
        }
    }
}