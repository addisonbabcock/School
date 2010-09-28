using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA6
{
	class Program
	{
		static void Main (string [] args)
		{
			int iNumber = 0;

			while (iNumber < 5)
			{
				iNumber = iNumber + 1;
				Console.WriteLine ("This is the {0}th time looping!", iNumber);
			}

			Console.ReadKey ();
		}
	}
}
