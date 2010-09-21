using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA4
{
	class Program
	{
		static void Main(string[] args)
		{
			string name;
			const string me = "ADDISON";

			Console.Write("Please enter your name: ");
			name = Console.ReadLine();

			if (name.ToUpper() == me)
			{
				Console.WriteLine("Hello me!");
			}
			else
			{
				Console.WriteLine("Hello someone else.");
			}

			Console.ReadKey();
		}
	}
}
