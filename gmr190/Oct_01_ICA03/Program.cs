using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_01_ICA03
{
    class Program
    {
        public static int GrabInt(string message)
        {
            int ret = 0;
            while (true)
            {
                try
                {
                    Console.Write(message);
                    ret = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input: " + e.Message);
                    continue;
                }

                return ret;
            }
        }

        static void Main(string[] args)
        {
            int number = 0;
            int count = 1;

            number = GrabInt("Please enter in a positive integer: ");

            if (number > 0)
            {
                if (number > 1)
                {
                    while (number > 1)
                    {
                        if (number % 2 == 0)
                        {
                            number /= 2;
                        }
                        else
                        {
                            number = number * 3 + 1;
                        }

                        Console.WriteLine("Step {0}: Number = {1}", count, number);
                    }
                }
                else
                {
                    Console.WriteLine("Step {0}: Number = 1", count);
                }

                Console.WriteLine("This proves the hailstorm theory");
            }
            else
            {
                Console.WriteLine("Sorry the number you have entered is not valid; you must rerun the program.");
            }

            Console.ReadKey();
        }
    }
}
