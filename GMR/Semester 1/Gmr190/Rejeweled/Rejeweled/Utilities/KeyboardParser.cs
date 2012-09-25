using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Rejeweled
{
	class KeyboardEvent
	{
		public enum EventType
		{
			KeyPushed,
			KeyReleased,
			KeyBeingHeld,
		}

		private EventType mType;
		private Keys mKey;

		public EventType Type
		{
			get { return mType; }
		}

		public Keys Key
		{
			get { return mKey; }
		}

		public KeyboardEvent(EventType type, Keys key)
		{
			mType = type;
			mKey = key;
		}
	}

	class KeyboardParser
	{
		private KeyboardState mPreviousState;
		Queue<KeyboardEvent> mEvents;

		public void Update(KeyboardState currentState)
		{
			if (mPreviousState != null)
			{
				foreach (Keys currentKey in currentState.GetPressedKeys())
				{
					if (mPreviousState.IsKeyDown(currentKey))
					{
						mEvents.Enqueue(new KeyboardEvent(KeyboardEvent.EventType.KeyBeingHeld, currentKey));
					}
					else
					{
						mEvents.Enqueue(new KeyboardEvent(KeyboardEvent.EventType.KeyPushed, currentKey));
					}
				}

				foreach (Keys previousKey in mPreviousState.GetPressedKeys())
				{
					if (currentState.IsKeyUp(previousKey))
					{
						mEvents.Enqueue(new KeyboardEvent(KeyboardEvent.EventType.KeyReleased, previousKey));
					}
				}
			}
			mPreviousState = currentState;
		}

		public KeyboardParser()
		{
			mEvents = new Queue<KeyboardEvent>();
		}

		public KeyboardEvent GetNextEvent()
		{
			KeyboardEvent kbEvent = null;
			if (mEvents.Count > 0)
				kbEvent = mEvents.Dequeue();
			return kbEvent;
		}
	}
}
