using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab01
{
	class Program
	{
		private static Random rand;

		public static int GrabDouble (string message)
		{
			int ret = 0;
			while (true)
			{
				try
				{
					Console.Write (message);
					ret = Convert.ToInt32 (Console.ReadLine ());
				}
				catch (Exception e)
				{
					Console.WriteLine ("Invalid input: " + e.Message);
					continue;
				}

				return ret;
			}
		}

		static char ProcessMenu (string message, string validOptions)
		{
			char ret = ' ';

			do
			{
				Console.Write (message);
				string input = Console.ReadLine ();

				if (input.Length != 1)
				{
					Console.WriteLine ("Please select only one of the possibilities: " + validOptions);
					continue;
				}

				ret = input.ToUpper ()[0];
				if (validOptions.Contains (ret))
					return ret;
			} while (true);
		}

		static void Main (string[] args)
		{
			rand = new Random ();

			List<int> key = new List<int> ();
			GenerateKey (key);

			foreach (int i in key)
			{
				Console.Write ("{0} ", i);
			}
			Console.ReadKey ();
		}

		private static void GenerateKey (List <int> key)
		{
			do
			{
				key.Clear ();
				for (int i = 0; i < 4; ++i)
					key.Add (rand.Next (10));
			} while (!VerifyUniqueness (key));
		}

		private static bool VerifyUniqueness (List <int> key)
		{
			for (int i = 0; i < key.Count; ++i)
			{
				for (int j = 0; j < key.Count; ++j)
				{
					if (i != j)
					{
						if (key [i] == key [j])
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}
