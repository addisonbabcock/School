using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassExample3
{
	class Program
	{
		static void Main (string [] args)
		{
			MyPrint myPrint = new MyPrint ();
			myPrint.PrintHeader ("System Information");
			myPrint.PrintItem ("cpu");
			myPrint.PrintItem ("amd 2.2ghz");

			Console.ReadKey ();
		}
	}
}
