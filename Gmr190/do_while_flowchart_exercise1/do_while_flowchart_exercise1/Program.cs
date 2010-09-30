using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace do_while_flowchart_exercise1
{
	class Program
	{
		static void Main (string [] args)
		{
			int total = 0;
			int count = 0;
			int num = 0;
			double average = 0;

			do
			{
				try
				{
					Console.Write ("Please enter a number: ");
					num = Convert.ToInt32 (Console.ReadLine ());
					total += num;
					++count;
				}
				catch (Exception e)
				{
					Console.WriteLine ("Input failed: " + e.Message);
				}
			} while (count < 5);

			average = (double)total / (double)count;

			Console.WriteLine ("The average is: {0}", average);
			Console.ReadKey ();
		}
	}
}
