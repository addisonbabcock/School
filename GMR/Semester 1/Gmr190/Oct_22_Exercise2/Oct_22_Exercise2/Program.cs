using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;

namespace Oct_22_Exercise2
{
    class Program
    {
        static void Main(string[] args)
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            hours = ConsoleUtils.GetInt("Enter hours: ");
            minutes = ConsoleUtils.GetInt("Enter minutes: ");
            seconds = ConsoleUtils.GetInt("Enter seconds: ");

            TimeSpan timeSpan = new TimeSpan(hours, minutes, seconds);
            Console.WriteLine("Total seconds: {0}", timeSpan.TotalSeconds);
            Console.ReadKey();
        }
    }
}
