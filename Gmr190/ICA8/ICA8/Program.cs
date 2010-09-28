using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA8
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
			int numberInSeries;
			int nextNumberInSeries;
			int number;
			int save;

			numberInSeries = 1;
			nextNumberInSeries = 1;

			number = GrabInt ("Please enter how many positive numbers in the Fibionacci series you would like to see: ");
			number -= 1;

			Console.Write ("Number in series: {0}", numberInSeries);

			while (number > 0)
			{
				Console.Write (" {0}", nextNumberInSeries);
				save = nextNumberInSeries;
				nextNumberInSeries += numberInSeries;
				numberInSeries = save;
				number -= 1;
			}

			Console.ReadKey ();
		}
	}
}
