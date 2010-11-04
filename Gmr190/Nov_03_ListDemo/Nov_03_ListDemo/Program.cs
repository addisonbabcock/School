using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nov_03_ListDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> names = new List<string>();

            do
            {
                Console.Write("Please enter a name: ");
                names.Add(Console.ReadLine());
            } while (names.Count < 5);

            Console.Clear();
            foreach (string name in names)
            {
                Console.WriteLine("Here is a name: " + name);
            }

            bool found = false;
            do
            {
                Console.Write("Insert a name to search for: ");
                string search = Console.ReadLine();

                if (names.IndexOf(search) < 0)
                {
                    found = false;
                    Console.WriteLine("That is not the name you are looking for.");
                }
                else
                {
                    found = true;
                    Console.WriteLine("That is indeed a valid name.");
                }
            } while (!found);

            Console.ReadKey();
        }
    }
}
