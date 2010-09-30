using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace do_while_exercise2
{
	class Program
	{
		static void Main (string [] args)
		{
			int count = 0;
			int highTotal = 0;
			int numDays = 0;
			int dailyHigh = 0;
			int averageHigh = 0;
			string monthName = "";

			Console.Write ("Please enter the month: ");
			monthName = Console.ReadLine ();

			Console.Write ("Please enter the number of days: ");
			numDays = Convert.ToInt32 (Console.ReadLine ());

			do
			{
				Console.Write ("Please enter the daily high temperature: ");
				dailyHigh = Convert.ToInt32 (Console.ReadLine ());
				highTotal += dailyHigh;
				++count;
			} while (count < numDays);

			averageHigh = highTotal / numDays;
			Console.Write ("The average high temperature for the month of {0} was {1}", monthName, averageHigh);
			Console.ReadKey ();
		}
	}
}
