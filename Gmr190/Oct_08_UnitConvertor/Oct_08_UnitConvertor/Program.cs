using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_08_UnitConvertor
{
    class Program
    {
        public static double GrabDouble(string message)
        {
            double ret = 0;
            while (true)
            {
                try
                {
                    Console.Write(message);
                    ret = Convert.ToDouble(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input: " + e.Message);
                    continue;
                }

                return ret;
            }
        }

        static char ProcessMenu(string message, string validOptions)
        {
            char ret = ' ';

            do
            {
                Console.Write (message);
                string input = Console.ReadLine();

                if (input.Length != 1)
                {
                    Console.WriteLine("Please select only one of the possibilities: " + validOptions);
                    continue;
                }

                ret = input.ToUpper()[0];
                if (validOptions.Contains (ret))
                    return ret;
            } while (true);
        }

        static void Main(string[] args)
        {
            char convertType = ' ';
            char keepGoing = 'y';

            do
            {
                convertType = ProcessMenu("Please select a type of unit to convert ((L)ength, (W)eight , (V)olume): ", "LWV");

                switch (convertType)
                {
                    case 'L':
                        ConvertLength();
                        break;

                    case 'W':
                        ConvertWeight();
                        break;

                    case 'V':
                        ConvertVolume();
                        break;

                    default:
                        Console.WriteLine("Invalid menu selection, please try again");
                        continue;
                }

                keepGoing = ProcessMenu("Would you like to convert another value (Y/N)? ", "YN");

            } while (keepGoing.ToString().ToUpper().Contains('Y'));
        }

        private static void ConvertVolume()
        {
            char secondaryConvertType = ' ';
            double convertValue = 0.0;
            double convertedValue = 0.0;

            secondaryConvertType = ProcessMenu("Would you like to convert\n1. Liters to US gallons\n2. US gallons to liters\n", "12");
            convertValue = GrabDouble("Please enter the value to convert: ");

            switch (secondaryConvertType)
            {
                case '1':
                    convertedValue = 0.26 * convertValue;
                    Console.WriteLine("{0} US gallons", convertedValue);
                    break;

                case '2':
                    convertedValue = 3.78 * convertValue;
                    Console.WriteLine("{0} liters", convertedValue);
                    break;

                default:
                    Console.WriteLine("I don't know what to convert!");
                    break;
            }
        }

        private static void ConvertWeight()
        {
            char secondaryConvertType = ' ';
            double convertValue = 0.0;
            double convertedValue = 0.0;

            secondaryConvertType = ProcessMenu("Would you like to convert\n1. Kilograms to pounds\n2. Pounds to kilograms\n", "12");
            convertValue = GrabDouble("Please enter the value to convert: ");

            switch (secondaryConvertType)
            {
                case '1':
                    convertedValue = 2.2 * convertValue;
                    Console.WriteLine("{0} pounds", convertedValue);
                    break;

                case '2':
                    convertedValue = 0.45 * convertValue;
                    Console.WriteLine("{0} kilograms", convertedValue);
                    break;

                default:
                    Console.WriteLine("I don't know what to convert!");
                    break;
            }
        }

        private static void ConvertLength()
        {
            char secondaryConvertType = ' ';
            double convertValue = 0.0;
            double convertedValue = 0.0;

            secondaryConvertType = ProcessMenu("Would you like to convert\n1. Meters to feet\n2. Feet to meters\n", "12");
            convertValue = GrabDouble("Please enter the value to convert: ");

            switch (secondaryConvertType)
            {
                case '1':
                    convertedValue = 3.28 * convertValue;
                    Console.WriteLine("{0} feet", convertedValue);
                    break;

                case '2':
                    convertedValue = 0.3 * convertValue;
                    Console.WriteLine("{0} meters", convertedValue);
                    break;

                default:
                    Console.WriteLine("I don't know what to convert!");
                    break;
            }
        }
    }
}
