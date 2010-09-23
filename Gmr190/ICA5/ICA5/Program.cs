using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA5
{
	class Program
	{
		static void Main (string [] args)
		{
			try
			{
				Console.WriteLine ("GMR 190 number comparator!");

				int high = 0;
				int num1 = 0;
				int num2 = 0;

				Console.Write ("Please input number 1: ");
				string sNum1 = Console.ReadLine ();

				Console.Write ("Please input number 2: ");
				string sNum2 = Console.ReadLine ();

				num1 = Convert.ToInt32 (sNum1);
				num2 = Convert.ToInt32 (sNum2);

				if (num1 > num2)
				{
					high = num1;
				}
				else
				{
					high = num2;
				}

				Console.Write ("The highest number is: {0}\n", high);
			}
			catch (Exception e)
			{
				Console.WriteLine ("Failed at something: {0}", e.Message);
			}
			finally
			{
				Console.ReadLine ();
			}
		}
	}
}
