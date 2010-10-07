using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_06_Tip_Calculator
{
	class Program
	{
		public static double CalculateTip(double billAmount, double tipRate)
		{
			return billAmount * tipRate / 100.0;
		}

		public static bool IsInInclusiveRange(double amount, double min, double max)
		{
			return amount >= min && amount <= max;
		}

		public static double InputDouble(string message, double min, double max)
		{
			double ret = 0.0;

			do
			{
				try
				{
					Console.Write(message);
					ret = Convert.ToDouble(Console.ReadLine());

					if (IsInInclusiveRange(ret, min, max))
						return ret;
					else
						Console.WriteLine("Input value {0} was outside of range {1}-{2}.", ret, min, max);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			} while (true);
		}

		static void Main(string[] args)
		{
			double billAmount = -1.0;
			double tipRate = -1.0;

			billAmount = InputDouble("Enter bill amount: ", 1.5, 200.0);
			tipRate = InputDouble("Enter tip rate: ", 0.0, 60.0);

			try
			{
				double tipAmount = CalculateTip(billAmount, tipRate);
				Console.WriteLine("Tip Amount: {0}", tipAmount);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}
	}
}
