using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Oct_22_Exercise3
{
    class Program
    {
        static void Main(string[] args)
        {
            double hours = 0.0;
            double rate = 0.0;
            double grossPay = 0.0;

            hours = ConsoleUtils.GetDouble("Enter hours: ", 0.0, double.MaxValue);
            rate = ConsoleUtils.GetDouble("Enter rate: ", 0.0, double.MaxValue);

            grossPay = PayCalc.CalculateGrossPay(hours, rate);
            Console.WriteLine("Gross pay: {0}", grossPay);
            Console.ReadKey();
        }
    }
}
