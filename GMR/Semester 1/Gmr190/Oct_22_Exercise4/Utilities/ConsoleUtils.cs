using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public static class ConsoleUtils
    {
        public static int GetInt(string message, int min, int max)
        {
            int returnVal = min;

            while (true)
            {
                Console.Write(message);

                try
                {
                    returnVal = Convert.ToInt32(Console.ReadLine());

                    if (returnVal >= min && returnVal <= max)
                        return returnVal;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unacceptable input. " + e.Message);
                }
            }
        }

        public static int GetInt(string message)
        {
            return GetInt(message, int.MinValue, int.MaxValue);
        }

        public static double GetDouble(string message, double min, double max)
        {
            double returnVal = min;

            while (true)
            {
                Console.Write(message);

                try
                {
                    returnVal = Convert.ToDouble(Console.ReadLine());

                    if (returnVal >= min && returnVal <= max)
                        return returnVal;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unacceptable input. " + e.Message);
                }
            }
        }

        public static double GetDouble(string message)
        {
            return GetDouble(message, double.MinValue, double.MaxValue);
        }
    }
}
