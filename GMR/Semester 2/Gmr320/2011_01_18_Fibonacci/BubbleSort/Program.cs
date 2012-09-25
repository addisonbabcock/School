using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BubbleSort
{
	class Program
	{
		static Random rng;

		static void BubbleSort (List<int> arr)
		{
			for (int pass = 1; pass < arr.Count; ++pass)
			{
				bool didSwap = false;

				for (int i = 0; i < arr.Count - pass; ++i)
				{
					if (arr [i] > arr [i + 1])
					{
						int temp = arr [i];
						arr [i] = arr [i + 1];
						arr [i + 1] = temp;
						didSwap = true;
					}
				}

				if (!didSwap)
					return;
			}
		}

		static void Main (string [] args)
		{
			rng = new Random ();
			List<int> arr = new List<int> ();
			for (int i = 0; i < 15; ++i)
			{
				arr.Add (rng.Next (100));
			}

			BubbleSort (arr);

			foreach (int i in arr)
			{
				Console.Write ("{0} ", i);
			}
			Console.ReadKey ();
		}
	}
}
