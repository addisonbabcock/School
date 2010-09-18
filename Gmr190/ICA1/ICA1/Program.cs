using System;

namespace ICA1
{
	class Program
	{
		static void Main(string[] args)
		{
			double dSubtotal = 0.0;
			double dTotal = 0.0;
			double dGST = 0.0;

			try
			{
				Console.WriteLine("Please enter the Subtotal: ");
				dSubtotal = Convert.ToDouble(Console.ReadLine());

				Console.WriteLine("Please enter the GST (%):");
				dGST = Convert.ToDouble(Console.ReadLine());

				dTotal = dSubtotal + (dGST / 100.0 * dSubtotal);

				Console.WriteLine("Total is: {0:C}", dTotal);
			}
			catch (Exception e)
			{
				Console.WriteLine("Something went wrong! " + e.Message);
			}
			finally
			{
				Console.ReadLine();
			}
		}
	}
}
