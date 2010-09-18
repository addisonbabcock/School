using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA1
{
	class Program
	{
		static void Main(string[] args)
		{
			double Subtotal;
			double Total;

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

			Total = Subtotal * 1.07;

			Console.WriteLine(ICA1.Properties.Resources.TotalOutput, Total.ToString());

			Console.ReadLine();
		}
	}
}
