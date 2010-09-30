using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace for_loop_example
{
	class Program
	{
		static void Main (string [] args)
		{
			int sum = 0;
			int num = 0;

			for (int i = 0; i < 4; ++i)
			{
				Console.Write ("Enter a number: ");
				num = Convert.ToInt32 (Console.ReadLine ());
				sum += num;
			}

			Console.WriteLine ("the sum of the numbers is: {0}", sum);
			Console.ReadKey ();
		}
	}
}
