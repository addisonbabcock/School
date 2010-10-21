using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitConvertor2
{
	class LengthConvertor
	{
		public void ShowOptions ()
		{
			Console.WriteLine ("Length options:");
			Console.WriteLine ("1. Meters to Feet");
			Console.WriteLine ("2. Feet to Meters");
		}

		public double Convert (double _in, int convertType)
		{
			double output = 0.0;

			switch (convertType)
			{
				case 1:
					output = _in * 3.28;
					break;

				case 2:
					output = _in * 0.3;
					break;

				default:
					throw new InvalidOperationException ();
			}

			return output;
		}
	}
}
