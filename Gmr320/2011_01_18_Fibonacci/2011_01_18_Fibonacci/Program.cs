using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2011_01_18_Fibonacci
{
	class Program
	{
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

			Console.ReadKey ();
		}
	}
}
