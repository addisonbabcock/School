using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prime_finder
{
    class Program
    {
        static void Main(string[] args)
        {
            var max = Convert.ToInt32(Console.ReadLine());

            for (int n = 2; n < max / 2; ++n)
            {
                if (max % n != 0)
                {
                    Console.WriteLine("{0}", n);
                }
            }

            Console.ReadKey();
        }
    }
}
