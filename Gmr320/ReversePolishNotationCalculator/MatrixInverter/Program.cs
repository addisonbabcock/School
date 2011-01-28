using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MatrixInverter
{
	class Program
	{
		int [,] Invert (int [,] original)
		{
			/*i := 1
			j := 1
			while (i ≤ m and j ≤ n) do
			  Find pivot in column j, starting in row i:
			  maxi := i
			  for k := i+1 to m do
				if abs(A[k,j]) > abs(A[maxi,j]) then
				  maxi := k
				end if
			  end for
			  if A[maxi,j] ≠ 0 then
				swap rows i and maxi, but do not change the value of i
				Now A[i,j] will contain the old value of A[maxi,j].
				divide each entry in row i by A[i,j]
				Now A[i,j] will have the value 1.
				for u := i+1 to m do
				  subtract A[u,j] * row i from row u
				  Now A[u,j] will be 0, since A[u,j] - A[i,j] * A[u,j] = A[u,j] - 1 * A[u,j] = 0.
				end for
				i := i + 1
			  end if
			  j := j + 1
			end while*/

			int i = 0, j = 0;
			while (i < original.Length && j < original.Length)
			{
				int max = i;
				for (int k = i + 1; k < original.Length; ++k)
				{
				}
			}
		}

		static void Main (string [] args)
		{

		}
	}
}
