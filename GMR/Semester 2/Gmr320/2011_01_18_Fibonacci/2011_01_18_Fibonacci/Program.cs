using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2011_01_18_Fibonacci
{
	class Program
	{
		static uint old_fib (uint seriesIndex)
		{
			uint a = 1;
			uint b = 1;
			for (uint i = 1; i < seriesIndex; ++i)
			{
				uint temp = b;
				b = a + b;
				a = temp;
			}

			return b;
		}

		static uint Fibonacci (uint seriesIndex)
		{
			if (seriesIndex <= 1)
				return 1;
			else
				return Fibonacci (seriesIndex - 1) + Fibonacci (seriesIndex - 2);
		}

		static void Main (string [] args)
		{
			for (uint i = 0; i < 10; ++i)
			{
				Console.WriteLine ("{0}", Fibonacci (i));
			}

			//Console.WriteLine ("{0}", Fibonacci (8));

			for (uint i = 0; i < 10; ++i)
			{
				Console.WriteLine ("{0}", old_fib (i));
			}

			Console.ReadKey ();
		}
	}
}
