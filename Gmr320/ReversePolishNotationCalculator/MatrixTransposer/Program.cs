using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixTransposer
{
	class Program
	{
		static List<int> Transpose (List<int> original)
		{
			List<int> output = new List<int> (original);

			int dim = (int)Math.Sqrt (original.Count);
			for (int x = 0; x < dim; ++x)
			{
				for (int y = 0; y < dim; ++y)
				{
					output [x + y * dim] = original [y + x * dim];
				}
			}

			return output;
		}

		static void Main (string [] args)
		{
			List<int> matrix = new List<int> ();
			for (int i = 1; i <= 9; ++i)
				matrix.Add (i);

			var transposed = Transpose (matrix);
			foreach (var trans in transposed)
				Console.Write ("{0} ", trans);
			Console.WriteLine ();


			List<int> bigMatrix = new List<int> ();
			for (int i = 1; i <= 16; ++i)
				bigMatrix.Add (i);

			var bigTranspose = Transpose (bigMatrix);
			foreach (var trans in bigTranspose)
				Console.Write ("{0} ", trans);
			Console.ReadKey ();
		}
	}
}
