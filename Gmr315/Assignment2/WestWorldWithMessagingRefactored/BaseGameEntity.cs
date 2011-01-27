#region Using

using System;
using WestWorldWithMessagingRefactored.Messaging;
using WestWorldWithMessagingRefactored.Misc;

#endregion

namespace WestWorldWithMessagingRefactored
{
    public abstract class BaseGameEntity
    {
        private static int _nextValidId;
        //every entity must have a unique identifying number
        private int _id;
        protected ConsoleColor OutputMessageColor = ConsoleColor.White;

        protected BaseGameEntity(int id)
        {
            ID = id;
        }

        public int ID
        {
            get { return _id; }
            private set
            {
                //make sure the val is equal to or greater than the next available ID
                if (value < _nextValidId)
                {
                    throw new ArgumentException(
                        "Set ID provided with an Invalid ID.  ID must be greater then or equal to the next available id");
                }

                _id = value;

                _nextValidId = _id + 1;
            }
        }

        public void OutputStatusMessage(string statusMessage)
        {
            ConsoleManager.SetOutputColor(OutputMessageColor);
            Console.WriteLine(string.Format("{0}: {1}", EntityManager.GetNameOfEntity(ID), statusMessage));
        }

        public abstract bool HandleMessage(Message message);
        public abstract void Update();
    }
}