using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_01_ICA01
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
            int classes = 0;
            int entries = 0;
            string answer = "";
            int grade = 0;
            double average = 0.0;

            classes = GrabInt("Please enter in number of classes: ");

            do
            {
                entries = 0;
                average = 0.0;

                do
                {
                    ++entries;
                    grade = GrabInt("Please enter grade for student " + entries + ": ");
                    average += grade;

                    Console.Write("Do you wish to continue (Y/N)? ");
                    answer = Console.ReadLine();

                } while (answer.ToString ().ToUpper ().Contains ("Y"));

                --classes;
                average /= entries;

                Console.WriteLine("The class average for this class = {0}", average);

            } while (classes > 0);

            Console.WriteLine("Thanks, bye for now!");
            Console.ReadKey();
        }
    }
}
