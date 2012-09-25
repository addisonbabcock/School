using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	/// <summary>
	/// A utility class for tracking the passing of time. Useful for various animations.
	/// </summary>
	class Timer
	{
		TimeSpan mTimeRemaining;
		TimeSpan mLength;

		/// <summary>
		/// Constructs a Timer.
		/// </summary>
		/// <param name="length">How long the timer should last.</param>
		public Timer(TimeSpan length)
		{
			mLength = new TimeSpan(length.Ticks);
			mTimeRemaining = new TimeSpan(length.Ticks);
		}

		/// <summary>
		/// Call this every frame to count down the timer.
		/// </summary>
		/// <param name="timePassed">How much time has passed since the last call to update.</param>
		/// <returns>True if the timer has completed.</returns>
		public bool Update(TimeSpan timePassed)
		{
			bool retVal = false;

			mTimeRemaining -= timePassed;

			if (mTimeRemaining.TotalMilliseconds <= 0.0)
			{
				//set it to 0 without allocating junk objects
				mTimeRemaining -= mTimeRemaining;
				retVal = true;
			}

			return retVal;
		}

		/// <summary>
		/// Calculates how close the timer is to being completed as a percentage.
		/// </summary>
		/// <returns>A percentage value (0.0 - 1.0) representing how close the timer is to being complete.</returns>
		public double PercentComplete()
		{
			return Math.Min (1.0, Math.Max (0.0, 1.0 - (mTimeRemaining.TotalMilliseconds / mLength.TotalMilliseconds)));
		}
	}
}
