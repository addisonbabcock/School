using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA7
{
	class Program
	{
		public static int GrabInt (string message)
		{
			int ret = 0;
			while (true)
			{
				try
				{
					Console.Write (message);
					ret = Convert.ToInt32 (Console.ReadLine ());
				}
				catch (Exception e)
				{
					Console.WriteLine ("Invalid input: " + e.Message);
					continue;
				}

				return ret;
			}
		}

		static void Main (string [] args)
		{
			int factorial = 0;
			int number = 0;
			int origNumber = 0;

			factorial = 1;

			origNumber = number = GrabInt ("Please enter the positive number you want to see the factorial of: ");

			while (number > 1)
			{
				factorial = factorial * number;
				number = number - 1;
			}

			Console.WriteLine ("The factorial of {0} is {1}", origNumber, factorial);
			Console.ReadKey ();
		}
	}
}
