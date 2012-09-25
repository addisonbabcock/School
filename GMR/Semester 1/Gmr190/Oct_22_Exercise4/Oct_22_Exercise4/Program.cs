using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Oct_22_Exercise4
{
    class Program
    {
        static void Main(string[] args)
        {
            double radius = 0.0, height = 0.0, volume = 0.0;

            radius = ConsoleUtils.GetDouble("Enter radius: ", 0.0, double.MaxValue);
            height = ConsoleUtils.GetDouble("Enter height: ", 0.0, double.MaxValue);

            volume = VolumeCalc.CalcConeVolume(radius, height);
            Console.WriteLine("Cone volume: {0}", volume);
            Console.ReadKey();
        }
    }
}
