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
			int [] arr = { 1, 2, 3, 4 };

			//reallocate
			int [] temp = new int [arr.Length + 1];
			for (int i = 0; i < arr.Length; ++i)
			{
				temp [i] = arr [i];
			}
			temp [arr.Length] = 5;

			arr = temp;

			Console.ReadKey ();
		}
	}
}
