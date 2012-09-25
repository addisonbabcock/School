using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace switch_example
{
	class Program
	{
		public static double GrabDouble(string message)
		{
			double ret = 0;
			while (true)
			{
				try
				{
					Console.Write(message);
					ret = Convert.ToDouble(Console.ReadLine());
				}
				catch (Exception e)
				{
					Console.WriteLine("Invalid input: " + e.Message);
					continue;
				}

				return ret;
			}
		}

		static void Main(string[] args)
		{
			double num1;
			double num2;
			string op;
			double result = 0.0;

			num1 = GrabDouble("Please enter a number: ");

			Console.Write("Please enter an operator: ");
			op = Console.ReadLine();

			num2 = GrabDouble("Please enter another number: ");

			switch (op)
			{
				case "+":
					result = num1 + num2;
					break;

				case "-":
					result = num1 - num2;
					break;

				case "*":
					result = num1 * num2;
					break;

				case "/":
					result = num1 / num2;
					break;

				default:
					Console.WriteLine("Unknown operator: {0}", op);
					break;
			}

			Console.WriteLine("Result: {0}", result);
			Console.ReadKey();
		}
	}
}
