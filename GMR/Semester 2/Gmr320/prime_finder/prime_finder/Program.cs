using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prime_finder
{
    class Program
    {
        static bool IsPrime(int check)
        {
            int a = check / 2;
            while (a >= 2)
            {
                if (check % a == 0)
                    return true;
                --a;
            }
            return false;
        }

        static void Main(string[] args)
        {
            var max = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            for (int n = 0; n < max; ++n)
            {
                if (!IsPrime (n))
                {
                    Console.WriteLine("{0}", n);
                }
            }

            Console.ReadKey();
        }
    }
}
