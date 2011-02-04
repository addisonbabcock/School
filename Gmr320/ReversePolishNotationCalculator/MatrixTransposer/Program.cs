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

			Console.WriteLine ("Original 3x3 matrix: ");
			foreach (var i in matrix)
				Console.Write ("{0} ", i);
			Console.WriteLine ();

			var transposed = Transpose (matrix);

			Console.WriteLine ("Transposed 3x3 matrix:");
			foreach (var trans in transposed)
				Console.Write ("{0} ", trans);
			Console.WriteLine ();


			List<int> bigMatrix = new List<int> ();
			for (int i = 1; i <= 16; ++i)
				bigMatrix.Add (i);

			Console.WriteLine ("Original 4x4 matrix:");
			foreach (var i in bigMatrix)
				Console.Write ("{0} ", i);
			Console.WriteLine ();

			var bigTranspose = Transpose (bigMatrix);

			Console.WriteLine ("Transposed 4x4 matrix:");
			foreach (var trans in bigTranspose)
				Console.Write ("{0} ", trans);
			Console.ReadKey ();
		}
	}
}
