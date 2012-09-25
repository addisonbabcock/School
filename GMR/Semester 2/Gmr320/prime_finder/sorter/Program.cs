using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> arr = new List<int>();

            int input = -1;
            do
            {
                input = Convert.ToInt32(Console.ReadLine());
                if (input == 0)
                    break;
                else
                    arr.Add(input);
            } while (true);

            for (int i = 0; i < arr.Count; ++i)
            {
                for (int j = i; j < arr.Count; ++j)
                {
                    if (arr[i] > arr[j])
                    {
                        int temp = arr[i];
                        arr[i] = arr[j];
                        arr[j] = temp;
                    }
                }
            }

            Console.WriteLine();
            foreach (int n in arr)
            {
                Console.WriteLine("{0}", n);
            }

            Console.ReadKey();
        }
    }
}
