using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactorialCalulator
{
	class Program
	{
		//Completes in O(N) time.
		static int Factorial (int a)
		{
			if (a == 0)
				return 1;

			return Factorial (a - 1) * a;
		}

		static void Main (string [] args)
		{
//			for (int i = 0; i < 10; ++i)
//				Console.WriteLine ("{0} - {1}", i, Factorial ((i));

			Console.WriteLine ("Enter a positive number: ");
			int input = Convert.ToInt32 (Console.ReadLine ());
			Console.WriteLine ("{0}! = {1}", input, Factorial (input));
			Console.ReadKey ();
		}
	}
}
