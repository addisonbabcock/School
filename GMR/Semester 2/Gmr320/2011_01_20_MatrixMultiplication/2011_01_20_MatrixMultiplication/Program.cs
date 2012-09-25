using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2011_01_20_MatrixMultiplication
{
	class Program
	{
		static void Main (string [] args)
		{
			int [] a = { 1, 2, 3, 4 };
			int [] b = { 5, 7, 6, 8 };
			int [] c = new int [4];
			int dim = (int)Math.Sqrt (a.Length);
			int posInC = 0;

			for (int row = 0; row < a.Length; row += dim)
			{
				for (int col = 0; col < b.Length; col += dim)
				{
					c [posInC++] = a [row] * b [col] + a [row + 1] * b [col + 1];
				}
			}

			foreach (int i in c)
			{
				Console.Write ("{0} ", i);
			}
			Console.WriteLine ();
			Console.ReadKey ();
		}
	}
}
