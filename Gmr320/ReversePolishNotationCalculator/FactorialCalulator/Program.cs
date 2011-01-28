using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FactorialCalulator
{
	class Program
	{
		static UInt64 Factorial (UInt64 a)
		{
			if (a == 0)
				return 1;

			return Factorial (a - 1) * a;
		}

		static void Main (string [] args)
		{
			for (int i = 0; i < 10; ++i)
				Console.WriteLine ("{0} - {1}", i, Factorial ((UInt64)i));
			Console.ReadKey ();
		}
	}
}
