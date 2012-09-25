using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2011_01_20_ArrayResizer
{
	class Program
	{
		static void Main (string [] args)
		{
			int [] arr = { 1, 2, 3, 4, 5, 6 };

			//reallocate
			int [] temp = new int [arr.Length + 1];
			for (int i = 0; i < arr.Length; ++i)
			{
				temp [i] = arr [i];
			}
			temp [arr.Length] = 7;

			arr = temp;

			foreach (int i in arr)
				Console.Write ("{0} ", i);

			Console.ReadKey ();
		}
	}
}
