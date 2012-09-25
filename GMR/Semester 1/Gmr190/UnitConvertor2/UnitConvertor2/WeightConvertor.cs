using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitConvertor2
{
	class WeightConvertor
	{
		public void ShowOptions ()
		{
			Console.WriteLine ("Weight options:");
			Console.WriteLine ("1. Kilograms to Pounds");
			Console.WriteLine ("2. Pounds to Kilograms");
		}

		public double Convert (double _in, int convertType)
		{
			double output = 0.0;

			switch (convertType)
			{
				case 1:
					output = _in * 2.2;
					break;

				case 2:
					output = _in * 0.45;
					break;

				default:
					throw new InvalidOperationException ();
			}

			return output;
		}
	}
}
