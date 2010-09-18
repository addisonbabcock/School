using System;

namespace ICA1
{
	class Program
	{
		private const double DBL_TaxMultiplier = 1.07;
		static void Main(string[] args)
		{
			double Subtotal = 0.0;
			double Total = 0.0;

			while (true)
			{
				Console.WriteLine(ICA1.Properties.Resources.SubtotalPrompt);

				try
				{
					Subtotal = Convert.ToDouble(Console.ReadLine());
				}
				catch (FormatException ex)
				{
					//try again
					Console.WriteLine(ICA1.Properties.Resources.FormatExceptionMessage + ex.Message);
					continue;
				}
				catch (OverflowException ex)
				{
					//try again
					Console.WriteLine(ICA1.Properties.Resources.OverflowExceptionMessage + ex.Message);
					continue;
				}

				break;
			}

			Total = Subtotal * DBL_TaxMultiplier;

			Console.WriteLine(ICA1.Properties.Resources.TotalOutput, Total);
			Console.ReadLine();
		}
	}
}
