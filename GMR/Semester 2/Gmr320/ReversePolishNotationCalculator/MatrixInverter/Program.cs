using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixInverter
{
	class Program
	{
		static void ExchangeRows (double [,] data, int rowA, int rowB)
		{
			double temp;

			for (int i = 0; i < data.GetLength (1); ++i)
			{
				temp = data [rowA, i];
				data [rowA, i] = data [rowB, i];
				data [rowB, i] = temp;
			}
		}

		static void ScaleRow (double [,] data, double scale, int row)
		{
			for (int i = 0; i < data.GetLength (1); ++i)
				data [row, i] *= scale;
		}

		static void ScaleRowAdd (double [,] data, double scale, int sourceRow, int destRow)
		{
			for (int i = 0; i < data.GetLength (1); ++i)
				data [destRow, i] += scale * data [sourceRow, i];
		}

		static double [,] Invert (double [,] original)
		{
			//initialize with identity matrix
			double [,] inverse = new double [original.GetLength (0), original.GetLength (1)];
			for (int i = 0; i < inverse.GetLength (0); ++i)
				for (int j = 0; j < inverse.GetLength (1); ++j)
					inverse [i, j] = i == j ? 1.0 : 0.0;

			for (int column = 0; column < original.GetLength (1); ++column)
			{
				if (Math.Abs (original [column, column]) <= 2.0 * double.Epsilon)
				{
					for (int row = column + 1; row < original.GetLength (0); ++ row)
					{
						if (Math.Abs (original [row, column]) <= 2.0 * double.Epsilon)
						{
							ExchangeRows (original, column, row);
							ExchangeRows (inverse, column, row);
							break;
						}
					}
				}

				double scale = 1.0 / original [column, column];
				ScaleRow (original, scale, column);
				ScaleRow (inverse, scale, column);

				for (int row = 0; row < original.GetLength (0); ++row)
				{
					if (row != column)
					{
						double scale2 = -original [row, column];
						ScaleRowAdd (original, scale, column, row);
						ScaleRowAdd (inverse, scale, column, row);
					}
				}
			}

			return inverse;
		}

		static void Main (string [] args)
		{
			Console.WriteLine ("Never did bother to finish this... ignore it.");
			double [,] matrix = new double [3, 3];
			for (int i = 0; i < 3; ++i)
				for (int j = 0; j < 3; ++j)
					matrix [i, j] = (double)i;

			Console.WriteLine ("Pre-inversion: ");
			for (int i = 0; i < 3; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					Console.Write ("{0} ", matrix [i,j]);
				}
				Console.WriteLine ();
			}

			var inverted = Invert (matrix);

			Console.WriteLine ("Post-inversion: ");
			for (int i = 0; i < 3; ++i)
			{
				for (int j = 0; j < 3; ++j)
				{
					Console.Write ("{0} ", inverted [i, j]);
				}
				Console.WriteLine ();
			}

			Console.ReadKey ();
		}
	}
}
