using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_06_calculator
{
	class Program
	{
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
						Console.WriteLine("Input value {0} was outside of range [{1}-{2}].", ret, min, max);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			} while (true);
		}

		public static char ProcessMenu(string message, string values)
		{
			string input;
			char c;
			while (true)
			{
				Console.Write(message);
				input = Console.ReadLine();
				c = input[0];

				if (values.Contains(c))
				{
					return c;
				}

				Console.WriteLine("Must be one of the following options: " + values);
			}
		}

		public static double Add(double a, double b)
		{
			return a + b;
		}

		public static double Subtract(double a, double b)
		{
			return a - b;
		}

		public static double Multiply(double a, double b)
		{
			return a * b;
		}

		public static double Divide(double a, double b)
		{
			return a / b;
		}

		static void Main(string[] args)
		{
			double a = 0.0;
			double b = 0.0;
			double result = 0.0;
			char op = ' ';

			a = InputDouble("Please enter a number: ", double.MinValue, double.MaxValue);
			b = InputDouble("Please enter another number: ", double.MinValue, double.MaxValue);
			op = ProcessMenu("Select an operator (ASMD): ", "ASMDasmd");

			switch (op)
			{
				case 'a':
				case 'A':
					result = Add(a, b);
					break;

				case 's':
				case 'S':
					result = Subtract(a, b);
					break;

				case 'm':
				case 'M':
					result = Multiply(a, b);
					break;

				case 'd':
				case 'D':
					result = Divide(a, b);
					break;
			}

			Console.WriteLine("Result: {0}", result);
			Console.ReadKey();
		}
	}
}
