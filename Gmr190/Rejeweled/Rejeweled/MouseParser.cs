using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rejeweled
{
	class MouseEvent
	{
		/// <summary>
		/// Stores information about a mouse event.
		/// </summary>
		public enum EventType
		{
			MouseClick,
			MouseDrag
		}

		private EventType mEvent;
		private Vector2 mMouseLocation;
		private Vector2 mDragStart;
		private Vector2 mDragEnd;

		public EventType MouseEventType
		{
			get { return mEvent; }
		}

		public Vector2 MouseLocation
		{
			get 
			{
				if (MouseEventType == EventType.MouseClick)
					return mMouseLocation;
				throw new InvalidOperationException ("Attempted to access MouseLocation when the event type is not MouseClick.");
			}
		}

		public Vector2 DragStart
		{
			get
			{
				if (MouseEventType != EventType.MouseDrag)
					throw new InvalidOperationException ("Attempted to access DragStart when the event type is not MouseDrag.");
				return mDragStart;
			}
		}

		public Vector2 DragEnd
		{
			get
			{
				if (MouseEventType != EventType.MouseDrag)
					throw new InvalidOperationException ("Attempted to access DragEnd when the event type is not MouseDrag.");
				return mDragEnd;
			}
		}

		public MouseEvent (EventType type, Vector2 location)
		{
			mEvent = type;
			mMouseLocation = location;
		}

		public MouseEvent (Vector2 dragStart, Vector2 dragEnd)
		{
			mEvent = EventType.MouseDrag;
			mDragStart = dragStart;
			mDragEnd = dragEnd;
		}
	}

	/// <summary>
	/// Parses mouse input into a format that's easier to use.
	/// </summary>
	class MouseParser
	{
		private MouseState mPreviousState;
		private MouseState mDragOrigin;
		private bool mIsDragging;
		private Queue<MouseEvent> mEvents;

		public void Update (MouseState currentMouseState)
		{
			if (mPreviousState != null)
			{
				//look for clicks
				if (mPreviousState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released &&
					mPreviousState.X == currentMouseState.X && mPreviousState.Y == currentMouseState.Y)
				{
					//deal with a left mouse click
					if (!mIsDragging)
					{
						mEvents.Enqueue (
							new MouseEvent (
								MouseEvent.EventType.MouseClick,
								new Vector2 (
									(float)currentMouseState.X,
									(float)currentMouseState.Y)));
					}
				}

				//was clicked and is now released
				if (mPreviousState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Released)
				{
					if (mIsDragging)
					{
						//queue drag event
						mEvents.Enqueue (
							new MouseEvent (
								new Vector2 (
									(float)mDragOrigin.X,
									(float)mDragOrigin.Y),
								new Vector2 (
									(float)currentMouseState.X,
									(float)currentMouseState.Y)));
					}
				}

				//look for dragging
				if (mPreviousState.LeftButton == ButtonState.Pressed && currentMouseState.LeftButton == ButtonState.Pressed &&
					(mPreviousState.X != currentMouseState.X || mPreviousState.Y != currentMouseState.Y))
				{
					if (!mIsDragging)
						mDragOrigin = mPreviousState;
					mIsDragging = true;
				}

				if (currentMouseState.LeftButton == ButtonState.Released)
				{
					mIsDragging = false;
				}
			}

			//MUST BE LAST!
			mPreviousState = currentMouseState;
		}

		public MouseParser ()
		{
			mEvents = new Queue<MouseEvent> ();
		}

		public MouseEvent GetNextEvent ()
		{
			MouseEvent mouseEvent = null;
			if (mEvents.Count > 0)
				mouseEvent = mEvents.Dequeue ();
			return mouseEvent;
		}
	}
}
