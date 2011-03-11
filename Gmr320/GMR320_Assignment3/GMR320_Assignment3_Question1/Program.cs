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

			for (int i = 0; i < 360; ++i)
			{
				double newX = x * Math.Cos (1.0) - y * Math.Sin (1.0);
				double newY = x * Math.Sin (1.0) + y * Math.Cos (1.0);

				x = newX;
				y = newY;
			}

			Console.WriteLine ("Resulting point: {0}, {1}", x, y);
			Console.ReadLine ();
		}
	}
}
