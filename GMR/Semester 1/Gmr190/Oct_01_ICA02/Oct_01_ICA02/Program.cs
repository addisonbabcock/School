using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oct_01_ICA02
{
    class Program
    {
        static void Main(string[] args)
        {
            string cont = "";
          //  int remainder = 0;
            int number = 0;
            int divideby = 0;
            bool prime = false;
            bool validnum = false;

            Console.Write("Do you want to check a number (Y/N)? ");
            cont = Console.ReadLine().ToUpper ();

            while (cont.Contains("Y"))
            {
                do
                {
                    Console.WriteLine("Enter the number: ");
                    validnum = false;

                    try
                    {
                        number = Convert.ToInt32(Console.ReadLine());

                        if (number < 1 || number > 99)
                        {
                            validnum = false;
                            Console.WriteLine("Number must be between 1 and 99 inclusive");
                        }
                        else
                        {
                            validnum = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Invalid input: " + e.Message);
                    }

                } while (!validnum);

                prime = true;
                divideby = 2;

                do
                {
                    if (number % divideby == 0)
                    {
                        prime = false;
                    }

                    ++divideby;

                } while (prime && divideby < (number / 2));

                if (prime || number == 2)
                {
                    Console.WriteLine("PRIME!");
                }
                else
                {
                    Console.WriteLine("NOT PRIME!");
                }

                Console.Write("Do you want to check another number (Y/N)? ");
                cont = Console.ReadLine().ToUpper();
            }

            Console.WriteLine("Thanks for playing!");
            Console.ReadKey();
        }
    }
}
