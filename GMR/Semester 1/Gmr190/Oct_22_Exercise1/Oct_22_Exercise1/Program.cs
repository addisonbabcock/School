using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Oct_22_Exercise1
{
    class Program
    {
        static void Main(string[] args)
        {
            int pennies;
            int nickels;
            int dimes;
            int quarters;

            pennies = ConsoleUtils.GetInt("Enter pennies: ");
            nickels = ConsoleUtils.GetInt("Enter nickels: ");
            dimes = ConsoleUtils.GetInt("Enter dimes: ");
            quarters = ConsoleUtils.GetInt("Enter quarters: ");

            CalculateChange calc = new CalculateChange(pennies, nickels, dimes, quarters);
            int total = calc.CalculateTotalChange();

            Console.WriteLine("Total change is: {0}", total);
            Console.ReadKey();
        }
    }
}
