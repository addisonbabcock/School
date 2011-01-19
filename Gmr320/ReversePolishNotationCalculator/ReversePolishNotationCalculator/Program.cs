using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReversePolishNotationCalculator
{
	class Program
	{
		static void Main (string [] args)
		{
			double result = 0.0;
			Stack<double> numbers = new Stack<double> ();
			char [] splitters = {' '};

			Console.WriteLine ("What would you like me to calculate? ");
			string input = Console.ReadLine ();

			List<string> tokens = input.Split (splitters).ToList<string> ();

			foreach (string token in tokens)
			{
				try
				{
					double number = Convert.ToDouble (token);

					if (numbers.Count == 0)
						result = number;

					numbers.Push (number);
				}
				catch (FormatException)
				{
					switch (token)
					{
						case "+":
							result += numbers.Pop ();
							break;

						case "-":
							result -= numbers.Pop ();
							break;

						case "*":
							result *= numbers.Pop ();
							break;

						case "/":
							result /= numbers.Pop ();
							break;

						default:
							Console.WriteLine ("I don't know what {0} means.", token);
							break;
					}
				}
			}

			Console.WriteLine ("Result: {0}", result);
			Console.ReadKey ();
		}
	}
}
