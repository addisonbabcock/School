using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            GameObject obj = new GameObject("Weapon", 100, 100);

            Console.WriteLine("Weapon name: {0}", obj.GetObjectName());
            Console.WriteLine("Current duration: {0}", obj.GetCurrentDuration());
            Console.WriteLine("Max duration: {0}", obj.GetMaxDuration());
            Console.WriteLine("");

            obj.SetCurrentDuration(50);
            Console.WriteLine("Current duration: {0}", obj.GetCurrentDuration());
            Console.WriteLine("");

            obj.TakeDamage(25);
            Console.WriteLine("Current duration: {0}", obj.GetCurrentDuration());
            Console.WriteLine("");

            obj.Repair();
            Console.WriteLine("Current duration: {0}", obj.GetCurrentDuration());
            Console.WriteLine("");

            Console.ReadLine();
        }
    }
}
