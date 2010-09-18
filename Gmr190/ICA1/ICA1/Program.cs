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

            Console.WriteLine("Please enter the Subtotal: ");
            sSubtotal = Console.ReadLine();
            Subtotal = Convert.ToDouble(sSubtotal);

            Total = Subtotal * 1.07;

            Console.WriteLine("Total: {0}", Total);

            Console.ReadLine();
        }
    }
}
