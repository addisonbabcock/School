using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitConvertor2
{
	class VolumeConvertor
	{
		public void ShowOptions ()
		{
			Console.WriteLine ("Volume options:");
			Console.WriteLine ("1. Liters to US Gallons");
			Console.WriteLine ("2. US Gallons to Liters");
		}

		public double Convert (double _in, int convertType)
		{
			double output = 0.0;

			switch (convertType)
			{
				case 1:
					output = _in * 0.26;
					break;

				case 2:
					output = _in * 3.78;
					break;

				default:
					throw new InvalidOperationException ();
			}

			return output;
		}
	}
}
