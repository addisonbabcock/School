using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA9
{
	class Program
	{
		static void Main (string [] args)
		{
			int count = 1;

			try
			{
				Console.Write ("Please enter how many times to loop: 9");
				count = Convert.ToInt32 (Console.ReadLine ());

				do
				{
					Console.WriteLine (count);
					--count;
				} while (count > 0);
			}
			catch (Exception e)
			{
				Console.WriteLine (e.Message);
			}

			Console.ReadKey ();
		}
	}
}
