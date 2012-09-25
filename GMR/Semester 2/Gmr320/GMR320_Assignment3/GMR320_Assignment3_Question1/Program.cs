using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMR320_Assignment3_Question1
{
	class Program
	{
		static void Main (string [] args)
		{
			double x = 1.0;
			double y = 1.0;
			double angle = Math.PI / 180.0;

			for (int i = 0; i < 360; ++i)
			{
				double newX = x * Math.Cos (angle) - y * Math.Sin (angle);
				double newY = x * Math.Sin (angle) + y * Math.Cos (angle);

				x = newX;
				y = newY;
			}

			Console.WriteLine ("Resulting point: {0}, {1}", x, y);
			Console.ReadLine ();

			//Don't do it this way!
			//Instead start each frame at 1.0, 1.0 then rotate by 1.0, 2.0, 3.0, etc
		}
	}
}
