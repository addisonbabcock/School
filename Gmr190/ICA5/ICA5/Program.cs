using System;

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
				num1 = Convert.ToInt32 (Console.ReadLine ());

				Console.Write ("Please input number 2: ");
				num2 = Convert.ToInt32 (Console.ReadLine ());

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
