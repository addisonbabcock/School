using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rejeweled
{
	class Timer
	{
		TimeSpan mTimeRemaining;
		TimeSpan mLength;

		public Timer(TimeSpan length)
		{
			mLength = new TimeSpan(length.Ticks);
			mTimeRemaining = new TimeSpan(length.Ticks);
		}

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

		public double PercentComplete()
		{
			return 1.0 - (mTimeRemaining.TotalMilliseconds / mLength.TotalMilliseconds);
		}
	}
}
