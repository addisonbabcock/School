using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitConvertor2
{
	class Program
	{
		static void Main (string [] args)
		{
			VolumeConvertor volCon = new VolumeConvertor ();
			LengthConvertor lenCon = new LengthConvertor ();
			WeightConvertor weiCon = new WeightConvertor ();
			double orig = 0.0;
			double result = 0.0;
			int typeOfConvert = 0;
			int subType = 0;

			Console.WriteLine ("Unit Convertor");
			Console.WriteLine ("1. Length");
			Console.WriteLine ("2. Volume");
			Console.WriteLine ("3. Weight");
			Console.Write ("What type of value would you like to convert? ");
			typeOfConvert = Convert.ToInt32 (Console.ReadLine ());

			switch (typeOfConvert)
			{
				case 1:
					lenCon.ShowOptions ();
					break;

				case 2:
					volCon.ShowOptions ();
					break;

				case 3:
					weiCon.ShowOptions ();
					break;
			}
			Console.WriteLine ("Select units to convert: ");
			subType = Convert.ToInt32 (Console.ReadLine ());

			Console.Write ("What value would you like to convert? ");
			orig = Convert.ToDouble (Console.ReadLine ());

			switch (typeOfConvert)
			{
				case 1:
					result = lenCon.Convert (orig, subType);
					break;

				case 2:
					result = volCon.Convert (orig, subType);
					break;

				case 3:
					result = weiCon.Convert (orig, subType);
					break;
			}

			Console.WriteLine ("Result: {0}", result);
			Console.ReadKey ();
		}
	}
}
