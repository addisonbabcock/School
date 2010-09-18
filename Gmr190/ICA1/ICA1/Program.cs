using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ICA1
{
    class Program
    {
        static void Main(string[] args)
        {
            double Subtotal;
            double Total;
            string sSubtotal;

            while (true)
            {
                Console.WriteLine("Please enter the Subtotal: ");
                sSubtotal = Console.ReadLine();

                try
                {
                    Subtotal = Convert.ToDouble(sSubtotal);
                }
                catch (FormatException ex)
                {
                    continue;
                }
                catch (OverflowException ex)
                {
                    continue;
                }

                break;
            }

            Total = Subtotal * 1.07;

            Console.WriteLine("Total: {0}", Total);

            Console.ReadLine();
        }
    }
}
